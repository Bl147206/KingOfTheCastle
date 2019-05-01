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

namespace KingOfTheCastle
{
    class Help : Screen
    {
        Texture2D[] helpSlides;
        enum Slides
        {
            One,
            Two,
            Three
        }
        Slides currentSlide;
        Rectangle display;
        GamePadState gp;
        GamePadState gpO;
    

        public Help(KingOfTheCastle game)
        {

            helpSlides = new Texture2D[3];
            for(int x = 0; x<helpSlides.Length;x++)
            {
                helpSlides[x] = game.Content.Load<Texture2D>("help" + x);
            }
            currentSlide = Slides.One;
            display = new Rectangle(0, 0, Globals.screenW, Globals.screenH);
            this.game = game;
        }
        public override void Update(GameTime gameTime)
        {
            gp = GamePad.GetState(PlayerIndex.One);
            if (gp.Buttons.A == ButtonState.Pressed && gpO.Buttons.A != ButtonState.Pressed)
            {
                if (currentSlide != Slides.Three)
                    currentSlide++;
                else
                {
                    game.currentScreen = new TitleScreen(this.game,gp);
                    game.currentScreen.game = this.game;
                }

            }
            if (gp.Buttons.B == ButtonState.Pressed && gpO.Buttons.B != ButtonState.Pressed)
            {
                if(currentSlide!=Slides.One)
                    currentSlide--;
                else
                {
                    game.currentScreen = new TitleScreen(this.game,gp);
                    game.currentScreen.game = this.game;
                }
            }
            gpO = gp;
        }

        public override void Draw(GameTime gameTime)
        {
            game.spriteBatch.Draw(helpSlides[(int)currentSlide], display, Color.White);

        }
    }
}
