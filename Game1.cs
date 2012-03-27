using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace FiniteStateMachine
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        Miner Bob;
        MinersWife Elsa;
        Outlaw Jesse;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        public static Texture2D bg;

        public static int numCellsX = 25;
        public static int numCellsY = 15;
        public static LocationGrid grid;

        public static int cellWidth = 32;
        public static int cellHeight = 32;

        public static int screenWidth = numCellsX * cellWidth;
        public static int screenHeight = numCellsY * cellHeight + 200;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            grid = new LocationGrid(numCellsX, numCellsY);
            grid.AddRandomLocation("mountain", "mountain", 15, 0.2d);
            grid.AddRandomLocation("plain", "", 1, 1.3d);
            grid.AddRandomLocation("water", "water", 4, 0.3d);
            grid.Populate();
  
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Here's a little hack: The Miner and MinersWife must know each other's id in
            // order to communicate.  We calculate them inside each agent based on their
            // creation order, so the pair must always be created in this sequence.
            Bob = new Miner();
            Elsa = new MinersWife();
            Jesse = new Outlaw();
            AgentManager.AddAgent(Bob);
            AgentManager.AddAgent(Elsa);
            AgentManager.AddAgent(Jesse);
            // TODO: We could add more agents here
            Printer.offset.Y = numCellsY * cellHeight;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>("Arial");
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.ApplyChanges();
            bg = Content.Load<Texture2D>("bg");
            grid.LoadContent(Content);
            AgentManager.LoadContent(Content);
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
        /// 

        int frameCounter = 0;

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            Message.gameTime = gameTime;
            if (frameCounter > 180)
            {
                Bob.Update();
                Elsa.Update();
                Jesse.Update();
                Message.SendDelayedMessages();
                frameCounter = 0;
            }
            ++frameCounter;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            grid.Draw(spriteBatch);
            AgentManager.Draw(spriteBatch);
            Printer.Draw(spriteBatch, spriteFont);

            base.Draw(gameTime);
        }
    }
}
