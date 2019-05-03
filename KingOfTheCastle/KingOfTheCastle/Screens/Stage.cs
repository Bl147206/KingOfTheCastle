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
        int platformThickness;
        Player winner;
        int background;
        SoundEffect winSound;
        SoundEffect music;
        SoundEffectInstance musicControl;

        //Rectangle rect = new Rectangle(0, 0, 20, 20);

        public Stage(int round, KingOfTheCastle game)
        {
            platforms = new Platform[Globals.rng.Next(round+1)+2];
            music = game.Content.Load<SoundEffect>("Heroic Intrusion");
            winSound = game.Content.Load<SoundEffect>("Applause");
            musicControl = music.CreateInstance();
            background =Globals.rng.Next(0,game.backgrounds.Length);

            platformThickness = 15;

            platforms[0] = new Platform(new Vector2(Globals.screenW/2, Globals.screenH - 100), Globals.screenW - 200, platformThickness);
            for (int x = 1; x < platforms.Length; x++) //Makes random platforms
            {
                int z = x % 4;
                platforms[x] = new Platform(new Vector2((float)Globals.rng.Next(screenAdjust(Globals.screenW,"W")), (float)(platforms[0].destination.Y - z * screenAdjust(120,"H") - screenAdjust(120,"H"))), Globals.rng.Next(100, 750), platformThickness);
            }
            frames = 0;
            musicControl.Volume = .3f;
            this.game = game;

            projectiles = new ProjectileHandler(game);
            
            seconds = 100;
            timeleft = ""+seconds;

           

            foreach (Player p in game.players)
            {
                if(p!=null)
                    p.spawn();
            }
            quest = new FirstKillQuest(this.game);

            killLevel = (int) (Globals.screenH * 1.4);
            musicControl.Play();
        }
        public override void Update(GameTime gameTime)
        {
            kb = Keyboard.GetState();

            int dead = 0;
            int alive = 0;
            if(!roundOver)
                foreach (Player p in game.players)
                {
                    if (p != null)
                    {

                        GamePadState gamePad = GamePad.GetState(p.playerIndex);

                        if (gamePad.Buttons.Start == ButtonState.Pressed && game.oldGamePadStates[p.playerNumber - 1].Buttons.Start != ButtonState.Pressed && !isPaused)
                        {
                            isPaused = true;
                            idxPause = p.playerNumber;
                            musicControl.Pause();
                        }
                        else if (gamePad.Buttons.Start == ButtonState.Pressed && game.oldGamePadStates[p.playerNumber - 1].Buttons.Start != ButtonState.Pressed && isPaused && p.playerNumber == idxPause)
                        {
                            isPaused = false;
                            musicControl.Play();
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
                        }
                    }
                }

            frames++;
            if (!roundOver)
            {
                timeleft = "" + ((60 * seconds - frames) / 60 + 1);
                if (frames >= 60 * seconds || (dead == game.getControllerCount() - 1 && game.getControllerCount() != 1))
                {
                    roundOver = true;
                    musicControl.Stop();
                    musicControl = winSound.CreateInstance();
                    musicControl.Play();
                    frames = 0;
                    foreach(Player p in game.players)
                    {
                        if (p != null)
                            if (p.IsAlive())
                                winner = p;

                    }
                }
            }
            if (roundOver)
            {
                if (game.round == 9) {

                }

                if (frames >= 180)
                {
                    game.currentScreen = new Shop(this.game);
                    musicControl.Stop();
                }
            }

            if (!roundOver)
            {
                projectiles.Update();
                quest.check();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.Navy);
            game.spriteBatch.Draw(game.backgrounds[background], new Rectangle(0, 0, Globals.screenW, Globals.screenH),Color.White);
            game.spriteBatch.Draw(game.test, new Rectangle(0, Globals.screenH - screenAdjust(60, "H"), Globals.screenW, screenAdjust(60, "H")),Color.Black);
            for (int x = 0; x < platforms.Length; x++)
            {
                if(platforms[x] != null)
                {
                    game.spriteBatch.Draw(game.test, platforms[x].destination, Color.Brown);
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
            game.spriteBatch.Draw(game.test, new Rectangle(20, 20, 3 * int.Parse(timeleft), 50), Color.Gray);
            projectiles.draw();

            if (isPaused) {
                game.spriteBatch.Draw(game.test, new Rectangle(0, 0, 10000, 10000), Color.Black * .5f);
                game.spriteBatch.Draw(game.test, new Rectangle(Globals.screenW / 2 - 300, Globals.screenH / 2 - 150, 600, 300), Color.Black);
                game.spriteBatch.DrawString(game.font, "Player " + idxPause, new Vector2(Globals.screenW / 2 - 100, Globals.screenH / 2 - 100), Color.White);
                game.spriteBatch.DrawString(game.font, "Press start to unpause", new Vector2(Globals.screenW / 2 - 260, Globals.screenH / 2 + 50), Color.White);
            }
            if(roundOver)
            {
                game.spriteBatch.Draw(game.test, new Rectangle(Globals.screenW / 2 - 300, Globals.screenH / 2 - 150, 600, 300), Color.Black);
                game.spriteBatch.Draw(game.test, new Rectangle(0, 0, 10000, 10000), Color.Black * .5f);

                if (game.round != 9) {
                    game.spriteBatch.DrawString(game.font, "Round Over!", new Vector2(Globals.screenW / 2 - 100, Globals.screenH / 2 - 100), winner.playerColor);
                } else {
                    game.spriteBatch.DrawString(game.font, "Game Over!", new Vector2(Globals.screenW / 2 - 100, Globals.screenH / 2 - 100), winner.playerColor);
                }
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
