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
        public Rectangle location;
        Rectangle window;
        Rectangle attackRec; //temp for testing
        Direction facing;
        public GamePadState oldGamePad;
        public Texture2D texture;
        public PlayerIndex playerIndex;
        public bool onGround, fallingThroughPlatform, isAlive, isMAttacking, isRAttacking, airJumpUsed, isDashing, completedMainQuest, shielding;
        public int playerNumber, maxXVelocity, jumpForce, gold, maxHealth, health, rAttackTimer, shortJumpForce, dashSpeed,
            mAttack, rAttack, mAttackTimer, intersectingPlatforms, heightUpToNotFallThrough, kills, jumps, dashTimer, dashDelay, maxYVelocity,
            maxShieldHP, shieldHP, shieldRechargeRate;
        public Color playerColor, rangedColor, meleeColor;
        //more specific x and y coords
        public double x, y, xVelocity, yVelocity, xAccel, gravity, groundFrictionForce, mAttackSpeed, rAttackSpeed, terminalVelocity;

        public Player(KingOfTheCastle game, Rectangle spawnLocation, Texture2D texture, int playerIndex, Color color)
        {
            //stuff that gets shared by all players at the start
            health = 20;
            maxHealth = 20;
            maxShieldHP = 10;
            shieldHP = maxShieldHP;
            shieldRechargeRate = 20; //Recharges 1 point every x game ticks
            xVelocity = 0;
            yVelocity = 0;
            xAccel = 3;
            gravity = 1;
            dashSpeed = 30;
            gold = 20;
            dashDelay = 60; //in frames
            groundFrictionForce = 2; //decrease in x velocity when you're not holding a direction
            jumpForce = 23; //intial force of a jump
            shortJumpForce = 15;
            maxXVelocity = 15;
            maxYVelocity = -30;
            terminalVelocity = 20;
            heightUpToNotFallThrough = 180; //distance from the bottom of the screen you stop being able to fall through platforms at
            fallingThroughPlatform = false;
            isAlive = true;
            mAttackSpeed = .5;
            kills = 0;
            jumps = 0;
            facing = Direction.Right;
            completedMainQuest = false;

            this.playerColor = rangedColor = meleeColor = color;

            mAttack = 2;
            mAttackSpeed = 0.50;

            rAttack = 2;
            rAttackSpeed = 0.65;

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
            // temp life testing stuff
            if(gamePad.DPad.Up == ButtonState.Pressed)
            {
                kill();
            }

            if(health <= 0)
            {
                kill();
            }

            dashLogic(gamePad);

            horizontalMovement(gamePad);

            jumpLogic(gamePad);

            gravityLogic();

            rangedLogic(gamePad);

            meleeLogic(gamePad);

            platformLogic(gamePad, platforms);

            x += xVelocity;
            y += yVelocity;
            UpdatePosition(x, y);

            oldGamePad = gamePad;
        }

        public void shieldLogic(GamePadState gamePad)
        {
            if(gamePad.IsButtonDown(Buttons.RightShoulder) )
            {

            }
        }

        public void dashLogic(GamePadState gamePad)
        {
            if ((gamePad.ThumbSticks.Right.Y != 0 || gamePad.ThumbSticks.Right.X != 0) && dashTimer == 0)
            {// Dashing
                double normalizer = Math.Abs(gamePad.ThumbSticks.Right.Y) + Math.Abs(gamePad.ThumbSticks.Right.X);
                xVelocity += ((double)gamePad.ThumbSticks.Right.X / normalizer) * (double) dashSpeed;
                if(gamePad.ThumbSticks.Right.Y < 0)
                {// can only dash down
                    yVelocity -= ((double)gamePad.ThumbSticks.Right.Y / normalizer) * (double)dashSpeed;

                }
                dashTimer = dashDelay;
                isDashing = true; 
            }
            else if(dashTimer > 0)
            {
                dashTimer--;
            }
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
                    meleeAttack(new Rectangle(location.X, location.Y, 200, 200), mAttack);
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
                            // this helps to make sure the player dosen't partials fall through a platform before being pulled to the top
                            location.Y += (int)yVelocity;

                            // this is some super jank code that makes it check if the bottom part of a player intersects
                            location.Height /= 2;
                            location.Y += location.Height;

                            if (location.Intersects(p.destination))
                            {// if a player is falling and they're in a platform snap them to the top

                                // undoing the jank stuff from earlier
                                location.Y -= location.Height;
                                location.Height *= 2;

                                onGround = true;
                                y = p.destination.Y - location.Height+1;
                                yVelocity = 0;
                                airJumpUsed = false;
                                break;
                            }
                            else
                            {
                                location.Y -= (int)yVelocity;

                                // Undoing the jank stuff from earlier
                                location.Y -= location.Height;
                                location.Height *= 2;
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
            if (Math.Abs(gamePad.ThumbSticks.Left.X) > 0 && !isDashing) //When holding down a stick x change
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
                if (!isDashing)
                {
                    xVelocity = Math.Sign(xVelocity) * maxXVelocity;
                }
            }
            else if (isDashing)
            {
                isDashing = false;
            }
        }

        public void jumpLogic(GamePadState gamePad)
        {
            if (onGround)
            {
                if (gamePad.IsButtonDown(Buttons.A)) //jumping
                {
                    yVelocity -= jumpForce;
                    location.Y -= 1;
                    onGround = false;
                    airJumpUsed = false;
                    jumps++;
                }
                else if (gamePad.IsButtonDown(Buttons.B)) //short jumping
                {
                    yVelocity -= shortJumpForce;
                    location.Y -= 1;
                    onGround = false;
                    airJumpUsed = false;
                    
                }
            }
            else if (!airJumpUsed)
            {
                if (gamePad.IsButtonDown(Buttons.A) && !oldGamePad.IsButtonDown(Buttons.A)) 
                {
                    yVelocity = -jumpForce;
                    airJumpUsed = true;
                }
                else if (gamePad.IsButtonDown(Buttons.B) && !oldGamePad.IsButtonDown(Buttons.B)) 
                {
                    yVelocity = -jumpForce;
                    airJumpUsed = true;
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
                if(yVelocity < maxYVelocity)
                {
                    yVelocity = maxYVelocity;
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
            game.spriteBatch.Draw(texture, location, playerColor);
            game.spriteBatch.DrawString(game.font, health.ToString() + " " + kills, 
                new Vector2(playerNumber * 100, Globals.screenH - game.font.LineSpacing * 1), playerColor);
            if (isMAttacking)
            {// temp stuff for weapon testing
                game.spriteBatch.Draw(game.test, attackRec, meleeColor);
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

        public void damage(int damageAmount, int attacker)
        {
            health -= damageAmount;
            if(health <= 0)
            {
                kill();
                game.players[attacker-1].kills += 1;
            }
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

        public void spawn()
        {
            isAlive = true;
            health = maxHealth;
            location = new Rectangle(Globals.screenW / (2 * (playerNumber + 1)), Globals.screenH - (250 * (playerNumber + 1)), 60, 60);
            yVelocity = 0;
            xVelocity = 0;
            y = location.Y;
            x = location.X;
            completedMainQuest = false;
            jumps = 0;
            kills = 0;
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
                        p.damage(weaponDamage, playerNumber);
                    }
                }
            }
        }
    
        public void rangedAttack()
        {
            Stage stage = (Stage) game.currentScreen;
            int pXVel = 20;
            Rectangle pHitBox = new Rectangle(0,0,40,10);
            switch (facing)
            {
                case Direction.Left:
                    pXVel *= -1;
                    pHitBox.X = location.X - pHitBox.Width;
                    break;
                case Direction.Right:
                    pXVel *= 1;
                    pHitBox.X = location.X + location.Width;
                    break;
            }
            pHitBox.Y = (int)((double)location.Y + ((double)location.Height / 2) - ((double)pHitBox.Height / 2));
            ProjectileHandler.Projectile projectile;
            projectile = new ProjectileHandler.Projectile(game.test, pHitBox, playerNumber, pXVel, rAttack, rangedColor);
            stage.projectiles.add(projectile);
        }
    }
}
