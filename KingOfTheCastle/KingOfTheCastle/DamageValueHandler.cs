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
    class DamageValueHandler
    {

        List<DamageValue> DamageValues;
        KingOfTheCastle game;

        public DamageValueHandler(KingOfTheCastle game)
        {
            this.game = game;
            DamageValues = new List<DamageValue>();
        }

        public void draw()
        {
            foreach (DamageValue d in DamageValues)
            {
                d.draw(game.spriteBatch, game.smallFont);
            }
        }

        public void update()
        {
            for(int i = 0; i < DamageValues.Count; i++)
            {
                DamageValues[i].update();
                if(DamageValues[i].displayTime == 0)
                {
                    DamageValues.RemoveAt(i--);
                }
            }
        }

        public void addDamageValue(DamageValue damageValue)
        {
            DamageValues.Add(damageValue);
        }

        public class DamageValue
        {

            int shieldDamage, healthDamage;
            bool killed;
            Player damaged;
            SpriteFont font;
            public int xPos, yPos, displayTime = 30;

            public DamageValue(int shieldDamage, int healthDamage, bool killed, Player damaged, SpriteFont font)
            {
                this.shieldDamage = shieldDamage;
                this.healthDamage = healthDamage;
                this.damaged = damaged;
                this.font = font;
                this.killed = killed;
            }

            public void update()
            {
                displayTime--;
                xPos = damaged.location.X;
                yPos = damaged.location.Y - font.LineSpacing;
            }

            public void draw(SpriteBatch spriteBatch, SpriteFont font)
            {
                if(killed)
                {
                    spriteBatch.DrawString(font, (shieldDamage + healthDamage).ToString(), new Vector2(xPos, yPos), Color.DarkRed);
                }
                else
                {
                    if (shieldDamage > 0)
                    {
                        spriteBatch.DrawString(font, shieldDamage.ToString(), new Vector2(xPos, yPos), Color.Black);
                        xPos += font.LineSpacing * (shieldDamage.ToString().Length + 2);
                    }
                    if (healthDamage > 0)
                    {
                        spriteBatch.DrawString(font, healthDamage.ToString(), new Vector2(xPos, yPos), Color.Red);
                    }
                }
            }
        }
    }
}
