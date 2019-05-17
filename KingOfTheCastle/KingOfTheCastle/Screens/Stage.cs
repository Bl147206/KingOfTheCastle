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
        public Platform[] platforms;
        public ProjectileHandler projectiles;
        public DamageValueHandler damageValues;
        KeyboardState kb;
        int frames;
        int seconds;
        int killLevel;
        String timeleft;
        Quest quest;
        bool roundOver;
        int platformThickness;
        Player winner;
        int background;
        SoundEffect winSound;
        SoundEffect music;
        SoundEffectInstance musicControl;
        Bar timer;

        //Rectangle rect = new Rectangle(0, 0, 20, 20);

        public Stage(int round, KingOfTheCastle game)
        {
            platforms = new Platform[4];
            music = game.Content.Load<SoundEffect>("Heroic Intrusion");
            winSound = game.Content.Load<SoundEffect>("Applause");
            musicControl = music.CreateInstance();
            background = Globals.rng.Next(0, game.backgrounds.Length);

            platformThickness = 25;

            platforms[0] = new Platform(new Vector2(Globals.screenW / 2, Globals.screenH - 100), Globals.screenW - 500, platformThickness);
            for (int x = 1; x < platforms.Length; x++) //Makes random platforms
            {
                int z = x % 4;//In case we want more than 5 platforms
                platforms[x] = new Platform(new Vector2(Globals.rng.Next(platforms[0].destination.X+350, platforms[0].destination.X+ platforms[0].destination.Width-350), (float)(platforms[0].destination.Y - z * screenAdjust(120, "H") - screenAdjust(120, "H"))), Globals.rng.Next(100, 750), platformThickness);
            }
            frames = 0;
            musicControl.Volume = .3f;
            this.game = game;

            projectiles = new ProjectileHandler(game);
            damageValues = new DamageValueHandler(game);
            
            seconds = 100;
            timeleft = "" + seconds;

            foreach (Player p in game.players)
            {
                if (p != null)
                    p.spawn();
            }
            switch (Globals.rng.Next(1, 4))
            {
                case 1:
                    quest = new FirstKillQuest(this.game);
                    break;
                case 2:
                    quest = new JumpQuest(this.game);
                    break;
                case 3:
                    quest = new ProjectileQuest(this.game);
                    break;
            }
                
        

            timer = new Bar(seconds,seconds, new Rectangle(20, 20, 300, 50), Color.Gray,game);

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
                                if(p.attacker!=0)
                                {
                                    game.players[p.attacker - 1].kills += 1;
                                    game.players[p.attacker - 1].roundKills++;
                                    game.players[p.attacker - 1].gold += p.goldOnKill;
                                    game.strongHit.Play();
                                }
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
                    int aliveAmount = 0;
                    Player win = null;
                    foreach(Player p in game.players)
                    {
                        if (p != null) {
                            if (p.IsAlive()) {
                                aliveAmount++;
                                win = p;
                            }
                        }
                    }
                    if (aliveAmount == 1)
                        winner = win;

                    else if(aliveAmount>1)
                    {
                        int highHealth = 0;
                        Player healthy = null;
                        foreach(Player p in game.players)
                        {
                            if(p!=null)
                            {
                                if(p.IsAlive())
                                {
                                    if (p.health > highHealth)
                                    {
                                        highHealth = p.health;
                                        healthy = p;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            timer.update(int.Parse(timeleft));
            if (roundOver)
            {
                if (game.round == 9) {
                    // calculate actual winner
                    List<Player> mostWon = new List<Player>();
                    int won = 0;
                    foreach (Player p in game.players) {
                        if (p == null)
                            continue;

                        if (mostWon.Count == 0) {
                            mostWon.Add(p);
                            won = p.numRoundsWon;
                            continue;
                        }

                        if (p.numRoundsWon > won) {
                            mostWon.Clear();
                            mostWon.Add(p);
                            continue;
                        }

                        if (p.numRoundsWon == won) {
                            mostWon.Add(p);
                        }
                    }
                    
                    // If we have one person here they win
                    if (mostWon.Count == 1) {
                        winner = mostWon[0];
                    }

                    List<Player> mostKills = new List<Player>();
                    int kills = 0;
                    foreach (Player p in mostWon) {
                        if (mostKills.Count == 0) {
                            mostKills.Add(p);
                            kills = p.kills;
                            continue;
                        }

                        if (p.kills > kills) {
                            mostKills.Clear();
                            mostKills.Add(p);
                            continue;
                        }

                        if (p.kills == kills) {
                            mostKills.Add(p);
                        }
                    }

                    if (mostKills.Count == 1) {
                        winner = mostKills[0];
                    }

                    List<Player> mostGold = new List<Player>();
                    int gold = 0;
                    foreach (Player p in mostKills) {
                        if (mostGold.Count == 0) {
                            mostGold.Add(p);
                            gold = p.gold;
                            continue;
                        }

                        if (p.gold > gold) {
                            mostGold.Clear();
                            mostGold.Add(p);
                            continue;
                        }

                        if (p.gold == gold) {
                            mostGold.Add(p);
                        }
                    }

                    if (mostGold.Count == 1) {
                        winner = mostGold[0];
                    } else {
                        winner = null;
                    }

                    if (frames >= 200) {
                        game.currentScreen = new TitleScreen(game);
                        musicControl.Stop();
                    }
                }

                if (frames >= 180 && game.round != 9)
                {
                    game.currentScreen = new Shop(game);
                    musicControl.Stop();
                }
            }

            if (!roundOver)
            {
                projectiles.Update();
                damageValues.update();
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
            quest.Draw();
            foreach (Player p in game.players)
            {
                if (p != null && p.IsAlive())
                {
                    p.draw();
                }
            }
            
            timer.draw();
            //game.spriteBatch.Draw(game.test, new Rectangle(20, 20, 3 * int.Parse(timeleft), 50), Color.Gray); //Timer bar
            projectiles.draw();
            damageValues.draw();

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
                    if(winner==null)
                        game.spriteBatch.DrawString(game.font, "Time Over!", new Vector2(Globals.screenW / 2 - 100, Globals.screenH / 2 - 100), Color.White);
                    else
                        game.spriteBatch.DrawString(game.font, "Round Over!", new Vector2(Globals.screenW / 2 - 100, Globals.screenH / 2 - 100), winner.playerColor);
                } else {
                    if (winner == null) {
                        game.spriteBatch.DrawString(game.font, "Game Over!", new Vector2(Globals.screenW / 2 - 100, Globals.screenH / 2 - 100), Color.White);
                        game.spriteBatch.DrawString(game.font, "Nobody Wins!", new Vector2(Globals.screenW / 2 - 100, Globals.screenH / 2 - 50), Color.White);
                    } else {
                        game.spriteBatch.DrawString(game.font, "Game Over!", new Vector2(Globals.screenW / 2 - 100, Globals.screenH / 2 - 100), winner.playerColor);
                        game.spriteBatch.DrawString(game.font, "Player " + winner.playerNumber + " Wins!", new Vector2(Globals.screenW / 2 - 150, Globals.screenH / 2 + 25), winner.playerColor);
                    }
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
