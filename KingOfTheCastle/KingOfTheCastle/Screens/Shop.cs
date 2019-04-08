using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KingOfTheCastle
{
    class Shop: Screen
    {
        Texture2D background;
        public Shop(KingOfTheCastle game)
        {
            background = game.shopText;
        }
        public override void Update(GameTime gameTime) {

        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(new Color(180,140,100));
            game.spriteBatch.Draw(background, new Rectangle(0, 0, Globals.screenW, Globals.screenH), Color.White);

        }
    }
}
