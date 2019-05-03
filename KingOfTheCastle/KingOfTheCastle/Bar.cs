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
    class Bar
    {

        public int max;
        public int current;
        Rectangle maxBar, displayRec;
        Color color;
        KingOfTheCastle game;

        public Bar(int max, int current, Rectangle maxBar, Color color, KingOfTheCastle game)
        {
            this.max = max;
            this.current = current;
            this.maxBar = maxBar;
            this.color = color;
            this.game = game;

            displayRec = maxBar;
        }

        public void update()
        {
            displayRec.Width = (int) (maxBar.Width * (double)current / max);
        }

        public void update(int current)
        {
            this.current = current;
            displayRec.Width = (int)(maxBar.Width * (double)current / max);
        }

        public void draw()
        {
            game.spriteBatch.Draw(game.test, displayRec, color);
        }

    }
}
