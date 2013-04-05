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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        FpsViewer fps;
        SpriteManager spriteManager;
        LightManager lightManager; 
        InputManager inputManager;

        Texture2D floorTexture; 

        RenderTarget2D toApplyLight;

        int screenWidth = 1440;
        int screenHeight = 900;
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.IsFullScreen = false;

            //AA 
            graphics.PreferMultiSampling = true;
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

            // Add Component
            Components.Add(fps);
            Components.Add(spriteManager);
            Components.Add(inputManager);
            Components.Add(lightManager);

            // AddService
            Services.AddService(typeof(GraphicsDeviceManager), graphics);
            Services.AddService(typeof(SpriteManager), spriteManager);
            Services.AddService(typeof(LightManager), lightManager);
            Services.AddService(typeof(InputManager), inputManager); 

            // draworder 
            fps.DrawOrder = 10;
            spriteManager.DrawOrder = 9;
            inputManager.DrawOrder = 8;
            lightManager.DrawOrder = 7; 
            base.Initialize();
          

            

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
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
           
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

    

            //set render target to toApplyLight which will be used to blend together with the screenLight render target
            GraphicsDevice.SetRenderTarget(toApplyLight);
            GraphicsDevice.Clear(Color.Black);
            drawFloor();
            GraphicsDevice.SetRenderTarget(null); 
            // sends the rendertarget to the light manager; 
            lightManager.setRendertarget(toApplyLight); 

            base.Draw(gameTime);
        }

        public void drawFloor()
        {
            Rectangle source = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.LinearWrap, DepthStencilState.Default, RasterizerState.CullNone);
            spriteBatch.Draw(floorTexture, Vector2.Zero, source, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            spriteBatch.End();
        }
    }
}
