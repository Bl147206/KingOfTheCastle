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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class KingOfTheCastle : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public SpriteFont font;
        public SpriteBatch spriteBatch;
        public Texture2D test;
        public Screen currentScreen;
        KeyboardState kb;
        public Texture2D shopText;
        public Texture2D shopHighlight;
        public Texture2D swordTexture;
        public int round = 1;
        public SpriteFont smallFont;
        public Player[] players;
        public Texture2D bowTexture;
        public GamePadState[] oldGamePadStates;
        public Texture2D questBackdrop;
        public Texture2D character;
        public Texture2D Coin;
        public Texture2D[] backgrounds;
        public Texture2D swordAttackT;
        public SpriteFont playerFont;
        public Texture2D arrow;
        public Texture2D arrowF;

        public KingOfTheCastle()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            IsMouseVisible = false;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            
            currentScreen = new TitleScreen(this);
            //currentScreen = new Stage(0);
            currentScreen.game = this;
            backgrounds = new Texture2D[7];
            players = new Player[4];

            for (int i = 0; i < getControllerCount(); i += 1) {
                Rectangle tempRec = new Rectangle(Globals.screenW / (2 * (i+1)), Globals.screenH - (250 * (i+1)), 60, 60);
                if(i==0)
                players[i] = new Player(this, tempRec, test, /*index*/ i + 1,Color.IndianRed);
                if (i == 1)
                    players[i] = new Player(this, tempRec, test, /*index*/ i + 1, Color.LightBlue);
                if (i == 2)
                    players[i] = new Player(this, tempRec, test, /*index*/ i + 1, Color.LightGreen);
                if (i == 3)
                    players[i] = new Player(this, tempRec, test, /*index*/ i + 1, Color.Goldenrod);
            }

            oldGamePadStates = new GamePadState[4];

            base.Initialize();
        }

        public int getControllerCount() {
            int result = 0;

            for (int i = 0; i < 4; i += 1) {
                PlayerIndex idx;

                switch (i + 1) {
                    case 1:
                        idx = PlayerIndex.One;
                        break;
                    case 2:
                        idx = PlayerIndex.Two;
                        break;
                    case 3:
                        idx = PlayerIndex.Three;
                        break;
                    case 4:
                        idx = PlayerIndex.Four;
                        break;
                    default:
                        idx = PlayerIndex.One;
                        break;
                }

                result += GamePad.GetState(idx).IsConnected ? 1 : 0;
            }

            return result;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            playerFont = Content.Load<SpriteFont>("playerFont");
            test = Content.Load<Texture2D>("blank");
            smallFont = Content.Load<SpriteFont>("sFont");
            character = Content.Load<Texture2D>("Character");
            Coin = Content.Load<Texture2D>("Coin");
            swordAttackT = Content.Load<Texture2D>("swordSwipe");
            arrow = Content.Load<Texture2D>("arrow");
            arrowF = Content.Load<Texture2D>("arrowF");

            for(int x = 0; x<backgrounds.Length;x++)
            {
                backgrounds[x] = Content.Load<Texture2D>("backdrop" + x);
            }
            foreach (Player p in players)
            {
                if(p != null)
                {
                    p.texture = character;
                }
            }
            shopText = Content.Load<Texture2D>("Shop");
            // TODO: use this.Content to load your game content here
            font = Content.Load<SpriteFont>("SpriteFont1");
            shopHighlight = Content.Load<Texture2D>("border");
            swordTexture = Content.Load<Texture2D>("sword");
            bowTexture = Content.Load<Texture2D>("bow");
            questBackdrop = Content.Load<Texture2D>("questback");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed||kb.IsKeyDown(Keys.Escape))
                this.Exit();
            kb = Keyboard.GetState();

            // TODO: Add your update logic here

            currentScreen.Update(gameTime);

            for (int i = 0; i < 4; i++) {
                if (oldGamePadStates == null)
                    break;

                oldGamePadStates[i] = GamePad.GetState((PlayerIndex) i);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here

            spriteBatch.Begin();

            currentScreen.Draw(gameTime);

            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
