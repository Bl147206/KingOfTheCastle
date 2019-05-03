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
    class Shop: Screen
    {
        enum playerSelection
        {
            One,
            Two,
            Three
        };
        playerSelection[] p = new playerSelection[4];
        Rectangle[] pSelect = new Rectangle[4];
        GamePadState[] playerPad= new GamePadState[4];
        GamePadState[] playerPad0 = new GamePadState[4];
        Rectangle[,] items = new Rectangle[4,3];
        Texture2D[,] itemsT = new Texture2D[4, 3];
        Color[,] itemsC = new Color[4, 3];
        int[] goldTotals;
        Texture2D background;
        int frames;
        int seconds;
        String timeleft;
        Texture2D blank;
        Inventory[] inventories = new Inventory[4];
        string[,] stats = new string[4, 3];
        SoundEffect buyItem;
        SoundEffect click;
        SpriteFont timerFont;
        int startTime;


        public Shop(KingOfTheCastle game)
        {
            
            background = game.shopText;
            blank = game.test;
            buyItem = game.Content.Load<SoundEffect>("purchase");
            click = game.Content.Load<SoundEffect>("timeClick");
            for (int x = 0; x < pSelect.Length; x++)
            {
                p[x] = playerSelection.One;
            }
            pSelect[0] = new Rectangle(screenAdjust(80, "W"), screenAdjust(20, "H"), screenAdjust(140, "W"), screenAdjust(135, "H"));//20,20,140,135
            pSelect[1] = new Rectangle(screenAdjust(1095, "W"), screenAdjust(15, "H"), screenAdjust(140, "W"), screenAdjust(135, "H"));//1095,15,140,135
            pSelect[2] = new Rectangle(screenAdjust(80, "W"), screenAdjust(610, "H"), screenAdjust(140, "W"), screenAdjust(135, "H"));//80,610,140,135
            pSelect[3] = new Rectangle(screenAdjust(1095, "W"), screenAdjust(600, "H"), screenAdjust(140, "W"), screenAdjust(135, "H"));//1095,600,140,135
            for (int x = 0; x < inventories.Length; x++)
            {
                inventories[x] = new Inventory(game.round, game);
                for (int y = 0; y < itemsT.GetLength(1); y++)
                {
                    itemsT[x, y] = inventories[x].weapons[y].texture;
                    itemsC[x, y] = inventories[x].weapons[y].color;
                    string type = "";
                    string speed = Math.Round(1/inventories[x].weapons[y].attackSpeed,2)+"";
                    if (inventories[x].weapons[y].texture == game.swordTexture)
                        type = "Melee";
                    else if (inventories[x].weapons[y].texture == game.bowTexture)
                        type = "Ranged";
                    else
                        type = "Armor";
                    if(type!="Armor")
                    stats[x, y] = "Name: " + inventories[x].weapons[y].name + "\nType: " + type + "\nCost: " + inventories[x].weapons[y].cost + "\nAttack Speed: " + speed + "\nDamage: " 
                        + inventories[x].weapons[y].attack;
                    else
                        stats[x, y] = "Name: " + inventories[x].weapons[y].name + "\nType: " + type + "\nCost: " + inventories[x].weapons[y].cost + "\nArmor: " + inventories[x].weapons[y].armorBonus;
                }
            }
            items[0, 0] = new Rectangle(screenAdjust(80, "W"), screenAdjust(20, "H"), screenAdjust(140, "W"), screenAdjust(130, "H"));
            items[0, 1] = new Rectangle(screenAdjust(80, "W"), screenAdjust(185, "H"), screenAdjust(140, "W"), screenAdjust(130, "H"));//185
            items[0, 2] = new Rectangle(screenAdjust(80, "W"), screenAdjust(360, "H"), screenAdjust(140, "W"), screenAdjust(130, "H"));//360
            items[1, 0] = new Rectangle(screenAdjust(1095, "W"), screenAdjust(15, "H"), screenAdjust(140, "W"), screenAdjust(130, "H"));
            items[1, 1] = new Rectangle(screenAdjust(1095, "W"), screenAdjust(175, "H"), screenAdjust(140, "W"), screenAdjust(130, "H"));//175
            items[1, 2] = new Rectangle(screenAdjust(1095, "W"), screenAdjust(350, "H"), screenAdjust(140, "W"), screenAdjust(130, "H"));//350
            items[2, 0] = new Rectangle(screenAdjust(80, "W"), screenAdjust(610, "H"), screenAdjust(140, "W"), screenAdjust(130, "H"));
            items[2, 1] = new Rectangle(screenAdjust(80, "W"), screenAdjust(770, "H"), screenAdjust(140, "W"), screenAdjust(130, "H"));//770
            items[2, 2] = new Rectangle(screenAdjust(80, "W"), screenAdjust(945, "H"), screenAdjust(140, "W"), screenAdjust(130, "H"));//945
            items[3, 0] = new Rectangle(screenAdjust(1095, "W"), screenAdjust(600, "H"), screenAdjust(140, "W"), screenAdjust(130, "H"));
            items[3, 1] = new Rectangle(screenAdjust(1095, "W"), screenAdjust(770, "H"), screenAdjust(140, "W"), screenAdjust(130, "H"));//770
            items[3, 2] = new Rectangle(screenAdjust(1095, "W"), screenAdjust(945, "H"), screenAdjust(140, "W"), screenAdjust(130, "H"));//945

            frames = 0;
            startTime = 10;
            seconds = startTime;
            timeleft = "" + seconds;
            goldTotals = new int[game.getControllerCount()];
            foreach(Player p in game.players)
            {
                if(p != null)
                {
                    playerPad0[p.playerNumber - 1] = p.oldGamePad;
                }
            }
            timerFont = game.Content.Load<SpriteFont>("storetimeFont");
            this.game = game;
        }
        public override void Update(GameTime gameTime) {
            if (frames % 60 == 0&&frames!=0)
                click.Play();
            playerPad[0] = GamePad.GetState(PlayerIndex.One);
            playerPad[1] = GamePad.GetState(PlayerIndex.Two);
            playerPad[2] = GamePad.GetState(PlayerIndex.Three);
            playerPad[3] = GamePad.GetState(PlayerIndex.Four);
            for (int x = 0; x < goldTotals.Length; x++)
            {
                goldTotals[x] = game.players[x].gold;
            }
            for (int x = 0; x < playerPad.Length; x++)
            {
                
                if (playerPad[x].ThumbSticks.Left.Y < -.5 && playerPad0[x].ThumbSticks.Left.Y > -.5)
                {
                    if (p[x] != playerSelection.Three)
                    {
                        p[x]++;
                    }
                    else
                    {
                        p[x] = playerSelection.One;
                    }
                }

                if (playerPad[x].ThumbSticks.Left.Y > .5&& playerPad0[x].ThumbSticks.Left.Y<.5)
                {
                    if (p[x] != playerSelection.One)
                    {
                        p[x]--;
                    }
                    else
                    {
                        p[x] = playerSelection.Three;
                    }
                }
                if(playerPad[x].Buttons.A == ButtonState.Pressed && playerPad0[x].Buttons.A != ButtonState.Pressed)//Item Bought
                {
                    if(p[x]==playerSelection.One)
                    {
                        if (itemsT[x, 0] != blank && game.players[x].gold >= inventories[x].weapons[0].cost)
                        {
                            if (inventories[x].weapons[0].texture == game.swordTexture)
                            {
                                game.players[x].mAttack = inventories[x].weapons[0].attack;
                                game.players[x].mAttackSpeed = inventories[x].weapons[0].attackSpeed;
                                game.players[x].meleeColor = inventories[x].weapons[0].color;
                            }
                            if (inventories[x].weapons[0].texture == game.bowTexture)
                            {
                                game.players[x].rAttack = inventories[x].weapons[0].attack;
                                game.players[x].rAttackSpeed = inventories[x].weapons[0].attackSpeed;
                                game.players[x].rangedColor = inventories[x].weapons[0].color;
                            }
                            if (inventories[x].weapons[0].texture == game.armorTexture)
                            {
                                game.players[x].maxHealth = 20 + inventories[x].weapons[0].armorBonus;
                            }
                            game.players[x].gold -= inventories[x].weapons[0].cost;

                            itemsT[x, 0] = blank;
                            itemsC[x, 0] = Color.Brown;
                            buyItem.Play();
                        }
                    }
                    if (p[x] == playerSelection.Two)
                    {
                        if (itemsT[x, 1] != blank && game.players[x].gold >= inventories[x].weapons[1].cost)
                        {
                            if (inventories[x].weapons[1].texture == game.swordTexture)
                            {
                                game.players[x].mAttack = inventories[x].weapons[1].attack;
                                game.players[x].mAttackSpeed = inventories[x].weapons[1].attackSpeed;
                                game.players[x].meleeColor = inventories[x].weapons[1].color;
                            }
                            if (inventories[x].weapons[1].texture == game.bowTexture)
                            {
                                game.players[x].rAttack = inventories[x].weapons[1].attack;
                                game.players[x].rAttackSpeed = inventories[x].weapons[1].attackSpeed;
                                game.players[x].rangedColor = inventories[x].weapons[1].color;
                            }
                            if (inventories[x].weapons[1].texture == game.armorTexture)
                            {
                                game.players[x].maxHealth = 20 + inventories[x].weapons[0].armorBonus;
                            }
                            game.players[x].gold -= inventories[x].weapons[1].cost;

                            itemsT[x, 1] = blank;
                            itemsC[x, 1] = Color.Brown;
                            buyItem.Play();
                        }
                    }
                    if (p[x] == playerSelection.Three)
                    {
                        if (itemsT[x, 2] != blank && game.players[x].gold >= inventories[x].weapons[2].cost)
                        {
                            if (inventories[x].weapons[2].texture == game.swordTexture)
                            {
                                game.players[x].mAttack = inventories[x].weapons[2].attack;
                                game.players[x].mAttackSpeed = inventories[x].weapons[2].attackSpeed;
                                game.players[x].meleeColor = inventories[x].weapons[2].color;
                            }
                            if (inventories[x].weapons[2].texture == game.bowTexture)
                            {
                                game.players[x].rAttack = inventories[x].weapons[2].attack;
                                game.players[x].rAttackSpeed = inventories[x].weapons[2].attackSpeed;
                                game.players[x].rangedColor = inventories[x].weapons[2].color;
                            }
                            if (inventories[x].weapons[2].texture == game.armorTexture)
                            {
                                game.players[x].maxHealth = 20 + inventories[x].weapons[2].armorBonus;
                            }
                            game.players[x].gold -= inventories[x].weapons[2].cost;

                            itemsT[x, 2] = blank;
                            itemsC[x, 2] = Color.Brown;
                            buyItem.Play();
                        }
                    }
                }
            }
            //Player 1
            if (p[0] == playerSelection.One)
            {
                pSelect[0].Y = screenAdjust(20, "H");
            }
            if (p[0] == playerSelection.Two)
            {
                pSelect[0].Y = screenAdjust(185, "H");
            }
            if (p[0] == playerSelection.Three)
            {
                pSelect[0].Y = screenAdjust(360, "H");
            }
            //Player 2
            if (p[1] == playerSelection.One)
            {
                pSelect[1].Y = screenAdjust(15, "H");
            }
            if (p[1] == playerSelection.Two)
            {
                pSelect[1].Y = screenAdjust(175, "H");
            }
            if (p[1] == playerSelection.Three)
            {
                pSelect[1].Y = screenAdjust(350, "H");
            }
            //Player 3
            if (p[2] == playerSelection.One)
            {
                pSelect[2].Y = screenAdjust(610, "H");
            }
            if (p[2] == playerSelection.Two)
            {
                pSelect[2].Y = screenAdjust(770, "H");
            }
            if (p[2] == playerSelection.Three)
            {
                pSelect[2].Y = screenAdjust(945, "H");
            }
            //Player 4
            if (p[3] == playerSelection.One)
            {
                pSelect[3].Y = screenAdjust(600, "H");
            }
            if (p[3] == playerSelection.Two)
            {
                pSelect[3].Y = screenAdjust(770, "H");
            }
            if (p[3] == playerSelection.Three)
            {
                pSelect[3].Y = screenAdjust(945,"H");
            }

            playerPad0[0] = playerPad[0];
            playerPad0[1] = playerPad[1];
            playerPad0[2] = playerPad[2];
            playerPad0[3] = playerPad[3];
            frames++;
            timeleft = "" + ((60 * seconds - frames) / 60 + 1);
            if (frames >= 60 * (startTime))
            {
                game.round++;
                game.currentScreen = new Stage(game.round, this.game);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(new Color(180,140,100));
            game.spriteBatch.Draw(background, new Rectangle(0, 0, Globals.screenW, Globals.screenH), Color.White);
            for (int y = 0; y < inventories.Length; y++)//Weapons
            {
                for (int z = 0; z < inventories[y].weapons.Length; z++)
                {
                        game.spriteBatch.Draw(itemsT[y, z], items[y, z], itemsC[y,z]);
                }
            }
            foreach (Rectangle x in pSelect)//Highlight
            {
                game.spriteBatch.Draw(game.shopHighlight, x, Color.White);
            }

            for (int x = 0; x < p.Length; x++)//Stat blocks
            {
                switch (p[x])
                {
                    case (playerSelection.One):
                        if (itemsT[x, 0] != blank)
                            drawStats(stats[x, 0], new Vector2(items[x, 0].X + screenAdjust(190, "W"), items[x, 0].Y - 10));
                        break;
                    case (playerSelection.Two):
                        if (itemsT[x, 1] != blank)
                            drawStats(stats[x, 1], new Vector2(items[x, 1].X + screenAdjust(190, "W"), items[x, 1].Y - 10));
                        break;
                    case (playerSelection.Three):
                        if (itemsT[x, 2] != blank)
                            drawStats(stats[x, 2], new Vector2(items[x, 2].X + screenAdjust(190, "W"), items[x, 2].Y - 10));
                        break;
                }
            }
                switch(goldTotals.Length-1)
                {
                    case (0):
                    game.spriteBatch.Draw(game.Coin, new Rectangle(screenAdjust(795, "W"), screenAdjust(392, "H"), 70, 70), Color.White);
                    game.spriteBatch.DrawString(game.font, "$" + goldTotals[0], new Vector2((float)screenAdjust(800,"W"), (float)screenAdjust(400,"H")), Color.Gold);
                    
                        break;
                    case (1):
                    game.spriteBatch.Draw(game.Coin, new Rectangle(screenAdjust(795, "W"), screenAdjust(392, "H"), 70, 70), Color.White);
                    game.spriteBatch.Draw(game.Coin, new Rectangle(screenAdjust(1845, "W"), screenAdjust(392, "H"), 70, 70), Color.White);
                    game.spriteBatch.DrawString(game.font, "$" + goldTotals[0], new Vector2((float)screenAdjust(800, "W"), (float)screenAdjust(400, "H")), Color.Gold);
                    game.spriteBatch.DrawString(game.font, "$" + goldTotals[1], new Vector2((float)screenAdjust(1850, "W"), (float)screenAdjust(400, "H")), Color.Gold);
                   
                    break;
                    case (2):
                    game.spriteBatch.Draw(game.Coin, new Rectangle(screenAdjust(795, "W"), screenAdjust(392, "H"), 70, 70), Color.White);
                    game.spriteBatch.Draw(game.Coin, new Rectangle(screenAdjust(1845, "W"), screenAdjust(392, "H"), 70, 70), Color.White);
                    game.spriteBatch.Draw(game.Coin, new Rectangle(screenAdjust(795, "W"), screenAdjust(992, "H"), 70, 70), Color.White);
                    game.spriteBatch.DrawString(game.font, "$" + goldTotals[0], new Vector2((float)screenAdjust(800, "W"), (float)screenAdjust(400, "H")), Color.Gold);
                    game.spriteBatch.DrawString(game.font, "$" + goldTotals[1], new Vector2((float)screenAdjust(1850, "W"), (float)screenAdjust(400, "H")), Color.Gold);
                    game.spriteBatch.DrawString(game.font, "$" + goldTotals[2], new Vector2((float)screenAdjust(800, "W"), (float)screenAdjust(1000,"H")), Color.Gold);
                    
                    break;
                    case (3):
                    game.spriteBatch.Draw(game.Coin, new Rectangle(screenAdjust(795, "W"), screenAdjust(392, "H"), 70, 70), Color.White);
                    game.spriteBatch.Draw(game.Coin, new Rectangle(screenAdjust(1845, "W"), screenAdjust(392, "H"), 70, 70), Color.White);
                    game.spriteBatch.Draw(game.Coin, new Rectangle(screenAdjust(795, "W"), screenAdjust(992, "H"), 70, 70), Color.White);
                    game.spriteBatch.Draw(game.Coin, new Rectangle(screenAdjust(1845, "W"), screenAdjust(992, "H"), 70, 70), Color.White);
                    game.spriteBatch.DrawString(game.font, "$" + goldTotals[0], new Vector2((float)screenAdjust(800, "W"), (float)screenAdjust(400, "H")), Color.Gold);
                    game.spriteBatch.DrawString(game.font, "$" + goldTotals[1], new Vector2((float)screenAdjust(1850, "W"), (float)screenAdjust(400, "H")), Color.Gold);
                    game.spriteBatch.DrawString(game.font, "$" + goldTotals[2], new Vector2((float)screenAdjust(800, "W"), (float)screenAdjust(1000, "H")), Color.Gold);
                    game.spriteBatch.DrawString(game.font, "$" + goldTotals[3], new Vector2((float)screenAdjust(1850, "W"), (float)screenAdjust(1000, "H")), Color.Gold);
                    
                    break;
                }
            if(startTime - (frames / 60)>=10)
            game.spriteBatch.DrawString(timerFont, "Time Left\n        "+(startTime - (frames / 60)), new Vector2(Globals.screenW / 2 - 80, Globals.screenH / 2 - 70), Color.White);
            else
                game.spriteBatch.DrawString(timerFont, "Time Left\n         " + (startTime - (frames / 60)), new Vector2(Globals.screenW / 2 - 80, Globals.screenH / 2 - 70), Color.White);

            //Gold placement: P1: 800,400   P2: 1850,400    P3: 800,1000    P4: 1850,1000

        }
        public int screenAdjust(int value, string WorH)
        {
            int final = 0;
            if(WorH=="H")
            {
                final = value*(Globals.screenH / 1080);
            }
            if(WorH =="W")
            {
                final = value*(Globals.screenW / 1920);
            }
            return final;
        }

        public void drawStats(string stats, Vector2 destination)
        {
            game.spriteBatch.Draw(game.test, new Rectangle((int)destination.X, (int)destination.Y, screenAdjust(400,"W"), screenAdjust(180,"H")), Color.Black);
            game.spriteBatch.DrawString(game.smallFont, stats, new Vector2((int)destination.X+10, (int)destination.Y+10), Color.AntiqueWhite);
        }
    }
}
