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
    public class FirstKillQuest:Quest
    {
        Color titleColor;
        public FirstKillQuest(KingOfTheCastle game)
        {
            this.game = game;
            title = "FIRST BLOOD";
            titleLoc = new Vector2(Globals.screenW / 2 - 140, 0);
            yLoc = display.Y;

            titleColor = Color.White;
            questComplete = game.Content.Load<SoundEffect>("questComplete");
            crown = game.Content.Load<Texture2D>("crown");
        }

        public override void check()
        {
            if(!isCompleted)
            {
                foreach(Player p in game.players)
                {
                    if(p!=null)
                    {
                        if(p.roundKills>=1&&!p.completedMainQuest)
                        {
                            p.completedMainQuest = true;
                            isCompleted = true;
                            winner = p;
                            questComplete.Play();
                            oldDisplay = display;
                            p.gold += 15 + (5 * (game.round - 1));
                            break;
                        }
                    }
                }
            }
            if (isCompleted)
            {
                yLoc -= yVel;
                titleLoc.Y -= (float)yVel;
                for (int x = 0; x < yLocPlayers.Length; x++)
                {
                    yLocPlayers[x] -= yVel;
                    playerCompletionLocs[x].Y = (int)yLocPlayers[x];
                }
                if (timer > 30 && timer < 60 || timer > 630 && timer < 660)
                    yVel += .1;
                display.Y = (int)yLoc;
                timer++;
                titleColor = Color.Gold;
                if (timer == 600)
                {
                    yVel = .1;
                    yLoc = oldDisplay.Y;
                }
                if (timer >= 600)
                {
                    winnerText.Y -= (float)yVel;
                    oldDisplay.Y = (int)yLoc;
                }

            }
        }

        public override void Draw()
        {
            if (isCompleted)
            {
                game.spriteBatch.Draw(game.test, oldDisplay, Color.Gray);
                game.spriteBatch.Draw(crown, new Rectangle(oldDisplay.X + oldDisplay.Width / 2 - 50, oldDisplay.Y + 100, 100, 100), winner.playerColor);
                game.spriteBatch.DrawString(game.smallFont, "      Player " + winner.playerNumber + " \ncompleted the quest!", winnerText, winner.playerColor);
            }
            if (timer < 599)
                game.spriteBatch.Draw(game.questBackdrop, display, Color.White);
            game.spriteBatch.DrawString(game.font, title, titleLoc, titleColor);
        }
    }
}
