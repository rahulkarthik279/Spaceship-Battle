using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Spaceship_Battle
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        int w, h;
        StartMenu startmenu;
        LoadingScreen load;
        PauseGame pause;
        Level level;
        public enum GameState { Start, Load, Instructions, Level, Pause, Complete };
        public static GameState gamestate;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
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
            w = GraphicsDevice.Viewport.Width;
            h = GraphicsDevice.Viewport.Height;
            gamestate = GameState.Start;
            Background.initialize(GraphicsDevice);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            startmenu = new StartMenu(Services, w, h);
            load = new LoadingScreen(Services, w, h);
            pause = new PauseGame(Services);
            Level.LoadContent(Services, w, h);
            Instructions.LoadContent();
            // TODO: use this.Content to load your game content here
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
            
            switch (gamestate)
            {
                case GameState.Start:
                    gamestate += startmenu.update(gameTime);
                    return;
                case GameState.Load:
                    {
                        int starter = 1;
                        if (startmenu.buttons[0].active)
                        {
                            try
                            {
                                Console.WriteLine(PauseGame.savefilename);
                                String f = File.ReadAllText(PauseGame.savefilename);
                                starter = Convert.ToInt32(f);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("REEREEEEEEEE");
                                Console.WriteLine(e.Message);
                            }
                        }
                        startLevel(starter);

                        gamestate+=2;
                        break;

                    }
                case GameState.Level:
                    level.update(gameTime);
                    gamestate += pause.update(gameTime);
                    break;
                case GameState.Pause:
                    gamestate -= pause.update(gameTime);
                    if (pause.exit)
                    {
                        this.Exit();
                    }
                    break;
                case GameState.Instructions:
                    gamestate += Instructions.update();
                    break;
                case GameState.Complete:
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                        this.Exit();
                    break;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            switch (gamestate)
            {
                case GameState.Start:
                    startmenu.draw(gameTime, spriteBatch);
                    break;
                case GameState.Load:
                    load.draw(spriteBatch);
                    break;
                case GameState.Level:
                    level.draw(spriteBatch, gameTime);
                    pause.draw(spriteBatch, gameTime);
                    break;
                case GameState.Pause:
                    //level.draw(spriteBatch, gameTime);
                    pause.draw(spriteBatch, gameTime);
                    break;
                case GameState.Instructions:
                    Instructions.draw(spriteBatch, gameTime);
                    break;
                case GameState.Complete:
                    spriteBatch.DrawString(StartMenu.font, "Congrats! You have completed the mission!!!!\nClick back on controller or escape on keyboard to exit the game", new Vector2(200, 100), Color.White);
                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void startLevel(int numlevel)
        {
            graphics.PreferredBackBufferHeight = 400;
            graphics.PreferredBackBufferWidth = 800;
            graphics.ApplyChanges();
            w = GraphicsDevice.Viewport.Width;
            h = GraphicsDevice.Viewport.Height;
            level = new Level(Services, GraphicsDevice, numlevel, w, h);
        }
    }
}