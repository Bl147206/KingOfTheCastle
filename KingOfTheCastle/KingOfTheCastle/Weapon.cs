using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }

    class Melee: Weapon {
        public new readonly Kind kind = Kind.melee;

        public Melee(string name, int attack, double attackSpeed, int cost) {
            this.name = name;
            this.attack = attack;
            this.attackSpeed = attackSpeed;
            this.cost = cost;
        }
    }

    class Ranged: Weapon {
        public new readonly Kind kind = Kind.ranged;

        public Ranged(string name, int attack, double attackSpeed, int cost) {
            this.name = name;
            this.attack = attack;
            this.attackSpeed = attackSpeed;
            this.cost = cost;
        }
    }
}
