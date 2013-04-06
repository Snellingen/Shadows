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
    class SimpleMouse
    {
        // handles the previous state of the mouse and the current
        MouseState currentState, previousState;
        public Vector2 Position { get; protected set; }
        Texture2D texture;
        public Rectangle Rectange { get; protected set; }

        // Left button pressed? 
        public bool leftClick
        {
            get { return currentState.LeftButton == ButtonState.Pressed; }
        }

        // Left button pressed but now last frame? 
        public bool NewLeftClick
        {
            get
            {
                return currentState.LeftButton == ButtonState.Pressed &&
                    previousState.LeftButton == ButtonState.Released;
            }
        }

        // Is the left button released? 
        public bool ReleaseLeft
        {
            get { return !leftClick && previousState.LeftButton == ButtonState.Pressed; }
        }

        // Right button pressed? 
        public bool rightClick
        {
            get { return currentState.RightButton == ButtonState.Pressed; }
        }

        // Right button pressed but now last frame? 
        public bool NewRightClick
        {
            get
            {
                return currentState.RightButton == ButtonState.Pressed &&
                    previousState.RightButton == ButtonState.Released;
            }
        }

        // Is the Right button released? 
        public bool ReleaseRight
        {
            get { return !rightClick && previousState.RightButton == ButtonState.Pressed; }
        }

        // Construction (gets texture for mouse) 
        public SimpleMouse(Texture2D texture)
        {
            this.texture = texture;
        }

        public void Update()
        {
            // set both sates
            previousState = currentState;
            currentState = Mouse.GetState();

            // store position
            Position = new Vector2(currentState.X, currentState.Y);

            // create rectangele for the mouse. 
            Rectange = new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height); 
        }

       


        public void Draw(SpriteBatch spriteBatch)
        {
            // only draw if texture is not empty.
            if (texture != null)
            {
                spriteBatch.Draw(texture, Position, new Rectangle(0, 0, texture.Width, texture.Height), Color.White, 0, new Vector2(texture.Width/2, texture.Height/2),1f ,SpriteEffects.None, 0f);
            }
        }
    }
}
