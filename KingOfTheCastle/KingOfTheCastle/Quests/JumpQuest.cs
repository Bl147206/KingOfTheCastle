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
    public class JumpQuest:Quest
    {
        int requiredJumpAmt;
        Color titleColor;
        public JumpQuest(KingOfTheCastle game)
        {
            this.game = game;
            requiredJumpAmt = (int)Math.Round((double)(game.round)/4 * Globals.rng.Next(10,15));
            title = "JUMP " + requiredJumpAmt + " TIMES";            
            titleLoc = new Vector2(Globals.screenW / 2 - 140, 0);
            yLoc = display.Y;
            
            for(int i=0;i<playerCompletionLocs.Length;i++)
            {
                playerCompletionLocs[i] = new Vector2(Globals.screenW / 2 +(50*(i-2)), 100+((i%2)*50));
            }
            for(int i=0;i<yLocPlayers.Length;i++)
            {
                yLocPlayers[i] = playerCompletionLocs[i].Y;
            }
            for(int i=0;i<playerCompletionProgress.Length;i++)
            {
                playerCompletionProgress[i] = "0/"+requiredJumpAmt;
            }
            titleColor = Color.White;
            questComplete = game.Content.Load<SoundEffect>("questComplete");
            crown = game.Content.Load<Texture2D>("crown");
            
        }
        public override void check()
        {
            if(!isCompleted)
                foreach(Player p in game.players)
                {
                    if (p != null)
                    {
                        if (!p.completedMainQuest)
                            playerCompletionProgress[Array.IndexOf(game.players, p)] = p.jumps + "/" + requiredJumpAmt;

                        if (p.jumps >= requiredJumpAmt && !p.completedMainQuest)
                        {
                            p.completedMainQuest = true;
                            winner = p;
                            isCompleted = true;
                            questComplete.Play();
                            oldDisplay = display;
                            p.gold += 15 + (5 * (game.round - 1));
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
                if(timer>30&&timer<60||timer>630&&timer<660)
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
            if(isCompleted)
            {
                game.spriteBatch.Draw(game.test, oldDisplay, Color.Gray);
                game.spriteBatch.Draw(crown, new Rectangle(oldDisplay.X + oldDisplay.Width / 2 - 50, oldDisplay.Y + 100, 100, 100), winner.playerColor);
                game.spriteBatch.DrawString(game.smallFont, "      Player " + winner.playerNumber + " \ncompleted the quest!", winnerText, winner.playerColor);
            }
            if(timer<599)
                game.spriteBatch.Draw(game.questBackdrop, display, Color.White);
            game.spriteBatch.DrawString(game.font, title, titleLoc, titleColor);
            for(int i=0;i<playerCompletionLocs.Length;i++)
            {
                if(game.players[i]!=null)
                    game.spriteBatch.DrawString(game.smallFont, playerCompletionProgress[i], playerCompletionLocs[i], game.players[i].playerColor);
                else
                    game.spriteBatch.DrawString(game.smallFont, playerCompletionProgress[i], playerCompletionLocs[i], Color.White);
            }
            
            
        }
    }
}
