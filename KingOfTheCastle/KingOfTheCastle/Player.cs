﻿using System;
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
        public Texture2D texture;
        public PlayerIndex playerIndex;
        bool onGround, fallingThroughPlatform, isAlive;
        int maxXVelocity, jumpForce, gold, health, mAttack, rAttack, intersectingPlatforms, heightUpToNotFallThrough;
        Color color;
        //more specific x and y coords
        double x, y, xVelocity, yVelocity, xAccel, gravity, groundFrictionForce, mAttackSpeed, rAttackSpeed, terminalVelocity;
        Inventory inventory;

        public Player(KingOfTheCastle game, Rectangle spawnLocation, Texture2D texture, int playerIndex, Color color)
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
            heightUpToNotFallThrough = 180;
            fallingThroughPlatform = false;
            isAlive = true;

            this.color = color;

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
            //temp life testing stuff
            if(gamePad.DPad.Up == ButtonState.Pressed)
            {
                kill();
            }
            if(gamePad.ThumbSticks.Left.Y < -.5 && location.Y + location.Height < Globals.screenH - heightUpToNotFallThrough)
            {
                fallingThroughPlatform = true;
            }
            foreach (Platform p in platforms)
            {
                if(p != null)
                {
                    if (fallingThroughPlatform)
                    {
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
                            {
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
            if(fallingThroughPlatform && intersectingPlatforms == 0)
            {
                fallingThroughPlatform = false;
            }
            intersectingPlatforms = 0;

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
                else if (gamePad.IsButtonDown(Buttons.B)) //short jumping
                {
                    yVelocity -= jumpForce / 2;
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
            game.spriteBatch.Draw(texture, location, color);
        }

        public bool IsAlive()
        {
            return isAlive;
        }

        public void kill()
        {

            isAlive = false;
        }

        public void revive()
        {
            isAlive = true;
            location = new Rectangle(Globals.screenW / 2, Globals.screenH - 250, 60, 60);
            yVelocity = 0;
            xVelocity = 0;
            y = location.Y;
            x = location.X;
        }

    }
}
