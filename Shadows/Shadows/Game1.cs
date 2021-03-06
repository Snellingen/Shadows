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

namespace Shadows
{

    public enum GameState
    {
        Menu,
        Playing,
        Pause,
        GameOver,
        GameWin,
    }

    public enum Selected
    {
        PlayGame,
        Continue,
        ExitGame,
        NextSong,
        PauseSong,
        StopSong,
        PlaySong,
        ResumeSong,
        NextLevel,
    }

    public delegate void MyEventHandler(Selected selected);

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        SpriteBatch spriteBatch;

        // Components
        FpsViewer fps;
        GraphicsDeviceManager graphics;
        SpriteManager spriteManager;
        LightManager lightManager;
        InputManager inputManager;
        CollisionManager collisionManager;
        SoundManager soundManager;
        GameScreen activeScreen;
        StartScreen startScreen;
        PauseScreen pauseScreen;
        WinScreen winScreen;
        LooseScreen looseScreen;


        Vector2 inverseMatrixPostion;

        Texture2D floorTexture;
        Texture2D blood;
        RenderTarget2D toApplyLight;

        int screenWidth = 1440;
        int screenHeight = 900;

        Camera camera;
        GameState gameState;
        bool IsComponentsDisabled;
        bool paused;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.IsFullScreen = false;

