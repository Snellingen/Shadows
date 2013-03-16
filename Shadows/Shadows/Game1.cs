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

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            // Unlimited FPS :) 
            /*this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 100.0f);
            this.IsFixedTimeStep = false;
            graphics.SynchronizeWithVerticalRetrace = false;*/
            
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            fps = new FpsViewer(this);
            Components.Add(fps);
            spritemanager = new SpriteManager(this);
            Components.Add(spritemanager);
            base.Initialize();

            // ONLY REMOVE THIS IF YOU HAVE A MULTIPLE MONITOR SETUP!! ( DRAWS THE GAME ON THE SECOND MONITOR IF IN DEBUG MODE ) 
/* #if DEBUG
            var form = (System.Windows.Forms.Form)System.Windows.Forms.Control.FromHandle(this.Window.Handle);
            form.Location = new System.Drawing.Point(1920 + 1920/2, 0);
#endif */
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
