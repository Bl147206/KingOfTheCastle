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
        int seconds;
        int killLevel;
        String timeleft;
        Quest quest;
        bool roundOver;
        int overTimer;

        //Rectangle rect = new Rectangle(0, 0, 20, 20);

        public Stage(int round, KingOfTheCastle game)
        {
            platforms = new Platform[Globals.rng.Next(round+1)+2];
            platforms[0] = new Platform(new Vector2(Globals.screenW/2, Globals.screenH - 100), Globals.screenW - 200, 5);
            for (int x = 1; x < platforms.Length; x++) //Makes random platforms
            {
                int z = x % 4;
                platforms[x] = new Platform(new Vector2((float)Globals.rng.Next(screenAdjust(Globals.screenW,"W")), (float)(platforms[0].destination.Y - z * screenAdjust(120,"H") - screenAdjust(120,"H"))), Globals.rng.Next(100, 750), 5);
            }
            frames = 0;
            this.game = game;

            projectiles = new ProjectileHandler(game);
            
            seconds = 100;
            timeleft = ""+seconds;

            quest = new JumpQuest(this.game);

            foreach (Player p in game.players)
            {
                if(p!=null)
                    if (!p.IsAlive())
                        p.spawn();
            }

            killLevel = (int) (Globals.screenH * 1.4);
        }
        public override void Update(GameTime gameTime)
        {
            kb = Keyboard.GetState();

            int dead = 0;
            int alive = 0;
            foreach (Player p in game.players)
            {
                if (p != null)
                {

                    GamePadState gamePad = GamePad.GetState(p.playerIndex);

                    if (gamePad.Buttons.Start == ButtonState.Pressed && game.oldGamePadStates[p.playerNumber - 1].Buttons.Start != ButtonState.Pressed && !isPaused) {
                        isPaused = true;
                        idxPause = p.playerNumber;
                    } else if (gamePad.Buttons.Start == ButtonState.Pressed && game.oldGamePadStates[p.playerNumber - 1].Buttons.Start != ButtonState.Pressed && isPaused && p.playerNumber == idxPause) {
                        isPaused = false;
                    }

                    if (isPaused)
                        continue;

                    if (p.IsAlive())
                    {
                        alive++;
                        p.Update(platforms);
                        if (p.location.Y > killLevel)
                        {
                            p.kill();
                        }
                    }
                    else
                    {
                        dead++;
                        if (gamePad.DPad.Down == ButtonState.Pressed)
                        {// temp stuff to let a person revive themself
                            p.revive();
                            dead--;
                        }
                        if (gamePad.DPad.Left==ButtonState.Pressed)
                        {
                            p.spawn();
                            dead--;
                        }
                    }
                }
            }

            frames++;
            if (!roundOver)
            {             
                timeleft = "" + ((60 * seconds - frames) / 60 + 1);
                if (frames >= 60 * seconds || (dead == game.getControllerCount() - 1 && game.getControllerCount() != 1))
                {

                    foreach (Player p in game.players)
                        if (p != null)
                            p.kill();
                    game.currentScreen = new Shop(this.game);
                }
            }
            
            projectiles.Update();
            quest.check();
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
            quest.Draw();
            game.spriteBatch.DrawString(game.font, timeleft, new Vector2(0, 0), Color.White);
            projectiles.draw();

            if (isPaused) {
                game.spriteBatch.Draw(game.test, new Rectangle(0, 0, 10000, 10000), Color.Black * .5f);
                game.spriteBatch.Draw(game.test, new Rectangle(Globals.screenW / 2 - 300, Globals.screenH / 2 - 150, 600, 300), Color.Black);
                game.spriteBatch.DrawString(game.font, "Player " + idxPause, new Vector2(Globals.screenW / 2 - 100, Globals.screenH / 2 - 100), Color.White);
                game.spriteBatch.DrawString(game.font, "Press start to unpause", new Vector2(Globals.screenW / 2 - 260, Globals.screenH / 2 + 50), Color.White);
            }
        }

        public int screenAdjust(int value, string WorH)
        {
            int final = 0;
            if (WorH == "H")
            {
                final = value * (Globals.screenH / 1080);
            }
            if (WorH == "W")
            {
                final = value * (Globals.screenW / 1920);
            }
            return final;
        }
    }
}
