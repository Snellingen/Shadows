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
    public class InputManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        
        SimpleMouse sMouse;
        
        Matrix viewMatrix; 

        public InputManager(Game game)
            : base(game)
        {
            
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            sMouse = new SimpleMouse(Game.Content.Load<Texture2D>(@"Sprites\MouseTexture"));
            base.LoadContent();
        }

        public void setViewMatrix(Matrix viewMatrix)
        {
            this.viewMatrix = viewMatrix; 
        }

        public override void Update(GameTime gameTime)
        {
            sMouse.Update();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            sMouse.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
