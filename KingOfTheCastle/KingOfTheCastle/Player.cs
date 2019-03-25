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
    class Player
    {
        Rectangle location;
        Texture2D texture;
        int playerIndex;
        //more specific x and y coords
        double x;
        double y;

        public Player(Rectangle spawnLocation, Texture2D texture,int playerIndex)
        {
            location = spawnLocation;
            this.texture = texture;
            this.playerIndex = playerIndex;
            x = location.X;
            y = location.Y;
        }

        public void UpdatePosition(float x, float y)
        {
            this.y -= y;
            this.x += x;
            location.X = (int)this.x;
            location.Y = (int)this.y;
        }

    }
}
