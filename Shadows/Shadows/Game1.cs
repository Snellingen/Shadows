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
        SpriteManager spritemanager;
        InputManager inputManager;
        Texture2D tileTexture;
        Texture2D shadowHouseTexture; 

        //LIGHT 
        Vector2 lightPosition;
        LightSource light;
        ShadowMapResolver shadowmapResolver; // Processes the lightmap with the lights
        ShadowCasterMap shadowMap; // for shadowmap 
        LightsFX lightsFX; // For different light effects

        RenderTarget2D screenLights; 
        RenderTarget2D screenGround; 

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
            // TODO: Add your initialization logic here
            fps = new FpsViewer(this);
            Components.Add(fps);
            spritemanager = new SpriteManager(this);
            Components.Add(spritemanager);
            inputManager = new InputManager(this);
            Components.Add(inputManager);
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
            tileTexture = Content.Load<Texture2D>(@"World\tile");
            shadowHouseTexture = Content.Load<Texture2D>(@"World\ShadowHouse");

            // Lights: 
            // Load lightFx with effects
            lightsFX = new LightsFX(
               Content.Load<Effect>("resolveShadowsEffect"),
               Content.Load<Effect>("reductionEffect"),
               Content.Load<Effect>("2xMultiBlend"));
            shadowmapResolver = new ShadowMapResolver(GraphicsDevice, this.lightsFX, 400);
            light = new LightSource(graphics, 400, LightAreaQuality.VeryHigh, Color.White);
            shadowMap = new ShadowCasterMap(PrecisionSettings.VeryHigh, graphics, this.spriteBatch);
            lightPosition = spritemanager.GetPlayerPosition(); // light positon = player positon 
            screenLights = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            screenGround = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            // Generating shadow map, only added the wall texture. 
            shadowMap.StartGeneratingShadowCasteMap(false);
            shadowMap.AddShadowCaster(shadowHouseTexture, Vector2.Zero, screenWidth, screenHeight);
            shadowMap.EndGeneratingShadowCasterMap();
            
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            lightPosition = spritemanager.GetPlayerPosition(); 

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Process the light mape with the shadowmap, light, effect and position ( saves to lightsource.printedlight) 
            shadowmapResolver.ResolveShadows(shadowMap, light, PostEffect.CurveAttenuation_BlurHigh, lightPosition);

            // Draw lightmap to rendertarget screeLight
            GraphicsDevice.SetRenderTarget(screenLights);
            {
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
                {
                    light.Draw(spriteBatch);
                }
                spriteBatch.End();
            }

            // Draw ground to rendertarger ground
            GraphicsDevice.SetRenderTarget(screenGround);
            GraphicsDevice.Clear(Color.Black);
            DrawGround();
            DrawOther();

            // Combine light and ground render target and blend them.
            this.lightsFX.PrintLightsOverTexture(null, spriteBatch, graphics, screenLights, screenGround, 0.90f);

            base.Draw(gameTime);
        }

        public void DrawOther()
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone); ;
            spriteBatch.Draw(shadowHouseTexture, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
            spriteBatch.End(); 
        }

        public void DrawGround()
        {
            Rectangle source = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.LinearWrap, DepthStencilState.Default, RasterizerState.CullNone);
            spriteBatch.Draw(tileTexture, Vector2.Zero, source, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            spriteBatch.End(); 
        }
    }
}
