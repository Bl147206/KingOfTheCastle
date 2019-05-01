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
        public Rectangle oldDisplay;
        public Texture2D crown;
        public bool isCompleted=false;
        public int timer=0;
        public double yVel=.1;
        public double yLoc;
        public Vector2[] playerCompletionLocs=new Vector2[4];
        public double[] yLocPlayers = new double[4];
        public string[] playerCompletionProgress=new string[4];
        public SoundEffect questComplete;
        public Player winner;
        public abstract void check();
        public abstract void Draw();
    }
}
