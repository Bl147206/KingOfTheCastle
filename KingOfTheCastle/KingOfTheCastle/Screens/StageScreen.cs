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
    class StageScreen : Screen
    {
        Platform[] platforms;
        //Rectangle rect = new Rectangle(0, 0, 20, 20);
        public StageScreen()
        {
            platforms = new Platform[3];
            platforms[0] = new Platform(new Vector2(20, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 500), 40, 5);

            platforms[1] = new Platform(new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 20,
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 500), 40, 5);

            platforms[2] = new Platform(new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2,
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 200), GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 200, 5);
        }
        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            game.GraphicsDevice.Clear(Color.Blue);
            spriteBatch.Draw(game.test, platforms[0].destination, Color.Red);
            spriteBatch.Draw(game.test, platforms[1].destination, Color.Red);
            spriteBatch.Draw(game.test, platforms[2].destination, Color.Red);
            //spriteBatch.Draw(game.test, rect, Color.Black);

        }
    }
}
