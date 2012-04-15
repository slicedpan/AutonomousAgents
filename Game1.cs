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
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        public static Texture2D bg;

        public static int numCellsX = 30;
        public static int numCellsY = 20;
        public static LocationGrid grid;

        public static PathDrawer pathDrawer;

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
            AgentManager.AddAgent(new Miner("Bob"));
            AgentManager.AddAgent(new MinersWife("Elsa"));
            AgentManager.AddAgent(new Outlaw("Jesse"));
            AgentManager.AddAgent(new Sheriff("Wyatt"));
            AgentManager.AddAgent(new Undertaker("Mort"));
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
            pathDrawer = new PathDrawer();
            pathDrawer.LoadContent(Content);
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

        bool agentsActive = true;
        KeyboardState lastState = new KeyboardState();

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            KeyboardState currentState = Keyboard.GetState();
            if (currentState.IsKeyDown(Keys.Escape))
                this.Exit();
            if (currentState.IsKeyDown(Keys.P) && !lastState.IsKeyDown(Keys.P))
                agentsActive = !agentsActive;
            Message.gameTime = gameTime;
            if (agentsActive)
            {
                Message.SendDelayedMessages();
                AgentManager.Update(gameTime);
            }

            lastState = currentState;

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
            pathDrawer.DrawPaths(spriteBatch);
            base.Draw(gameTime);
        }
    }
}
