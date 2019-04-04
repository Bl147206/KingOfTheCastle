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
        public override void Update(GameTime gameTime) {

        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(new Color(180,140,100));
            game.spriteBatch.Draw(game.test, new Rectangle(Globals.screenW / 2, 0, 1, Globals.screenH),Color.Brown);
            game.spriteBatch.Draw(game.test, new Rectangle(0, Globals.screenH / 2, Globals.screenW, 1), Color.Brown);
        }
    }
}
