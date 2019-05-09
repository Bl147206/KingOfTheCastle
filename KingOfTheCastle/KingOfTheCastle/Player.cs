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
            maxShieldHP, shieldHP, shieldRechargeRate, shieldTimer, roundKills, numRoundsWon;
        public Color playerColor, rangedColor, meleeColor;
        public Rectangle sourceRectangle;
        Direction previousFacing;
        int animationTimer;
        SoundEffect meleeSound;
        SoundEffect rangedSound;
        Bar shieldBar;
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
            kills = 0;
            jumps = 0;
            roundKills = 0;
            facing = Direction.Right;
            completedMainQuest = false;
            sourceRectangle = new Rectangle(0, 0, 64, 64);
            this.playerColor = rangedColor = meleeColor = color;
            animationTimer = 0;//Used for walking animation so the character does not bounce too quickly
            mAttack = 2;
            mAttackSpeed = .5;

            rAttack = 2;
            rAttackSpeed = .65;

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
            meleeSound = game.Content.Load<SoundEffect>("swordSoundO");
            rangedSound = game.Content.Load<SoundEffect>("bowSoundO");

            shieldBar = new Bar(maxShieldHP, shieldHP, new Rectangle(((playerNumber - 1) * 400) + 100,Globals.screenH - 80, 330, 20), color, game);
        }


        public void Update(Platform[] platforms)
        {
            GamePadState gamePad = GamePad.GetState(playerIndex);

            if(health <= 0)
            {
                kill();
            }

            shieldLogic(gamePad);

            dashLogic(gamePad);

            horizontalMovement(gamePad);

            jumpLogic(gamePad);

            gravityLogic();

            rangedLogic(gamePad);

            meleeLogic(gamePad);

            platformLogic(gamePad, platforms);

            animationLogic();

            x += xVelocity;
            y += yVelocity;
            UpdatePosition(x, y);
            previousFacing = facing;

            shieldBar.update(shieldHP);

            oldGamePad = gamePad;
        }

        public void shieldLogic(GamePadState gamePad)
        {
            shielding = false;
            if(gamePad.IsButtonDown(Buttons.RightShoulder) && shieldHP > 0)
            {
                shieldTimer = 0;
                shielding = true;
            }
            else
            {
                shieldTimer++;
                if(shieldTimer == shieldRechargeRate)
                {
                    shieldHP++;
                    shieldTimer = 0;
                    if(shieldHP > maxShieldHP)
                    {
                        shieldHP = maxShieldHP;
                    }
                }
            }
        }

        public void animationLogic()
        {
            if (!isDashing)
            {
                animationTimer++;
                if (yVelocity == 0)//No jumping
                {
                    if (facing == Direction.Left && xVelocity == 0)//No movement, facing left
                    {
                        sourceRectangle.X = 0;
                        sourceRectangle.Y = 0;
                        animationTimer = 0;
                    }
                    if (facing == Direction.Right && xVelocity == 0)//No movement, facing right.
                    {
                        sourceRectangle.X = 0;
                        sourceRectangle.Y = 64;
                        animationTimer = 0;
                    }
                    if (facing == Direction.Left && xVelocity != 0)//No jump, moving left
                    {

                        sourceRectangle.Y = 0;

                        if (previousFacing == Direction.Right)
                        {
                            sourceRectangle.X = 0;
                            animationTimer = 0;
                        }
                        else
                        {
                            if (animationTimer == 4)
                            {
                                animationTimer = 0;
                                if (sourceRectangle.X != 320)
                                    sourceRectangle.X += 64;
                                else
                                    sourceRectangle.X = 0;
                            }
                        }
                    }
                    if (facing == Direction.Right && xVelocity != 0)//No jump, moving left
                    {

                        sourceRectangle.Y = 64;

                        if (previousFacing == Direction.Left)
                        {
                            sourceRectangle.X = 0;
                            animationTimer = 0;
                        }
                        else
                        {
                            if (animationTimer == 4)
                            {
                                animationTimer = 0;
                                if (sourceRectangle.X != 320)
                                    sourceRectangle.X += 64;
                                else
                                    sourceRectangle.X = 0;
                            }
                        }
                    }
                }
                if (yVelocity != 0)//In the air
                {
                    animationTimer = 0;
                    if (facing == Direction.Right)
                    {
                        if (Math.Abs(yVelocity) >= 23)//Max Y
                        {
                            sourceRectangle.X = 4 * 64;
                            sourceRectangle.Y = 2 * 64;
                        }
                        if (Math.Abs(yVelocity) < 23 && Math.Abs(yVelocity) >= 19)
                        {
                            sourceRectangle.X = 5 * 64;
                            sourceRectangle.Y = 2 * 64;
                        }
                        if (Math.Abs(yVelocity) < 19 && Math.Abs(yVelocity) >= 13)
                        {
                            sourceRectangle.X = 0;
                            sourceRectangle.Y = 3 * 64;
                        }
                        if (Math.Abs(yVelocity) < 13 && Math.Abs(yVelocity) >= 7)
                        {
                            sourceRectangle.X = 1 * 64;
                            sourceRectangle.Y = 3 * 64;
                        }
                        if (Math.Abs(yVelocity) < 2 && Math.Abs(yVelocity) > 0)//Almost 0 Velocity
                        {
                            sourceRectangle.X = 2 * 64;
                            sourceRectangle.Y = 3 * 64;
                        }
                    }
                    if (facing == Direction.Left)
                    {
                        if (Math.Abs(yVelocity) >= 23)//Max Y
                        {
                            sourceRectangle.X = 2 * 64;
                            sourceRectangle.Y = 4 * 64;
                        }
                        if (Math.Abs(yVelocity) < 23 && Math.Abs(yVelocity) >= 19)
                        {
                            sourceRectangle.X = 0;
                            sourceRectangle.Y = 4 * 64;
                        }
                        if (Math.Abs(yVelocity) < 19 && Math.Abs(yVelocity) >= 13)
                        {
                            sourceRectangle.X = 5 * 64;
                            sourceRectangle.Y = 3 * 64;
                        }
                        if (Math.Abs(yVelocity) < 13 && Math.Abs(yVelocity) >= 7)
                        {
                            sourceRectangle.X = 4 * 64;
                            sourceRectangle.Y = 3 * 64;
                        }
                        if (Math.Abs(yVelocity) < 7 && Math.Abs(yVelocity) > 0)//Almost 0 Velocity
                        {
                            sourceRectangle.X = 3 * 64;
                            sourceRectangle.Y = 3 * 64;
                        }
                    }
                }
            }
            else
            {
                switch(facing)
                {
                    case Direction.Left:
                        sourceRectangle.X = 2 * 64;
                        sourceRectangle.Y = 5 * 64;
                        break;
                    case Direction.Right:
                        sourceRectangle.X = 3 * 64;
                        sourceRectangle.Y = 5 * 64;
                        break;
                }
            }
        }

        public void dashLogic(GamePadState gamePad)
        {
            if ((gamePad.ThumbSticks.Right.Y != 0 || gamePad.ThumbSticks.Right.X != 0) && dashTimer == 0 && !shielding)
            {// Dashing
                double normalizer = Math.Abs(gamePad.ThumbSticks.Right.Y) + Math.Abs(gamePad.ThumbSticks.Right.X);
                xVelocity += ((double)gamePad.ThumbSticks.Right.X / normalizer) * (double) dashSpeed;
                if(gamePad.ThumbSticks.Right.Y < 0)
                {// can only dash down
                    yVelocity -= ((double)gamePad.ThumbSticks.Right.Y / normalizer) * (double)dashSpeed;

                }
                else
                {
                    yVelocity = 0;
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
            if(rAttackTimer == 0 && !shielding)
            {
                if (gamePad.Triggers.Left > 0)
                {
                    isRAttacking = true;
                    rangedAttack();
                    rAttackTimer = (int)(60 * (1/rAttackSpeed));
                    rangedSound.Play();
                }
            }
            else if (rAttackTimer > 0)
            {
                rAttackTimer--;
            }
        }

        public void meleeLogic(GamePadState gamePad)
        {
            isMAttacking = false;
            if(mAttackTimer == 0 && !shielding)
            {
                if (gamePad.Triggers.Right > 0)
                {
                    isMAttacking = true;
                    meleeAttack(new Rectangle(location.X, location.Y, 200, 200), mAttack);
                    mAttackTimer = (int) (60 * (1/mAttackSpeed));
                    meleeSound.Play();
                }
            }
            else if (mAttackTimer > 0)
            {
                mAttackTimer--;
            }
        }

        public void platformLogic(GamePadState gamePad, Platform[] platforms)
        {
            if (gamePad.ThumbSticks.Left.Y < -.5 && location.Y + location.Height < Globals.screenH - heightUpToNotFallThrough && !shielding)
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
            if (Math.Abs(gamePad.ThumbSticks.Left.X) > 0 && !isDashing && !shielding) //When holding down a stick x change
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
            if (onGround && !shielding)
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
            else if (!airJumpUsed && !shielding)
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
            if (!onGround&&!isDashing)
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
            if(shielding)
            { //Shielding textures
                game.spriteBatch.Draw(texture,  location, sourceRectangle, Color.Black);
            }
            else
            { //Normal textures
                game.spriteBatch.Draw(texture, location, sourceRectangle, playerColor);
            }
            game.spriteBatch.DrawString(game.playerFont, "P"+playerNumber+"| HP: "+health.ToString() + " Kills: " + kills+" |", 
                new Vector2(((playerNumber-1) * 400)+100, Globals.screenH - game.font.LineSpacing * 1), playerColor);
            if (isMAttacking)
            {// temp stuff for weapon testing
                if(facing==Direction.Left)
                    game.spriteBatch.Draw(game.swordAttackT, attackRec,new Rectangle(0,0,64,64), meleeColor, 0,new Vector2(0,0),SpriteEffects.FlipHorizontally,0);
                else
                    game.spriteBatch.Draw(game.swordAttackT, attackRec, meleeColor);
            }
            shieldBar.draw();
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
            if (shielding)
            { //Shield blocks
                shieldHP -= damageAmount;
                if(shieldHP < 0)
                { //if the attack did more damage than the shield can block
                    damageAmount = shieldHP * -1;
                }
                else
                { //if the shield blocks all the damage just return
                    return;
                }
            }
            health -= damageAmount;
            if(health <= 0)
            {
                kill();
                game.players[attacker - 1].kills += 1;
                game.players[attacker - 1].roundKills++;
                game.players[attacker - 1].gold += 10;
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
            roundKills = 0;
            shieldHP = maxShieldHP;
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
                if(p != null && p != this && p.IsAlive())
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
            Texture2D tex=null;
            int pXVel = 20;
            Rectangle pHitBox = new Rectangle(0,0,60,15);
            switch (facing)
            {
                case Direction.Left:
                    pXVel *= -1;
                    pHitBox.X = location.X - pHitBox.Width;
                    tex = game.arrow;
                    break;
                case Direction.Right:
                    pXVel *= 1;
                    pHitBox.X = location.X + location.Width;
                    tex = game.arrowF;
                    break;
            }
            pHitBox.Y = (int)((double)location.Y + ((double)location.Height / 2) - ((double)pHitBox.Height / 2));
            ProjectileHandler.Projectile projectile;
            projectile = new ProjectileHandler.Projectile(tex, pHitBox, playerNumber, pXVel, rAttack, rangedColor);
            stage.projectiles.add(projectile);
        }
    }
}
