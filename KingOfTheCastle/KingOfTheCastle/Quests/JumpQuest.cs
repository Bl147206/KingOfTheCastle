using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace KingOfTheCastle
{
    public class JumpQuest:Quest
    {
        int requiredJumpAmt;
        Color titleColor;
        public JumpQuest(KingOfTheCastle game)
        {
            this.game = game;
            requiredJumpAmt = game.round * Globals.rng.Next(5,10);
            title = "JUMP " + requiredJumpAmt + " TIMES";
            display = new Rectangle(Globals.screenW / 2 - 150, 0, 300, 200);
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
                            isCompleted = true;
                            p.gold += 10 + (5 * (game.round - 1));
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
                if(timer>30&&timer<60)
                    yVel += .1;
                display.Y = (int)yLoc;
                timer++;
                titleColor = Color.Yellow;
            }
            
            
                
        }

        public override void Draw()
        {
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
