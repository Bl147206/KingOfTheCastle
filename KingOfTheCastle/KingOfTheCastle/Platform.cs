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
    public class Platform
    {

        public Rectangle destination;
        public Rectangle textureD;
        public Texture2D texture;

        public Platform(Vector2 Center, int Width, int Height, Texture2D platformTexture, bool isBase)
        {
            destination = new Rectangle((int)Center.X - (Width / 2), (int)Center.Y - (Height / 2), Width, Height);
            textureD = new Rectangle(destination.X - Height / 2, destination.Y - Height / 2, Width + Height / 2, 2 * Height);
            Texture2D texture = platformTexture;
        }

    }
}
