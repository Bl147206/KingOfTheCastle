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
    class Stage: Screen
    {
        Platform[] platforms;
        KeyboardState kb;
        bool intersections;
        //Rectangle rect = new Rectangle(0, 0, 20, 20);
        public Stage(int round)
        {
            platforms = new Platform[Globals.rng.Next(round+1)+3];
            intersections = false;
            platforms[0] = new Platform(new Vector2(800,800), 1300,5);
            for(int x = 1; x<platforms.Length; x++)
            {
                platforms[x] = new Platform(new Vector2((float)Globals.rng.Next(1600), (float)(platforms[0].destination.Y-x*120)), Globals.rng.Next(100, 750), 5);
            }
            
        }
        public override void Update(GameTime gameTime)
        {
            kb = Keyboard.GetState();

        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.Blue);
            for (int x = 0; x < platforms.Length; x++)
            {
                game.spriteBatch.Draw(game.test, platforms[x].destination, Color.Red);
            }

        }
    }
}
