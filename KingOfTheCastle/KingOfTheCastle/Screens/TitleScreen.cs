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
    class TitleScreen: Screen
    {
        enum incColor
        {
            red,
            dRed,
            green,
            dGreen,
            blue,
            dBlue
        };

        Texture2D logo;
        Rectangle logoPos;
        Vector2 textpos;
        SoundEffect music;
        SoundEffectInstance musicControl;
        int time;
        double seconds;
        Color bg;
        incColor currentColor;
        KeyboardState kb;
        GamePadState previous;
        public TitleScreen(KingOfTheCastle game, GamePadState previous)
        {
            logo = game.Content.Load<Texture2D>("logo");
            music = game.Content.Load<SoundEffect>("Forward-Assault");
            logoPos = new Rectangle(Globals.screenW/2-400, Globals.screenH/2-350, 800, 700);
            textpos = new Vector2(Globals.screenW/2-300, (float)(Globals.screenH-Globals.screenH/7));
            musicControl= music.CreateInstance();
            bg = new Color(255,0,0);
            time = 0;
            seconds = 0;
            musicControl.Volume = .3f;
            musicControl.Play();
            this.previous = previous;
            currentColor = incColor.green;
        }
        public TitleScreen(KingOfTheCastle game)
        {
            logo = game.Content.Load<Texture2D>("logo");
            music = game.Content.Load<SoundEffect>("Forward-Assault");
            logoPos = new Rectangle(Globals.screenW / 2 - 400, Globals.screenH / 2 - 350, 800, 700);
            textpos = new Vector2(Globals.screenW / 2 - 300, (float)(Globals.screenH - Globals.screenH / 7));
            musicControl = music.CreateInstance();
            bg = new Color(255, 0, 0);
            time = 0;
            seconds = 0;
            musicControl.Volume = .3f;
            musicControl.Play();

            currentColor = incColor.green;
        }
        public override void Update(GameTime gameTime)
        { 
            GamePadState pad1 = GamePad.GetState(0);
            kb = Keyboard.GetState();
            if (pad1.IsButtonDown(Buttons.A)&&!previous.IsButtonDown(Buttons.A))//Will added this so he does not have to get a controller to test
            {
                game.currentScreen = new Stage(game.round,this.game);
                game.currentScreen.game = game;
                musicControl.Stop();
            }
            if (pad1.IsButtonDown(Buttons.Y))
            {
                game.currentScreen = new Help(this.game);
                musicControl.Stop();
            }
            switch (currentColor)
            {
                case incColor.green:
                    bg.G+=3;
               
                    if (bg.G == 255)
                        currentColor=incColor.dRed;
                    break;
                case incColor.dRed:
                    bg.R-=3;
                 
                    if (bg.R == 0)
                        currentColor = incColor.blue;
                    break;
                case incColor.blue:
                    bg.B+=3;
                    
                    if (bg.B == 255)
                        currentColor = incColor.dGreen;
                    break;
                case incColor.dGreen:
                    bg.G-=3;
                   
                    if (bg.G == 0)
                        currentColor = incColor.red;
                    break;
                case incColor.red:
                    bg.R+=3;
                    
                    if (bg.R == 255)
                        currentColor = incColor.dBlue;
                    break;
                case incColor.dBlue:
                    bg.B-=3;
                    
                    if (bg.B == 0)
                        currentColor = incColor.green;
                    break;
            }
            if(seconds==80)
            {
                time = 0;
                musicControl.Play();
            }
            time++;
            seconds = time / 60.0;
            previous = pad1;

        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(bg);
            game.spriteBatch.Draw(logo, logoPos, Color.White);
            game.spriteBatch.DrawString(game.font, "  Press Start or A to Play\n     Press Y for Help\nPress Back or Escape to exit", textpos, Color.Black);
            game.spriteBatch.DrawString(game.font, "Players Connected (" + game.getControllerCount() + " / 4)", new Vector2(screenAdjust(1300,"W"), screenAdjust(20,"H")), Color.Black);
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