            //AA 
            graphics.PreferMultiSampling = false;
            //IsMouseVisible = true;
            /*Unlimited FPS :) */
            this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 100.0f);
            this.IsFixedTimeStep = false;
            graphics.SynchronizeWithVerticalRetrace = false;

        }

        protected override void Initialize()
        {

            floorTexture = Content.Load<Texture2D>(@"World\tile");

            // TODO: Add your initialization logic here
            fps = new FpsViewer(this);
            spriteManager = new SpriteManager(this);
            inputManager = new InputManager(this);
            lightManager = new LightManager(this, graphics);
            collisionManager = new CollisionManager(this, new Rectangle(0, 0, screenWidth, screenHeight));
            soundManager = new SoundManager(this);

            camera = new Camera(new Vector2(screenWidth, screenHeight), 1.5f);

            // Add Component
            Components.Add(fps);
            Components.Add(spriteManager);
            Components.Add(inputManager);
            Components.Add(lightManager);
            Components.Add(collisionManager);
            Components.Add(soundManager);

            // Adding draw data to spriteManager) 
            spriteManager.addPlayers(1, new Vector2(100, 100));

            // Adding leves to spriteManager
            spriteManager.addLevels("ShadowHouse1", "MiniHouse1", new Vector2(100, 100), new Rectangle(457, 9, 220, 345), new LightSource[] {
                new LightSource(graphics, 600, LightAreaQuality.Low, Color.White, new Vector2(198, 474)),
                new LightSource(graphics, 500, LightAreaQuality.Low, Color.Aqua, new Vector2(1310, 474)),
                new LightSource(graphics, 500, LightAreaQuality.Low, Color.Gold, new Vector2(786, 474))}, new Vector2[] {
                new Vector2(100, 100), new Vector2( 100, 150),new Vector2( 100, 200),new Vector2( 100, 250),
                new Vector2( 100, 300),new Vector2( 100, 350),new Vector2( 100, 400),new Vector2( 200, 450),
                new Vector2( 100, 500),new Vector2( 100, 550),new Vector2( 100, 600),new Vector2( 105, 650),
                new Vector2( 100, 750),new Vector2( 100, 700),new Vector2( 130, 800),new Vector2( 120, 850),
                new Vector2( 100, 900)});

            spriteManager.addLevels("ShadowHouse2", "Minihouse2", new Vector2(522, 110), new Rectangle(680, 10, 220, 200), new LightSource[] {
                new LightSource(graphics, 600, LightAreaQuality.Low, Color.White, new Vector2(198, 474)),
                new LightSource(graphics, 500, LightAreaQuality.Low, Color.Aqua, new Vector2(1310, 474)),
                new LightSource(graphics, 500, LightAreaQuality.Low, Color.Gold, new Vector2(786, 474))}, new Vector2[] {
                new Vector2(100, 100), new Vector2( 100, 300)});

            spriteManager.addLevels("ShadowHouse3", "Minihouse3", new Vector2(105, 800), new Rectangle(565, 10, 312, 85), new LightSource[] {
                new LightSource(graphics, 600, LightAreaQuality.Low, Color.Red, new Vector2(198, 274)),
                new LightSource(graphics, 500, LightAreaQuality.Low, Color.Aqua, new Vector2(1310, 474)),
                new LightSource(graphics, 500, LightAreaQuality.Low, Color.Gold, new Vector2(786, 374))}, new Vector2[] {
                new Vector2(100, 100), new Vector2( 100, 300)});

            spriteManager.addLevels("ShadowHouse4", "Minihouse4", new Vector2(150, 500), new Rectangle(1297, 379, 137, 245), new LightSource[] {
                new LightSource(graphics, 600, LightAreaQuality.Low, Color.White, new Vector2(198, 474)),
                new LightSource(graphics, 500, LightAreaQuality.Low, Color.Aqua, new Vector2(610, 474)),
                new LightSource(graphics, 500, LightAreaQuality.Low, Color.Gold, new Vector2(786, 474))}, new Vector2[] {
                new Vector2(100, 100), new Vector2( 100, 300)});

            spriteManager.addLevels("ShadowHouse5", "Minihouse5", new Vector2(170, 450), new Rectangle(1130, 280, 304, 400), new LightSource[] {
                new LightSource(graphics, 400, LightAreaQuality.Low, Color.Red, new Vector2(198, 474)),
                new LightSource(graphics, 500, LightAreaQuality.Low, Color.Aqua, new Vector2(1310, 474)),
                new LightSource(graphics, 200, LightAreaQuality.Low, Color.Gold, new Vector2(786, 474))}, new Vector2[] {
                new Vector2(100, 100), new Vector2( 100, 300)});

            // sett currentLevel
            spriteManager.setCurrentLevel(5);
            lightManager.lightMapTexture = spriteManager.lvlNr;
            lightManager.addLight(spriteManager.currentLevel.Lights);
            spriteManager.addZombies(spriteManager.currentLevel.Zombies);


            // Disable Components
            spriteManager.Enabled = false;
            lightManager.Enabled = false;
            collisionManager.Enabled = false;

            IsComponentsDisabled = true;

            // AddService
            Services.AddService(typeof(GraphicsDeviceManager), graphics);
            Services.AddService(typeof(SpriteManager), spriteManager);
            Services.AddService(typeof(LightManager), lightManager);
            Services.AddService(typeof(InputManager), inputManager);
            Services.AddService(typeof(CollisionManager), collisionManager);
            Services.AddService(typeof(SoundManager), soundManager);

            // draworder 
            fps.DrawOrder = 11;
            inputManager.DrawOrder = 10;
            spriteManager.DrawOrder = 8;
            lightManager.DrawOrder = 7;
            base.Initialize();

            // LYDMANAGER
            soundManager.TryLoadSound("zombie-1", false);
            soundManager.TryLoadSound("zombie-2", false);
            soundManager.TryLoadSound("zombie-3", false);
            soundManager.TryLoadSound("zombie-4", false);
            soundManager.TryLoadSound("zombie-5", false);
            soundManager.TryLoadSound("zombie-brains", false);
            soundManager.TryLoadSound("zombie-hit", false);
            soundManager.TryLoadSound("shot", false);
            soundManager.TryLoadSound("buzz", false);
            soundManager.TryLoadSound("win", false);

            soundManager.TryLoadSound("run-loop", true);

            soundManager.TryLoadSong("Cold");
            soundManager.TryLoadSong("Ambient");
            soundManager.PlaySong(0);

            // Events
            Events.MyEvent += MenuHandler;

            // ONLY REMOVE THIS IF YOU HAVE A MULTIPLE MONITOR SETUP!! ( DRAWS THE GAME ON THE SECOND MONITOR IF IN DEBUG MODE ) 
            /* #if DEBUG
            var form = (System.Windows.Forms.Form)System.Windows.Forms.Control.FromHandle(this.Window.Handle);
            form.Location = new System.Drawing.Point(1920 + 1920/2, 0);
            #endif */
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            toApplyLight = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            blood = Content.Load<Texture2D>(@"World\blood");


            // SCREENS 
            startScreen = new StartScreen(this, spriteBatch, Content.Load<SpriteFont>("menufont"), Content.Load<Texture2D>(@"Sprites/image"));
            pauseScreen = new PauseScreen(this, spriteBatch, Content.Load<SpriteFont>("menufont"), Content.Load<Texture2D>(@"Sprites/image"));
            winScreen = new WinScreen(this, spriteBatch, Content.Load<SpriteFont>("menufont"), Content.Load<Texture2D>(@"Sprites/youwin"));
            looseScreen = new LooseScreen(this, spriteBatch, Content.Load<SpriteFont>("menufont"), Content.Load<Texture2D>(@"Sprites/youloose"));


            Components.Add(startScreen);
            Components.Add(pauseScreen);
            Components.Add(winScreen);
            Components.Add(looseScreen);

            pauseScreen.DrawOrder = 10;
            startScreen.DrawOrder = 9;
            looseScreen.DrawOrder = 9;
            winScreen.DrawOrder = 9;

            pauseScreen.Hide();
            startScreen.Hide();
            winScreen.Hide();
            looseScreen.Hide();
            gameState = GameState.Menu;
            activeScreen = startScreen;
        }

        protected override void UnloadContent()
        {
        }

        // Handles the event from menu
        public void MenuHandler(Selected selected)
        {
            switch (selected)
            {
                case Selected.ExitGame:
                    this.Exit();
                    break;
                case Selected.PlayGame:
                    gameState = GameState.Playing;
                    break;
                case Selected.Continue:
                    gameState = GameState.Playing;
                    break;
                case Selected.NextSong:
                    soundManager.NextSong();
                    break;
                case Selected.PauseSong:
                    soundManager.PauseSong();
                    break;
                case Selected.StopSong:
                    soundManager.StopSong();
                    break;
                case Selected.ResumeSong:
                    soundManager.ResumeSong();
                    break;
                case Selected.NextLevel:
                    spriteManager.nextLevel();
                    lightManager.lightMapTexture = spriteManager.lvlNr;
                    lightManager.addLight(spriteManager.currentLevel.Lights);
                    gameState = GameState.Playing;
                    break;
            }
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            soundManager.setGameTime(gameTime);

            if (inputManager.isKeyPressed(Keys.P))
            {
                if (!paused)
                {
                    gameState = GameState.Pause;
                    paused = true;

                }
                else
                {
                    gameState = GameState.Playing;
                    paused = false;
                }
            }

            // see if won or lost
            if (spriteManager.win)
            {
                gameState = GameState.GameWin;
            }

            if (spriteManager.fail)
            {
                gameState = GameState.GameOver;
            }

            camera.Update(spriteManager.GetPlayerPosition(0));
            spriteManager.viewMatrix = camera.ViewMatrix;
            lightManager.viewMatrix = camera.ViewMatrix;
            inputManager.viewMatrix = camera.ViewMatrix;

            inverseMatrixPostion = Vector2.Transform(new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Matrix.Invert(camera.ViewMatrix));
            spriteManager.inverseMatrixMosue = inverseMatrixPostion;

            // GameState Logic
            switch (gameState)
            {
                case GameState.Menu:

                    lightManager.Enabled = false;
                    spriteManager.Enabled = false;
                    spriteManager.Enabled = false;
                    pauseScreen.Hide();
                    startScreen.Show();
                    winScreen.Hide();
                    looseScreen.Hide();
                    break;

                case GameState.Playing:

                    lightManager.Enabled = true;
                    lightManager.Visible = true;
                    spriteManager.Enabled = true;
                    IsComponentsDisabled = false;
                    startScreen.Hide();
                    winScreen.Hide();
                    pauseScreen.Hide();
                    spriteManager.isPaused = false;
                    //activeScreen = scree;
                    break;

                case GameState.Pause:
                    lightManager.Enabled = false;
                    spriteManager.Enabled = false;
                    spriteManager.Enabled = false;
                    pauseScreen.Show();
                    winScreen.Hide();
                    looseScreen.Hide();

                    break;

                case GameState.GameOver:
                    startScreen.Hide();
                    looseScreen.Show();
                    lightManager.Enabled = false;
                    lightManager.Visible = false;
                    spriteManager.Enabled = false;
                    spriteManager.isPaused = true;
                    activeScreen = looseScreen;
                    break;

                case GameState.GameWin:
                    startScreen.Hide();
                    winScreen.Show();
                    lightManager.Enabled = false;
                    lightManager.Visible = false;
                    spriteManager.Enabled = false;
                    spriteManager.isPaused = true;
                    activeScreen = winScreen;
                    break;


            }

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);

            //set render target to toApplyLight which will be used to blend together with the screenLight render target
            GraphicsDevice.SetRenderTarget(toApplyLight);
            GraphicsDevice.Clear(Color.Black);
            drawFloor();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, camera.ViewMatrix);
            spriteBatch.Draw(blood, Vector2.Zero, Color.White);
            spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);

            // sends the rendertarget to the light manager; 
            lightManager.setRendertarget(toApplyLight);

            base.Draw(gameTime);
        }

        public void drawFloor()
        {
            Rectangle source = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.Default, RasterizerState.CullNone, null, camera.ViewMatrix);
            spriteBatch.Draw(floorTexture, Vector2.Zero, source, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            spriteBatch.End();
        }
    }
}
