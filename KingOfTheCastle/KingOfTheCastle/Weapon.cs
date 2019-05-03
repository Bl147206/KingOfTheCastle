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
    abstract class Weapon {
        public enum Kind: int {
            melee, ranged
        }

        public int attack;
        public double attackSpeed;
        public string name;
        public int cost;
        public Kind kind;
        public Texture2D texture;
        public Color color;
    }

    class Melee: Weapon {
        public new readonly Kind kind = Kind.melee;

        public Melee(string name, int attack, double attackSpeed, int cost, Texture2D sword) {
            this.name = name;
            this.attack = attack;
            this.attackSpeed = attackSpeed;
            this.cost = cost;
            texture = sword;
            color = new Color(Globals.rng.Next(50,200), Globals.rng.Next(50,200), Globals.rng.Next(50,200));
        }
    }

    class Ranged: Weapon {
        public new readonly Kind kind = Kind.ranged;

        public Ranged(string name, int attack, double attackSpeed, int cost, Texture2D bow)
        {
            this.name = name;
            this.attack = attack;
            this.attackSpeed = attackSpeed;
            this.cost = cost;
            texture = bow;
            color = new Color(Globals.rng.Next(50,200), Globals.rng.Next(50,200), Globals.rng.Next(50,200));
        }
    }

    class Armor : Weapon
    {
        public new readonly Kind kind = Kind.ranged;

        public Ranged(string name, int attack, double attackSpeed, int cost, Texture2D bow)
        {
            this.name = name;
            this.attack = attack;
            this.attackSpeed = attackSpeed;
            this.cost = cost;
            texture = bow;
            color = new Color(Globals.rng.Next(50, 200), Globals.rng.Next(50, 200), Globals.rng.Next(50, 200));
        }
    }
}
