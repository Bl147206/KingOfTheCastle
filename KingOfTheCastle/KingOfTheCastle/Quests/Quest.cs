using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace KingOfTheCastle
{
    public abstract class Quest
    {
        public KingOfTheCastle game;
        public string title;
        public Vector2 titleLoc;
        public Rectangle display;
        public Vector2[] playerCompletionLocs=new Vector2[4];
        public string[] playerCompletionProgress=new string[4];
        public abstract void check();
        public abstract void Draw();
    }
}
