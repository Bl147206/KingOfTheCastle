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
        KeyboardState kb;
        //Rectangle rect = new Rectangle(0, 0, 20, 20);
        public StageScreen()
        {
            platforms = new Platform[Globals.rng.Next(4)+3];

            platforms[0] = new Platform(new Vector2(800,800), 800, 5);
            for(int x = 1; x<platforms.Length; x++)
            {
                platforms[x] = new Platform(new Vector2((float)Globals.rng.Next(1600), (float)Globals.rng.Next(900)), Globals.rng.Next(100, 750), 5);
            }
            
        }
        public override void Update(GameTime gameTime)
        {
            kb = Keyboard.GetState();

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
