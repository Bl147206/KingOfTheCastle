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
        KingOfTheCastle game;
        Rectangle location;
        Rectangle window;
        Texture2D texture;
        PlayerIndex playerIndex;
        bool onGround, fallingThroughPlatform;
        int maxXVelocity, jumpForce, gold, health, mAttack, rAttack;
        //more specific x and y coords
        double x, y, xVelocity, yVelocity, xAccel, gravity, groundFrictionForce, mAttackSpeed, rAttackSpeed, terminalVelocity;
        Inventory inventory;

        public Player(KingOfTheCastle game, Rectangle spawnLocation, Texture2D texture, int playerIndex)
        {
            //stuff that gets shared by all players at the start
            xVelocity = 0;
            yVelocity = 0;
            xAccel = 3;
            gravity = 1;
            groundFrictionForce = 2;
            jumpForce = 30;
            maxXVelocity = 15;
            terminalVelocity = 20;
            fallingThroughPlatform = false;

            mAttack = 2;
            mAttackSpeed = 0.65;

            rAttack = 2;
            rAttackSpeed = 0.75;

            this.game = game;
            location = spawnLocation;
            this.texture = texture;
            
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
            if(gamePad.ThumbSticks.Left.Y < 0)
            {
                fallingThroughPlatform = true;
            }
            foreach (Platform p in platforms)
            {
                if(p != null)
                {
                    if (fallingThroughPlatform)
                    {
                        if (!location.Intersects(p.destination))
                        {
                            fallingThroughPlatform = false;
                        }
                    }
                    else
                    {
                        if (yVelocity >= 0)
                        {
                            if (location.Intersects(p.destination))
                            {
                                onGround = true;
                                y = p.destination.Y - location.Height;
                                yVelocity = 0;
                                break;
                            }
                        }
                    }
                }
                onGround = false;
            }
            if (Math.Abs(gamePad.ThumbSticks.Left.X) > 0) //When holding down a stick x change
            {
                xVelocity += gamePad.ThumbSticks.Left.X * xAccel;
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
            //on ground movement 
            if (onGround)
            {
                if (gamePad.IsButtonDown(Buttons.A)) //jumping
                {
                    yVelocity -= jumpForce;
                }
            }
            //in air movement
            if (!onGround)
            {
                yVelocity += gravity; //gravity decreasing y movement
                //yVelocity = 1;
                if(yVelocity > terminalVelocity)
                {
                    yVelocity = terminalVelocity;
                }
            }
            x += xVelocity;
            y += yVelocity;
            UpdatePosition(x, y);
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
            game.spriteBatch.Draw(texture, location, Color.Black);
        }

    }
}
