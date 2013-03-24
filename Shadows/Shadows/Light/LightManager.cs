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
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class LightManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteManager spriteManager;
        

        //LIGHT 
        Texture2D shadowHouseTexture; 
        Vector2 lightPosition;
        LightSource light;
        LightSource light2;
        LightSource light3;
        LightSource light4;
        ShadowMapResolver shadowmapResolver; // Processes the lightmap with the lights
        ShadowCasterMap shadowMap; // for shadowmap 
        LightsFX lightsFX; // For different light effects

        RenderTarget2D screenLights;
        RenderTarget2D toApplyLigth;

        public LightManager(Game game, GraphicsDeviceManager graphics)
            : base(game)
        {
            this.graphics = graphics; 
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            shadowHouseTexture = Game.Content.Load<Texture2D>(@"World\ShadowHouse");
            spriteManager = (SpriteManager)Game.Services.GetService(typeof(SpriteManager));

            // Lights: 
            // Load lightFx with effects
            lightsFX = new LightsFX(
               Game.Content.Load<Effect>("resolveShadowsEffect"),
               Game.Content.Load<Effect>("reductionEffect"),
               Game.Content.Load<Effect>("2xMultiBlend"));
            shadowmapResolver = new ShadowMapResolver(GraphicsDevice, this.lightsFX, 800);
            light = new LightSource(graphics, 600, LightAreaQuality.VeryHigh, Color.White);
            light2 = new LightSource(graphics, 800, LightAreaQuality.VeryHigh, Color.Red);
            light3 = new LightSource(graphics, 800, LightAreaQuality.VeryHigh, Color.Orange);
            light4 = new LightSource(graphics, 800, LightAreaQuality.VeryHigh, Color.Red);
            shadowMap = new ShadowCasterMap(PrecisionSettings.VeryHigh, graphics, this.spriteBatch);
            lightPosition = spriteManager.GetPlayerPosition(); // light positon = player positon 
            screenLights = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            //screenGround = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            // Generating shadow map, only added the wall texture. 
            shadowMap.StartGeneratingShadowCasteMap(false);
            shadowMap.AddShadowCaster(shadowHouseTexture, Vector2.Zero, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            shadowMap.EndGeneratingShadowCasterMap();

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            lightPosition = spriteManager.GetPlayerPosition();

            base.Update(gameTime);
        }

        public void setRendertarget(RenderTarget2D rt){
            toApplyLigth = rt; 
        }

        public override void Draw(GameTime gameTime)
        {
            // Process the light mape with the shadowmap, light, effect and position ( saves to lightsource.printedlight) 
            shadowmapResolver.ResolveShadows(shadowMap, light, PostEffect.CurveAttenuation_BlurHigh, lightPosition);
            shadowmapResolver.ResolveShadows(shadowMap, light2, PostEffect.LinearAttenuation_BlurHigh, Vector2.Zero);
            shadowmapResolver.ResolveShadows(shadowMap, light3, PostEffect.LinearAttenuation_BlurHigh, new Vector2(700, 783));
           
            //shadowmapResolver.ResolveShadows(shadowMap, light4, PostEffect.LinearAttenuation_BlurHigh, Vector2.Zero);
            // Draw lightmap to rendertarget screeLight
            GraphicsDevice.SetRenderTarget(screenLights);
            {
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
                {
                    light.Draw(spriteBatch, 90);
                    light2.Draw(spriteBatch);
                    light3.Draw(spriteBatch);
                }
                spriteBatch.End();
            }

            /* Draw ground to rendertarger ground
            GraphicsDevice.SetRenderTarget(screenGround);
            GraphicsDevice.Clear(Color.Black);
            DrawGround();*/

            // Combine light and ground render target and blend them.
            this.lightsFX.PrintLightsOverTexture(null, spriteBatch, graphics, screenLights, toApplyLigth, 0.90f);

            base.Draw(gameTime);
        }

        public void DrawGround()
        {
        }
    }
}
