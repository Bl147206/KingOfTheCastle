﻿using System;
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
            game.GraphicsDevice.Clear(Color.Beige);
            game.spriteBatch.Draw(game.test, new Rectangle(GraphicsAdapter.DefaultAdapter.Width / 2, 0, 1, GraphicsAdapter.DefaultAdapter.Height);
        }
    }
}
