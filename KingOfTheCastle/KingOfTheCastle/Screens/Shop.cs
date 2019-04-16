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
        Rectangle[,] items = new Rectangle[4,3];
        Texture2D background;
        int frames;
        int seconds;
        String timeleft;
        Inventory[] inventories = new Inventory[4];
        public Shop(KingOfTheCastle game)
        {
            background = game.shopText;
            this.game = game;
            for(int x = 0; x<pSelect.Length; x++)
            {
                p[x] = playerSelection.One;
            }
            pSelect[0] = new Rectangle(screenAdjust(80, "W"), screenAdjust(20, "H"), screenAdjust(140, "W"), screenAdjust(135, "H"));//20,20,140,135
            pSelect[1] = new Rectangle(screenAdjust(1095, "W"), screenAdjust(15, "H"), screenAdjust(140, "W"), screenAdjust(135, "H"));//1095,15,140,135
            pSelect[2] = new Rectangle(screenAdjust(80, "W"), screenAdjust(610, "H"), screenAdjust(140, "W"), screenAdjust(135, "H"));//80,610,140,135
            pSelect[3] = new Rectangle(screenAdjust(1095, "W"), screenAdjust(600, "H"), screenAdjust(140, "W"), screenAdjust(135, "H"));//1095,600,140,135
            for (int x = 0; x<inventories.Length; x++)
            {
                inventories[x] = new Inventory(game.round, this.game);
            }
            items[0, 0] = new Rectangle(screenAdjust(80, "W"), screenAdjust(20, "H"), screenAdjust(140, "W"), screenAdjust(135, "H"));
            items[0, 1] = new Rectangle(screenAdjust(80, "W"), screenAdjust(185, "H"), screenAdjust(140, "W"), screenAdjust(135, "H"));//185
            items[0, 2] = new Rectangle(screenAdjust(80, "W"), screenAdjust(360, "H"), screenAdjust(140, "W"), screenAdjust(135, "H"));//360
            items[1, 0] = new Rectangle(screenAdjust(1095, "W"), screenAdjust(15, "H"), screenAdjust(140, "W"), screenAdjust(135, "H"));
            items[1, 1] = new Rectangle(screenAdjust(1095, "W"), screenAdjust(175, "H"), screenAdjust(140, "W"), screenAdjust(135, "H"));//175
            items[1, 2] = new Rectangle(screenAdjust(1095, "W"), screenAdjust(350, "H"), screenAdjust(140, "W"), screenAdjust(135, "H"));//350
            items[2, 0] = new Rectangle(screenAdjust(80, "W"), screenAdjust(610, "H"), screenAdjust(140, "W"), screenAdjust(135, "H"));
            items[2, 1] = new Rectangle(screenAdjust(80, "W"), screenAdjust(770, "H"), screenAdjust(140, "W"), screenAdjust(135, "H"));//770
            items[2, 2] = new Rectangle(screenAdjust(80, "W"), screenAdjust(945, "H"), screenAdjust(140, "W"), screenAdjust(135, "H"));//945
            items[3, 0] = new Rectangle(screenAdjust(1095, "W"), screenAdjust(600, "H"), screenAdjust(140, "W"), screenAdjust(135, "H"));
            items[3, 1] = new Rectangle(screenAdjust(1095, "W"), screenAdjust(770, "H"), screenAdjust(140, "W"), screenAdjust(135, "H"));//770
            items[3, 2] = new Rectangle(screenAdjust(1095, "W"), screenAdjust(945, "H"), screenAdjust(140, "W"), screenAdjust(135, "H"));//945

            this.game = game;
            frames = 0;
            seconds = 20;
            timeleft = "" + seconds;
            game.round++;
        }
        public override void Update(GameTime gameTime) {
            playerPad[0] = GamePad.GetState(PlayerIndex.One);
            playerPad[1] = GamePad.GetState(PlayerIndex.Two);
            playerPad[2] = GamePad.GetState(PlayerIndex.Three);
            playerPad[3] = GamePad.GetState(PlayerIndex.Four);
            for (int x = 0; x < playerPad.Length; x++)
            {
                if (playerPad[x].DPad.Down == ButtonState.Pressed && playerPad0[x].DPad.Down != ButtonState.Pressed)
                {
                    if (p[x] != playerSelection.Three)
                    {
                        p[x]++;
                    }
                    else
                    {
                        p[x] = playerSelection.One;
                    }
                }

                if (playerPad[x].DPad.Up == ButtonState.Pressed && playerPad0[x].DPad.Up != ButtonState.Pressed)
                {
                    if (p[x] != playerSelection.One)
                    {
                        p[x]--;
                    }
                    else
                    {
                        p[x] = playerSelection.Three;
                    }
                }
                if(playerPad[x].Buttons.A == ButtonState.Pressed && playerPad[x].Buttons.A != ButtonState.Pressed)
                {

                }
            }
            //Player 1
            if (p[0] == playerSelection.One)
            {
                pSelect[0].Y = screenAdjust(20, "H");
            }
            if (p[0] == playerSelection.Two)
            {
                pSelect[0].Y = screenAdjust(185, "H");
            }
            if (p[0] == playerSelection.Three)
            {
                pSelect[0].Y = screenAdjust(360, "H");
            }
            //Player 2
            if (p[1] == playerSelection.One)
            {
                pSelect[1].Y = screenAdjust(15, "H");
            }
            if (p[1] == playerSelection.Two)
            {
                pSelect[1].Y = screenAdjust(175, "H");
            }
            if (p[1] == playerSelection.Three)
            {
                pSelect[1].Y = screenAdjust(350, "H");
            }
            //Player 3
            if (p[2] == playerSelection.One)
            {
                pSelect[2].Y = screenAdjust(610, "H");
            }
            if (p[2] == playerSelection.Two)
            {
                pSelect[2].Y = screenAdjust(770, "H");
            }
            if (p[2] == playerSelection.Three)
            {
                pSelect[2].Y = screenAdjust(945, "H");
            }
            //Player 4
            if (p[3] == playerSelection.One)
            {
                pSelect[3].Y = screenAdjust(600, "H");
            }
            if (p[3] == playerSelection.Two)
            {
                pSelect[3].Y = screenAdjust(770, "H");
            }
            if (p[3] == playerSelection.Three)
            {
                pSelect[3].Y = screenAdjust(945,"H");
            }

            playerPad0[0] = playerPad[0];
            playerPad0[1] = playerPad[1];
            playerPad0[2] = playerPad[2];
            playerPad0[3] = playerPad[3];
            frames++;
            timeleft = "" + ((60 * seconds - frames) / 60 + 1);
            if (frames >= 60 * (20))
                game.currentScreen = new Stage(game.round, this.game);
        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(new Color(180,140,100));
            game.spriteBatch.Draw(background, new Rectangle(0, 0, Globals.screenW, Globals.screenH), Color.White);
            for (int y = 0; y < inventories.Length; y++)
            {
                for (int z = 0; z < inventories[y].weapons.Length; z++)
                {
                    game.spriteBatch.Draw(inventories[y].weapons[z].texture, items[y, z], inventories[y].weapons[z].color);
                }
            }
            foreach (Rectangle x in pSelect)
            {
                game.spriteBatch.Draw(game.shopHighlight, x, Color.White);
            }

            game.spriteBatch.DrawString(game.font, timeleft, new Vector2(0, 0), Color.White);


            //80        20,185,360
            //1095      15,175,300
            //80        610,770,945
            //1095      600,770,945

        }
        public int screenAdjust(int value, string WorH)
        {
            int final = 0;
            if(WorH=="H")
            {
                final = value*(Globals.screenH / 1080);
            }
            if(WorH =="W")
            {
                final = value*(Globals.screenW / 1920);
            }
            return final;
        }
    }
}
