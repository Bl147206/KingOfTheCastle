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
    class Player
    {
        KingOfTheCastle game;
        Rectangle location;
        Rectangle window;
        Texture2D texture;
        PlayerIndex playerIndex;
        bool onGround;
        int maxXVelocity, jumpForce, gold, health;
        //more specific x and y coords
        double x, y, xVelocity, yVelocity, xAccel, gravity, groundFrictionForce;

        public Player(KingOfTheCastle game, Rectangle spawnLocation, Texture2D texture, int playerIndex)
        {
            //stuff that gets shared by all players at the start
            xVelocity = 0;
            yVelocity = 0;
            xAccel = 3;
            gravity = 6;
            groundFrictionForce = 5;

            this.game = game;
            location = spawnLocation;
            this.texture = texture;
            
            switch (playerIndex)
            {
                case 1:
                    this.playerIndex = PlayerIndex.One;
                    break;
                case 2:
                    this.playerIndex = PlayerIndex.One;
                    break;
                case 3:
                    this.playerIndex = PlayerIndex.One;
                    break;
                case 4:
                    this.playerIndex = PlayerIndex.One;
                    break;
            }
            window = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
            x = location.X;
            y = location.Y;
        }

        public void Update(Platform[] platforms)
        {
            GamePadState gamePad = GamePad.GetState(playerIndex);
            foreach (Platform p in platforms)
            {
                if (location.Y + location.Height == p.destination.Y)
                {
                    onGround = true;
                    break;
                }
            }
            //on ground movement 
            if (onGround)
            {
                if (gamePad.IsButtonDown(Buttons.A)) //jumping
                {
                    yVelocity = jumpForce;
                }
                if (Math.Abs(gamePad.ThumbSticks.Right.X) > 0) //When holding down a stick x change
                {
                    xVelocity += gamePad.ThumbSticks.Right.X * xAccel;
                }
                else if (Math.Abs(xVelocity) > 0) //Slowing down when not holding a direction
                {
                    if(Math.Abs(xVelocity) < groundFrictionForce && xVelocity != 0) //Making sure the player actaully stops
                    {
                        xVelocity = 0;
                    }
                    else
                    {
                        xVelocity += xVelocity - ((xVelocity / Math.Abs(xVelocity)) * groundFrictionForce);
                    }
                }
            }
            //in air movement
            if (!onGround)
            {
                //gravity decreasing y movement
                yVelocity += gravity;
            }
            x = x + xVelocity;
            y = y + yVelocity;
            UpdatePosition(x, y);
        }

        public void UpdatePosition(double x, double y)
        {
            this.y -= y;
            this.x += x;
            location.X = (int)this.x;
            location.Y = (int)this.y;
        }

        public void ResetPos(int x, int y)
        {
            UpdatePosition(x, y);
        }

        public void draw()
        {
            game.spriteBatch.Begin();
            game.spriteBatch.Draw(texture, location, Color.Black);
            game.spriteBatch.End();
        }

    }
}
