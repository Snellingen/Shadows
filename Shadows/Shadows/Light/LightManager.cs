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
        public Matrix viewMatrix { get; set; }

        //LIGHT 
        Texture2D shadowHouseTexture;
        Vector2 lightPosition;

        List<LightSource> lights = new List<LightSource>();
        List<Vector2> lightPositions = new List<Vector2>();
        LightSource playerLight;

        public int lightMapTexture;

        ShadowMapResolver shadowmapResolver; // Processes the lightmap with the lights
        ShadowCasterMap shadowMap; // for shadowmap 
        LightsFX lightsFX; // For different light effects

        RenderTarget2D screenLights;
        Texture2D toApplyLigth;

        public LightManager(Game game, GraphicsDeviceManager graphics)
            : base(game)
        {
            this.graphics = graphics;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            shadowHouseTexture = Game.Content.Load<Texture2D>(@"World\ShadowHouse" + lightMapTexture);
            spriteManager = (SpriteManager)Game.Services.GetService(typeof(SpriteManager));

            // Lights: 
            playerLight = new LightSource(graphics, 300, LightAreaQuality.VeryLow, Color.Wheat);

            // Load lightFx with effects
            lightsFX = new LightsFX(
               Game.Content.Load<Effect>("resolveShadowsEffect"),
               Game.Content.Load<Effect>("reductionEffect"),
               Game.Content.Load<Effect>("2xMultiBlend"));
            shadowmapResolver = new ShadowMapResolver(GraphicsDevice, this.lightsFX, 200);

            shadowMap = new ShadowCasterMap(PrecisionSettings.VeryHigh, graphics, this.spriteBatch);
            lightPosition = spriteManager.GetPlayerPosition(1); // light positon = player positon 
            screenLights = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            //screenGround = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            // Generating shadow map, only added the wall texture. 
            shadowMap.StartGeneratingShadowCasteMap(false);
            shadowMap.AddShadowCaster(shadowHouseTexture, Vector2.Zero, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            shadowMap.EndGeneratingShadowCasterMap();

            base.LoadContent();
        }

        public void addLight(int radius, LightAreaQuality quality, Color color, Vector2 position)
        {
            lights.Add(new LightSource(graphics, radius, quality, color, position));
        }

        public void addLight(IEnumerable<LightSource> light)
        {
            lights.AddRange(light);
        }

        public override void Update(GameTime gameTime)
        {
            lightPosition = spriteManager.GetPlayerPosition(1);

            base.Update(gameTime);
        }

        public void setRendertarget(Texture2D rt)
        {
            toApplyLigth = rt;
        }

        public override void Draw(GameTime gameTime)
        {
            // Process the light mape with the shadowmap, light, effect and position ( saves to lightsource.printedlight) 
            foreach (LightSource light in lights)
            {
                shadowmapResolver.ResolveShadows(shadowMap, light, PostEffect.LinearAttenuation_BlurHigh, light.DrawPosition);
            }
            shadowmapResolver.ResolveShadows(shadowMap, playerLight, PostEffect.CurveAttenuation_BlurHigh, lightPosition);

            //shadowmapResolver.ResolveShadows(shadowMap, light4, PostEffect.LinearAttenuation_BlurHigh, Vector2.Zero);
            // Draw lightmap to rendertarget screeLight
            GraphicsDevice.SetRenderTarget(screenLights);
            {
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, viewMatrix);
                {
                    foreach (LightSource light in lights)
                    {
                        light.Draw(spriteBatch);
                    }
                    playerLight.Draw(spriteBatch, 90);
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
    }
}
