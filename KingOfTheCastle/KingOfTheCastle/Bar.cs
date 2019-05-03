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

        int max;
        public int current;
        Rectangle maxBar, displayRec;
        Color color;

        public Bar(int max, int current, Rectangle maxBar, Color color)
        {
            this.max = max;
            this.current = current;
            this.maxBar = maxBar;
            this.color = color;

            displayRec = maxBar;
        }

        public void update()
        {
            displayRec.Width = (int) (maxBar.Width * (double)current / max);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            
        }

    }
}
