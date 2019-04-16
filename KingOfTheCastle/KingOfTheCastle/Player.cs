using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace KingOfTheCastle
{
    public class Player
    {
        enum Direction { Left, Right}

        KingOfTheCastle game;
        Rectangle location;
        Rectangle window;
        Rectangle attackRec; //temp for testing
        Direction facing;
        public Texture2D texture;
        public PlayerIndex playerIndex;
        bool onGround, fallingThroughPlatform, isAlive, isMAttacking, isRAttacking;
        public int playerNumber, maxXVelocity, jumpForce, gold, maxHealth, health, rAttackTimer,
            mAttack, rAttack, mAttackTimer, intersectingPlatforms, heightUpToNotFallThrough;
        Color color;
        //more specific x and y coords
        double x, y, xVelocity, yVelocity, xAccel, gravity, groundFrictionForce, mAttackSpeed, rAttackSpeed, terminalVelocity;

        public Player(KingOfTheCastle game, Rectangle spawnLocation, Texture2D texture, int playerIndex, Color color)
        {
            //stuff that gets shared by all players at the start
            health = 20;
            maxHealth = 20;
            xVelocity = 0;
            yVelocity = 0;
            xAccel = 3;
            gravity = 1;
            groundFrictionForce = 2; //decrease in x velocity when you're not holding a direction
            jumpForce = 30; //intial force of a jump
            maxXVelocity = 15;
            terminalVelocity = 20;
            heightUpToNotFallThrough = 180; //distance from the bottom of the screen you stop being able to fall through platforms at
            fallingThroughPlatform = false;
            isAlive = true;
            mAttackSpeed = .5;
            facing = Direction.Right;

            this.color = color;

            mAttack = 2;
            mAttackSpeed = 0.65;

            rAttack = 2;
            rAttackSpeed = 0.75;

            this.game = game;
            location = spawnLocation;
            this.texture = texture;
            this.playerNumber = playerIndex;
            switch (playerIndex)
            {
                case 1:
                    this.playerIndex = PlayerIndex.One;
                    break;
                case 2:
                    this.playerIndex = PlayerIndex.Two;
                    break;
                case 3:
                    this.playerIndex = PlayerIndex.Three;
                    break;
                case 4:
                    this.playerIndex = PlayerIndex.Four;
                    break;
                default:
                    Console.WriteLine("invalid player created");
                    Environment.Exit(0);
                    break;
            }
            window = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
            x = location.X;
            y = location.Y;
        }

        public void Update(Platform[] platforms)
        {
            GamePadState gamePad = GamePad.GetState(playerIndex);
            //temp life testing stuff
            if(gamePad.DPad.Up == ButtonState.Pressed)
            {
                kill();
            }

            if(health <= 0)
            {
                kill();
            }

            rangedLogic(gamePad);

            meleeLogic(gamePad);

            platformLogic(gamePad, platforms);

            horizontalMovement(gamePad);

            jumpLogic(gamePad);

            gravityLogic();

            x += xVelocity;
            y += yVelocity;
            UpdatePosition(x, y);
        }

        public void rangedLogic(GamePadState gamePad)
        {
            isRAttacking = false;
            if(rAttackTimer == 0)
            {
                if (gamePad.Triggers.Left > 0)
                {
                    isRAttacking = true;
                    rangedAttack();
                    rAttackTimer = (int)(60 * rAttackSpeed);
                }
            }
            else
            {
                rAttackTimer--;
            }
        }

        public void meleeLogic(GamePadState gamePad)
        {
            isMAttacking = false;
            if(mAttackTimer == 0)
            {
                if (gamePad.Triggers.Right > 0)
                {
                    isMAttacking = true;
                    meleeAttack(new Rectangle(location.X, location.Y, 200, 200), 10);
                    mAttackTimer = (int) (60 * mAttackSpeed);
                }
            }
            else
            {
                mAttackTimer--;
            }
        }

        public void platformLogic(GamePadState gamePad, Platform[] platforms)
        {
            if (gamePad.ThumbSticks.Left.Y < -.5 && location.Y + location.Height < Globals.screenH - heightUpToNotFallThrough)
            {// let the player fall through platforms when they're holding down
                fallingThroughPlatform = true;
            }
            foreach (Platform p in platforms)
            {
                if (p != null)
                {
                    if (fallingThroughPlatform)
                    {// check if the player has cleared the platform they're falling through
                        if (location.Intersects(p.destination))
                        {
                            intersectingPlatforms++;
                        }
                    }
                    else
                    {
                        if (yVelocity >= 0)
                        {
                            location.Y += (int)yVelocity;
                            if (location.Intersects(p.destination))
                            {// if a player is falling and they're in a platform snap them to the top
                                onGround = true;
                                y = p.destination.Y - location.Height;
                                yVelocity = 0;
                                break;
                            }
                            else
                            {
                                location.Y -= (int)yVelocity;
                            }
                        }
                    }
                }
                onGround = false;
            }

            if (fallingThroughPlatform && intersectingPlatforms == 0)
            {
                fallingThroughPlatform = false;
            }
            intersectingPlatforms = 0;
        }

        public void horizontalMovement(GamePadState gamePad)
        {
            if (Math.Abs(gamePad.ThumbSticks.Left.X) > 0) //When holding down a stick x change
            {
                if(Math.Sign(xVelocity) != Math.Sign(gamePad.ThumbSticks.Left.X))
                {
                    xVelocity = 0;
                }
                xVelocity += gamePad.ThumbSticks.Left.X * xAccel;
                switch (Math.Sign(gamePad.ThumbSticks.Left.X))
                {
                    case 1:
                        facing = Direction.Right;
                        break;
                    case -1:
                        facing = Direction.Left;
                        break;
                }
            }
            else if (Math.Abs(xVelocity) > 0) //Slowing down when not holding a direction
            {
                if (Math.Abs(xVelocity) < groundFrictionForce && xVelocity != 0) //Making sure the player actaully stops
                {
                    xVelocity = 0;
                }
                else
                {
                    xVelocity -= Math.Sign(xVelocity) * groundFrictionForce;
                }
            }
            if (Math.Abs(xVelocity) > maxXVelocity)
            {
                xVelocity = Math.Sign(xVelocity) * maxXVelocity;
            }
        }

        public void jumpLogic(GamePadState gamePad)
        {
            if (onGround)
            {
                if (gamePad.IsButtonDown(Buttons.A)) //jumping
                {
                    yVelocity -= jumpForce;
                }
                else if (gamePad.IsButtonDown(Buttons.B)) //short jumping
                {
                    yVelocity -= jumpForce / 2;
                }
            }
        }

        public void gravityLogic()
        {
            if (!onGround)
            {
                yVelocity += gravity; //gravity decreasing y movement
                if (yVelocity > terminalVelocity)
                {
                    yVelocity = terminalVelocity;
                }
            }
        }

        public void UpdatePosition(double x, double y)
        {
            this.y = y;
            this.x = x;
            location.X = (int)this.x;
            location.Y = (int)this.y;
        }

        public void ResetPos(int x, int y)
        {
            UpdatePosition(x, y);
        }

        public void draw()
        {
            game.spriteBatch.Draw(texture, location, color);
            game.spriteBatch.DrawString(game.font, health.ToString(), 
                new Vector2(playerNumber * 50, Globals.screenH - game.font.LineSpacing * 1), Color.Red);
            if (isMAttacking)
            {// temp stuff for weapon testing
                game.spriteBatch.Draw(game.test, attackRec, color);
            }
        }

        public bool IsAlive()
        {
            return isAlive;
        }

        public void kill()
        {
            isAlive = false;
            health = 0;
        }

        public void damage(int damageAmount)
        {
            health -= damageAmount;
        }

        public void revive()
        {
            isAlive = true;
            health = maxHealth;
            location = new Rectangle(Globals.screenW / 2, Globals.screenH - 250, 60, 60);
            yVelocity = 0;
            xVelocity = 0;
            y = location.Y;
            x = location.X;
        }

        public void meleeAttack(Rectangle weaponHitbox, int weaponDamage)
        {
            weaponHitbox.Y = (int)((double)location.Y + ((double)location.Height / 2) - ((double)weaponHitbox.Height / 2));
            switch (facing)
            {
                case Direction.Left:
                    weaponHitbox.X = location.X - weaponHitbox.Width;
                    break;
                case Direction.Right:
                    weaponHitbox.X = location.X + location.Width;
                    break;
            }
            attackRec = weaponHitbox;
            foreach(Player p in game.players)
            {
                if(p != null && p != this)
                {
                    if (weaponHitbox.Intersects(p.location))
                    {
                        p.damage(weaponDamage);
                    }
                }
            }
        }

        public void rangedAttack()
        {
            Stage stage = (Stage) game.currentScreen;
            int pXVel = 0;
            Rectangle pHitBox = new Rectangle(0,0,40,10);
            switch (facing)
            {
                case Direction.Left:
                    pXVel = -10;
                    pHitBox.X = location.X - pHitBox.Width;
                    break;
                case Direction.Right:
                    pXVel = 10;
                    pHitBox.X = location.X + location.Width;
                    break;
            }
            pHitBox.Y = (int)((double)location.Y + ((double)location.Height / 2) - ((double)pHitBox.Height / 2));
            ProjectileHandler.Projectile projectile;
            projectile = new ProjectileHandler.Projectile(game.test, pHitBox, playerNumber, pXVel, 10, color);
            stage.projectiles.add(projectile);
        }
    }
}
