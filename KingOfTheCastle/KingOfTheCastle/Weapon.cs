using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KingOfTheCastle {
    abstract class Weapon {
        public int attack;
        public int attackSpeed;
        public string name;
        public int cost;
    }

    class Melee: Weapon {

    }

    class Ranged: Weapon {

    }
}
