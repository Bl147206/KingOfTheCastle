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
        Inventory[] inventories = new Inventory[4];
        public Shop(KingOfTheCastle game)
        {
            background = game.shopText;
            this.game = game;
            for(int x = 0; x<pSelect.Length; x++)
            {
                p[x] = playerSelection.One;
            }
            pSelect[0] = new Rectangle(80, 20, 140, 135);//20,185,360
            pSelect[1] = new Rectangle(1095, 15, 140, 135);//15,175,350
            pSelect[2] = new Rectangle(80, 610, 140, 135);//610,770,945
            pSelect[3] = new Rectangle(1095, 600, 140, 135);//600,770,945
            for(int x = 0; x<inventories.Length; x++)
            {
                inventories[x] = new Inventory(game.round, this.game);
            }
            items[0, 0] = new Rectangle(80, 20, 140, 135);
            items[0, 1] = new Rectangle(80, 185, 140, 135);
            items[0, 2] = new Rectangle(80, 360, 140, 135);
            items[1, 0] = new Rectangle(1095, 15, 140, 135);
            items[1, 1] = new Rectangle(1095, 175, 140, 135);
            items[1, 2] = new Rectangle(1095, 350, 140, 135);
            items[2, 0] = new Rectangle(80, 610, 140, 135);
            items[2, 1] = new Rectangle(80, 770, 140, 135);
            items[2, 2] = new Rectangle(80, 945, 140, 135);
            items[3, 0] = new Rectangle(1095, 600, 140, 135);
            items[3, 1] = new Rectangle(1095, 770, 140, 135);
            items[3, 2] = new Rectangle(1095, 945, 140, 135);

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

            //80        20,185,360
            //1095      15,175,300
            //80        610,770,945
            //1095      600,770,945

        }
    }
}
