using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KingOfTheCastle {
    class Inventory {
        public List<Weapon> weapons;

        public Inventory(int currentRound) {
            this.weapons = new List<Weapon>();

            // generate 3 weapons
            while (this.weapons.Count != 3) {
                string part1 = Globals.weaponNames[Globals.rng.Next(Globals.weaponNames.Length)];
                string part2 = Globals.weaponNames[Globals.rng.Next(Globals.weaponNames.Length)];
                string name = part1 + " " + part2;

                if (this.weapons.All(weapon => name == weapon.name))
                    continue;

                var kind = (Weapon.Kind) Globals.rng.Next(2);

                int attack = Globals.rng.Next(currentRound * 3) + currentRound;
                double attackSpeed = Globals.rng.NextDouble() + .75 * currentRound;

                switch (kind) {
                    case Weapon.Kind.melee:
                        weapons.Add(new Melee(name, attack, attackSpeed, 10 * currentRound));
                        break;
                    case Weapon.Kind.ranged:
                        weapons.Add(new Ranged(name, attack, attackSpeed, 10 * currentRound));
                        break;
                }
            }
        }
    }
}
