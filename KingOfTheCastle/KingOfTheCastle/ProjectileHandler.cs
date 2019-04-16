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
    class ProjectileHandler
    {
        List<Projectile> projectiles;
        KingOfTheCastle game;

        public ProjectileHandler(KingOfTheCastle game)
        {
            this.game = game;
            projectiles = new List<Projectile>();
        }

        public void Update()
        {
            for(int projectile = 0; projectile < projectiles.Count; projectile++)
            {
                projectiles[projectile].Update(game.players);
                if(projectiles[projectile].hitBox.X > Globals.screenW ||
                    projectiles[projectile].hitBox.X + projectiles[projectile].hitBox.Width < 0 ||
                    projectiles[projectile].hitBox.Y + projectiles[projectile].hitBox.Height < 0 ||
                    projectiles[projectile].hitBox.Y > Globals.screenH ||
                    projectiles[projectile].dispose)
                { //removing a projectile that's off the screen
                    projectiles.RemoveAt(projectile--);
                }
            }
        }

        public void draw()
        {
            foreach(Projectile p in projectiles)
            {
                p.draw(game.spriteBatch);
            }
        }

        public void add(Projectile p)
        {
            projectiles.Add(p);
        }

        public class Projectile
        {
            public Rectangle hitBox;
            List<int> playersHits;
            int playerWhoFired;
            int xVelocity;
            int damageValue;
            Texture2D texture;
            public bool dispose;
            Color color;

            public Projectile(Texture2D texture, Rectangle hitBox, int playerWhoFired, int xVelocity, int damageValue, Color color)
            {
                this.hitBox = hitBox;
                playersHits = new List<int>();
                this.playerWhoFired = playerWhoFired;
                this.xVelocity = xVelocity;
                this.damageValue = damageValue;
                this.texture = texture;
                this.dispose = false;
                this.color = color;
            }

            public void Update(Player[] players)
            {
                hitBox.X += xVelocity;
                foreach(Player p in players)
                {
                    if(p != null && p.playerNumber != playerWhoFired && !playersHits.Contains(p.playerNumber))
                    {
                        p.damage(damageValue);
                        playersHits.Add(p.playerNumber);
                    }
                }
            }

            public void draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Draw(texture, hitBox, color);
            }
        }
    }
}
