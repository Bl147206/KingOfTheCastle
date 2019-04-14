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
using System.Timers;

namespace KingOfTheCastle
{
    class Stage: Screen
    {
        Platform[] platforms;
        public ProjectileHandler projectiles;
        KeyboardState kb;
        int frames;

        //Rectangle rect = new Rectangle(0, 0, 20, 20);

        public Stage(int round, KingOfTheCastle game)
        {
            platforms = new Platform[Globals.rng.Next(round+1)+2];
            platforms[0] = new Platform(new Vector2(Globals.screenW/2, Globals.screenH - 100), Globals.screenW - 200, 5);
            for (int x = 1; x < platforms.Length; x++) //Makes random platforms
            {
                int z = x % 4;
                platforms[x] = new Platform(new Vector2((float)Globals.rng.Next(Globals.screenW), (float)(platforms[0].destination.Y - z * 120 - 120)), Globals.rng.Next(100, 750), 5);
            }
            frames = 0;
            this.game = game;

            projectiles = new ProjectileHandler(game);

            foreach(Player p in game.players)
            {
                if(p!=null)
                    if (!p.IsAlive())
                        p.revive();
            }
        }
        public override void Update(GameTime gameTime)
        {
            kb = Keyboard.GetState();
            int dead = 0;
            foreach (Player p in game.players)
            {
                if (p != null)
                {
                    if (p.IsAlive())
                    {
                        p.Update(platforms);
                    }
                    else
                    {
                        dead++;
                        GamePadState gamePad = GamePad.GetState(p.playerIndex);
                        if (gamePad.DPad.Down == ButtonState.Pressed)
                        {// temp stuff to let a person revive themself
                            p.revive();
                            dead--;
                        }
                    }
                }
            }
            frames++;
            if (frames >= 60 * (60)/*seconds*/ || dead >= 3)
            {
                foreach (Player p in game.players)
                    if(p!=null)
                        p.kill();
                game.currentScreen = new Shop(this.game);
            }
            projectiles.Update();
        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.Navy);
            for (int x = 0; x < platforms.Length; x++)
            {
                if(platforms[x] != null)
                {
                    game.spriteBatch.Draw(game.test, platforms[x].destination, Color.Red);
                }
            }
            foreach (Player p in game.players)
            {
                if (p != null && p.IsAlive())
                {
                    p.draw();
                }
            }
            projectiles.draw();
        }
    }
}
