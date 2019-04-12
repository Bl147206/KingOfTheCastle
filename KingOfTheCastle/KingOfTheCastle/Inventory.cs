using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KingOfTheCastle {
    class Inventory {
        public Weapon[] weapons;
        int index = 0;
        public Inventory(int currentRound,KingOfTheCastle game) {
            this.weapons = new Weapon[3];

            // generate 3 weapons
            while (index<3) {
                string part1 = Globals.weaponNames[Globals.rng.Next(Globals.weaponNames.Length)];
                string part2 = Globals.weaponNames[Globals.rng.Next(Globals.weaponNames.Length)];
                string name = part1 + " " + part2;

                //if (this.weapons.All(weapon => name == weapon.name))
                //    continue;

                var kind = (Weapon.Kind) Globals.rng.Next(2);

                int attack = Globals.rng.Next(currentRound * 3) + currentRound;
                double attackSpeed = Globals.rng.NextDouble() + .75 * currentRound;

                switch (kind) {
                    case Weapon.Kind.melee:
                        weapons[index]=(new Melee(name, attack, attackSpeed, 10 * currentRound, game.swordTexture));

                        break;
                    case Weapon.Kind.ranged:
                        weapons[index]=(new Ranged(name, attack, attackSpeed, 10 * currentRound, game.bowTexture));
                        break;

                }
                index++;
            }
        }
    }
}
