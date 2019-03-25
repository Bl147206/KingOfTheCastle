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

        public Player(Rectangle spawnLocation, Texture2D texture,int playerIndex)
        {
            location = spawnLocation;
            this.texture = texture;
            this.playerIndex = playerIndex;
        }


    }
}
