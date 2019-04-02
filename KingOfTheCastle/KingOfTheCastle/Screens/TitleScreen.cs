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
        Texture2D logo;
        Rectangle logoPos;
        Vector2 textpos;
        public TitleScreen(KingOfTheCastle game)
        {
            logo = game.Content.Load<Texture2D>("logo");
            logoPos = new Rectangle(400, 100, 800, 700);
            textpos = new Vector2(475, 770);
        }

        public override void Update(GameTime gameTime)
        {
            GamePadState pad1 = GamePad.GetState(0);
            if (pad1.IsButtonDown(Buttons.Start) || pad1.IsButtonDown(Buttons.A))
            {
                game.currentScreen = new Stage(0);
                game.currentScreen.game = game;
            }
            

        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(new Color(Globals.rng.Next(200),Globals.rng.Next(200),Globals.rng.Next(200)));
            game.spriteBatch.Draw(logo, logoPos, Color.White);
            game.spriteBatch.DrawString(game.font, "     Press Start or A...\nPress Back or Escape to exit", textpos, Color.White);


        }
    }
}
