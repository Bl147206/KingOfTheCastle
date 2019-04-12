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
    class Shop: Screen
    {
        enum playerSelection
        {
            One,
            Two,
            Three
        };
        playerSelection[] p = new playerSelection[4];
        Rectangle[] pSelect = new Rectangle[4];
        GamePadState[] playerPad= new GamePadState[4];
        GamePadState[] playerPad0 = new GamePadState[4];
        Texture2D background;
        int frames;
        public Shop(KingOfTheCastle game)
        {
            background = game.shopText;
            for(int x = 0; x<pSelect.Length; x++)
            {
                p[x] = playerSelection.One;
            }
            pSelect[0] = new Rectangle(80, 20, 140, 135);//2 ==(80,185)
            pSelect[1] = new Rectangle(1095, 15, 140, 135);
            pSelect[2] = new Rectangle(80, 610, 140, 135);
            pSelect[3] = new Rectangle(1095, 600, 140, 135);
            this.game = game;
            frames = 0;
            game.round++;
        }
        public override void Update(GameTime gameTime) {
            playerPad[0] = GamePad.GetState(PlayerIndex.One);
            playerPad[1] = GamePad.GetState(PlayerIndex.Two);
            playerPad[2] = GamePad.GetState(PlayerIndex.Three);
            playerPad[3] = GamePad.GetState(PlayerIndex.Four);
            if (playerPad[0].DPad.Down==ButtonState.Pressed&& playerPad0[0].DPad.Down != ButtonState.Pressed)
            {
                if (p[0] != playerSelection.Three)
                {
                    p[0]++;
                }
                else
                {
                    p[0] = playerSelection.One;
                }
            }

            if (playerPad[0].DPad.Up == ButtonState.Pressed && playerPad0[0].DPad.Up != ButtonState.Pressed)
            {
                if (p[0] != playerSelection.One)
                {
                    p[0]--;
                }
                else
                {
                    p[0] = playerSelection.Three;
                }
            }
            //Player 1
            if (p[0] == playerSelection.One)
            {
                pSelect[0].Y = 20;
            }
            if (p[0] == playerSelection.Two)
            {
                pSelect[0].Y = 185;
            }
            if (p[0] == playerSelection.Three)
            {
                pSelect[0].Y = 360;
            }
            //Player 2
            if (p[1] == playerSelection.One)
            {
                pSelect[1].Y = 15;
            }
            if (p[1] == playerSelection.Two)
            {
                pSelect[1].Y = 175;
            }
            if (p[1] == playerSelection.Three)
            {
                pSelect[1].Y = 350;
            }
            //Player 3
            if (p[2] == playerSelection.One)
            {
                pSelect[2].Y = 610;
            }
            if (p[2] == playerSelection.Two)
            {
                pSelect[2].Y = 770;
            }
            if (p[2] == playerSelection.Three)
            {
                pSelect[2].Y = 945;
            }
            //Player 4
            if (p[3] == playerSelection.One)
            {
                pSelect[3].Y = 600;
            }
            if (p[3] == playerSelection.Two)
            {
                pSelect[3].Y = 770;
            }
            if (p[3] == playerSelection.Three)
            {
                pSelect[3].Y = 945;
            }

            playerPad0[0] = playerPad[0];
            playerPad0[1] = playerPad[1];
            playerPad0[2] = playerPad[2];
            playerPad0[3] = playerPad[3];
            frames++;
            if (frames >= 60 * (20))
                game.currentScreen = new Stage(game.round, this.game);
        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(new Color(180,140,100));
            game.spriteBatch.Draw(background, new Rectangle(0, 0, Globals.screenW, Globals.screenH), Color.White);
            foreach(Rectangle x in pSelect)
            {
                game.spriteBatch.Draw(game.shopHighlight, x, Color.White);
            }

        }
    }
}
