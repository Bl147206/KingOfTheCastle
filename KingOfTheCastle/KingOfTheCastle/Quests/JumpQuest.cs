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
        public JumpQuest(KingOfTheCastle game)
        {
            this.game = game;
            requiredJumpAmt = game.round * Globals.rng.Next(5,10);
            title = "JUMP " + requiredJumpAmt + " TIMES";
            display = new Rectangle(Globals.screenW / 2 - 150, 0, 300, 200);
            titleLoc = new Vector2(Globals.screenW / 2 - 140, 0);
            for(int i=0;i<playerCompletionLocs.Length;i++)
            {
                playerCompletionLocs[i] = new Vector2(Globals.screenW / 2 +(75*(i-2))+20, 100+((i%2)*50));
            }
            for(int i=0;i<playerCompletionProgress.Length;i++)
            {
                playerCompletionProgress[i] = "0/"+requiredJumpAmt;
            }
            
        }
        public override void check()
        {
            foreach(Player p in game.players)
            {
                if (p != null)
                {
                    if (!p.completedMainQuest)
                        playerCompletionProgress[Array.IndexOf(game.players, p)] = p.jumps + "/" + requiredJumpAmt;

                    if (p.jumps >= requiredJumpAmt && !p.completedMainQuest)
                    {
                        p.completedMainQuest = true;
                        p.gold += 10 + (5 * (game.round - 1));
                    }
                }
            }
        }

        public override void Draw()
        {
            game.spriteBatch.Draw(game.questBackdrop, display, Color.White);
            game.spriteBatch.DrawString(game.font, title, titleLoc, Color.White);
            for(int i=0;i<playerCompletionLocs.Length;i++)
            {
                if(game.players[i]!=null)
                    game.spriteBatch.DrawString(game.smallFont, playerCompletionProgress[i], playerCompletionLocs[i], game.players[i].playerColor);
            }
        }
    }
}
