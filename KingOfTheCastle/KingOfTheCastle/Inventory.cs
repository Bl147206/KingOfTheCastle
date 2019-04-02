using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KingOfTheCastle {
    class Inventory {
        public List<Weapon> weapons;

        public Inventory() {
            this.weapons = new List<Weapon>();

            // generate 3 weapons
            while (this.weapons.Count != 3) {
                string name = Globals.weaponNames[Globals.rng.Next(Globals.weaponNames.Length)] + " " + Globals.weaponNames[Globals.rng.Next(Globals.weaponNames.Length)];

                if (this.weapons.All(weapon => name == weapon.name))
                    continue;

                int type = Globals.rng.Next(2);

                switch (type) {
                    case 0:
                        weapons.Add(new Melee());
                        break;
                    case 1:
                        weapons.Add(new Ranged());
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
