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
    class Platform
    {

        Rectangle destination;

        public Platform(Vector2 Center, int Width, int Height)
        {
            destination = new Rectangle((int)Center.X - (Width / 2), (int)Center.Y - (Height / 2), Width, Height);
        }

    }
}
