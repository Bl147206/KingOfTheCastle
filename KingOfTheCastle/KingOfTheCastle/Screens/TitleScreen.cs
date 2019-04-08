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
        Color bg;
        incColor currentColor;
        KeyboardState kb;
        public TitleScreen(KingOfTheCastle game)
        {
            logo = game.Content.Load<Texture2D>("logo");
            logoPos = new Rectangle(Globals.screenW/2-400, Globals.screenH/2-350, 800, 700);
            textpos = new Vector2(Globals.screenW/2-335, (float)(Globals.screenH-Globals.screenH/9));
            bg = new Color(255,0,0);
            currentColor = incColor.green;
        }

        public override void Update(GameTime gameTime)
        {
            GamePadState pad1 = GamePad.GetState(0);
            kb = Keyboard.GetState();
            if (pad1.IsButtonDown(Buttons.Start) || pad1.IsButtonDown(Buttons.A)||kb.IsKeyDown(Keys.Space))//Will added this so he does not have to get a controller to test
            {
                game.currentScreen = new Stage(game.round,this.game);
                //game.currentScreen = new Stage(game.round);
                game.currentScreen.game = game;
            }
            switch(currentColor)
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
            

        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(bg);
            game.spriteBatch.Draw(logo, logoPos, Color.White);
            game.spriteBatch.DrawString(game.font, "     Press Start or A...\nPress Back or Escape to exit", textpos, Color.White);


        }
    }
}
