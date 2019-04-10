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
        Texture2D background;
        public Shop(KingOfTheCastle game)
        {
            background = game.shopText;
            for(int x = 0; x<pSelect.Length; x++)
            {
                p[x] = playerSelection.One;
            }
            pSelect[0] = new Rectangle(80, 20, 140, 135);
            pSelect[1] = new Rectangle(1095, 15, 140, 135);
            pSelect[2] = new Rectangle(80, 610, 140, 135);
            pSelect[3] = new Rectangle(1095, 600, 140, 135);
        }
        public override void Update(GameTime gameTime) {

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
