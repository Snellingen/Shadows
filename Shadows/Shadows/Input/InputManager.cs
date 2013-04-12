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

        //Mouse
        MouseState currentState, previousState;
        public Vector2 Position { get; protected set; }
        Texture2D pointerTexture;
        public Rectangle Rectange { get; protected set; }

        // Keyboard
        KeyboardState oldKeyboardState;

        public Matrix viewMatrix { get; set; }

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
            pointerTexture = Game.Content.Load<Texture2D>(@"Sprites\MouseTexture");
            base.LoadContent();
        }

        // Mouse events
        # region MouseEvents
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
        #endregion
        // Keyboard events
        #region KeyboardEvents

        public bool isKeyPressed(Keys key)
        {
            if (Keyboard.GetState().IsKeyUp(key) && oldKeyboardState.IsKeyDown(key))
            {
                return true;
            }

            return false;
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            // Mouse
            // set both sates
            previousState = currentState;
            currentState = Mouse.GetState();

            // store position
            Position = new Vector2(currentState.X, currentState.Y);

            // create rectangele for the mouse. 
            Rectange = new Rectangle((int)Position.X, (int)Position.Y, pointerTexture.Width, pointerTexture.Height);

            base.Update(gameTime);
            oldKeyboardState = Keyboard.GetState();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            //mouse
            if (pointerTexture != null)
            {
                spriteBatch.Draw(pointerTexture, Position, new Rectangle(0, 0, pointerTexture.Width, pointerTexture.Height), Color.White, 0, new Vector2(pointerTexture.Width / 2, pointerTexture.Height / 2), 1f, SpriteEffects.None, 0f);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
