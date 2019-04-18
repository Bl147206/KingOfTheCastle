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
        string title;
        Rectangle display;
        public JumpQuest(KingOfTheCastle game)
        {
            this.game = game;
            requiredJumpAmt = game.round * Globals.rng.Next(3,10);
            title = "JUMP " + requiredJumpAmt + " TIMES";
            display = new Rectangle(Globals.screenW / 2 - 150, 0, 300, 200);
        }
        public override void check()
        {
            foreach(Player p in game.players)
            {
                if(p!=null)
                    if (p.jumps >= requiredJumpAmt && !p.completedMainQuest)
                    {
                      p.completedMainQuest = true;
                      p.gold += 10 + (5 * (game.round - 1));
                    }
            }
        }

        public override void Draw()
        {
            game.spriteBatch.Draw(game.questBackdrop, display, Color.White);
        }
    }
}
