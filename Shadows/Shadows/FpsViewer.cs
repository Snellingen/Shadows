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
    public class FpsViewer :  Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        SpriteFont _spr_font;
        int _total_frames = 0;
        float _elapsed_time = 0.0f;
        int _fps = 0;
        Vector2 mousepos; 

        public FpsViewer(Game game)
            : base(game)
        {
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            // Put the name of the font
            _spr_font = Game.Content.Load<SpriteFont>("SpriteFont1");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            // Update
            _elapsed_time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            mousepos.X = Mouse.GetState().X;
            mousepos.Y = Mouse.GetState().Y;
            // 1 Second has passed
            if (_elapsed_time > 1000.0f)
            {
                _fps = _total_frames;
                _total_frames = 0;
                _elapsed_time = 0;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            // Only update total frames when drawing
            _total_frames++;
            spriteBatch.Begin();
            spriteBatch.DrawString(_spr_font, string.Format("FPS={0} Pos= X {1} Y {2}", _fps, mousepos.X, mousepos.Y), new Vector2(Game.GraphicsDevice.Viewport.Width - 300, 20.0f), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
