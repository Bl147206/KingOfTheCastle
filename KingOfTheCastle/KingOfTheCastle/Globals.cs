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

namespace KingOfTheCastle {
    class Globals {
        // super dank seed amirite
        public static Random rng = new Random();
        public static int screenW = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        public static int screenH = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        public static string[] weaponNames = { "Night", "Sky", "Edge", "Blood", "Ripper", "Royal", "Guardian", "Rapid", "River", "Stream" };
    }
}
