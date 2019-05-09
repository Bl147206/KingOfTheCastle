using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KingOfTheCastle {
    class Inventory {
        public Weapon[] weapons;
        int index = 0;
        List<int> possibleWeapons = new List<int>();
        int typeDecider;
        public Inventory(int currentRound,KingOfTheCastle game) {
            this.weapons = new Weapon[3];
            for (int x = 0; x < 3; x++) //Limit on for loop = number of KINDS of weapons
            {
                possibleWeapons.Add(x);
            }
            // generate 3 weapons
            while (index < 3) {
                string part1 = Globals.weaponNames[Globals.rng.Next(Globals.weaponNames.Length)];
                string part2 = Globals.weaponNames[Globals.rng.Next(Globals.weaponNames.Length)];
                string name = part1 + " " + part2;
                typeDecider = possibleWeapons.ElementAt(Globals.rng.Next(0,possibleWeapons.Count-1));
                var kind = (Weapon.Kind)(typeDecider);
                possibleWeapons.Remove(typeDecider);


                int attack = Globals.rng.Next(currentRound * 2) + currentRound + 2;
                double attackSpeed = (Globals.rng.NextDouble() + .2) * .4 + currentRound * .5;
                int armorBonus = Globals.rng.Next(currentRound * 2, currentRound * 10);

                switch (kind) {
                    case Weapon.Kind.melee:
                        weapons[index]=(new Melee(name, attack, attackSpeed, 2*attack + (int)(1/(attackSpeed) * 2), game.swordTexture));

                        break;
                    case Weapon.Kind.ranged:
                        weapons[index]=(new Ranged(name, attack, attackSpeed, 2*attack+(int)((1/attackSpeed)*2), game.bowTexture));
                        break;
                    case Weapon.Kind.armor:
                        weapons[index] = (new Armor(name, armorBonus, armorBonus+(armorBonus/3), game.armorTexture));
                        break;

                }
                index++;
            }
        }
    }
}
