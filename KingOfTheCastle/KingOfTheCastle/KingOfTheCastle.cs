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
        public SpriteFont smallerFont;
        public Player[] players;
        public Texture2D bowTexture;
        public GamePadState[] oldGamePadStates;
        public Texture2D questBackdrop;
        public Texture2D character;
        public Texture2D Coin;
        public Texture2D shieldTex;
        public Texture2D[] backgrounds;
        public Texture2D swordAttackT;
        public SpriteFont playerFont;
        public SpriteFont damageFont;
        public Texture2D arrow;
        public Texture2D arrowF;
        public Texture2D armorTexture;
        public SoundEffect cheer;
        public SoundEffect strongHit;
        //public Server server;
        //public static string serverStatus;

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
            backgrounds = new Texture2D[7];
            players = new Player[4];

            oldGamePadStates = new GamePadState[4];

            //serverStatus = "Press X to start LAN server.";

            //Client.start();

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

        /*
        public void startServer() {
            server = new Server();
            server.start();
        }
        */

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
            damageFont = Content.Load<SpriteFont>("spritefont3");
            character = Content.Load<Texture2D>("Character");
            Coin = Content.Load<Texture2D>("Coin");
            swordAttackT = Content.Load<Texture2D>("swordSwipe");
            arrow = Content.Load<Texture2D>("arrow");
            arrowF = Content.Load<Texture2D>("arrowF");
            armorTexture = Content.Load<Texture2D>("Armor");
            shieldTex = Content.Load<Texture2D>("shield");
            smallerFont = Content.Load<SpriteFont>("SpriteFont2");
            cheer = Content.Load<SoundEffect>("cheer");
            strongHit = Content.Load<SoundEffect>("stronghit");

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

        /*
        public void sendStart() {
            // Get current platform and send to it clients. Tell them to start the game.
            Server.Send("s?EOF");

            var stage = currentScreen as Stage;
            foreach (var platform in stage.platforms) {
                var dest = platform.destination;
                Server.Send("p?" + dest.X + "," + dest.Y + "," + dest.Width + "," + dest.Height + "EOF");
            }
        }
        */

    }
}
