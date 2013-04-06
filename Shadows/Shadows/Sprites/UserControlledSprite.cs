using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Shadows
{

    class UserControlledSprite : Sprite
    {
        Vector2 lastDirection = Vector2.Zero;
        public Keys keyRight = Keys.D;
        public Keys keyLeft = Keys.A;
        public Keys keyUp = Keys.W;
        public Keys keyDown = Keys.S;
        public Keys keyRoll = Keys.LeftControl;
        public Keys keyJump = Keys.Space;

        Vector2 inverseMatrixMouse; 

        // Used for gamepad to store last roatation
        float oldroation = 0;  

        public UserControlledSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed)
            : base (textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed)
        {
        }

        public UserControlledSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, millisecondsPerFrame)
        {
        }

        public void setInverseMatrixMouse(Vector2 pos)
        {
            this.inverseMatrixMouse = pos; 
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            // Moves the sprite based on direction
            MovementUpdate(gameTime);
            rotation = MouseRotation();
            //rotation = GamepadRotation();
            

            // If sprite is of the screen, move it back within the game window
            if (position.X < 0 + frameSize.X)
                position.X = 0 + frameSize.X;
            if (position.Y < 0 + frameSize.Y)
                position.Y = 0 + frameSize.Y;
            if (position.X > clientBounds.Width - frameSize.X)
                position.X = clientBounds.Width - frameSize.X;
            if (position.Y > clientBounds.Height - frameSize.Y)
                position.Y = clientBounds.Height - frameSize.Y;

            base.Update(gameTime, clientBounds);
            if ((Direction.X > 0) || Direction.X < 0)
                lastDirection = Direction;
        }

        // Basic input logic
        public override Vector2 Direction
        {
            get 
            {
                // Stores the vector direction
                Vector2 inputDirection = Vector2.Zero;

                if (Keyboard.GetState().IsKeyDown(keyLeft))
                    inputDirection.X -= 1;
                if (Keyboard.GetState().IsKeyDown(keyRight))
                    inputDirection.X += 1;
                if (Keyboard.GetState().IsKeyDown(keyUp))
                    inputDirection.Y -= 1;
                if (Keyboard.GetState().IsKeyDown(keyDown))
                    inputDirection.Y += 1;

                GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
                if (gamepadState.ThumbSticks.Left.X != 0)
                    inputDirection.X += gamepadState.ThumbSticks.Left.X;
                if (gamepadState.ThumbSticks.Left.Y != 0)
                    inputDirection.Y -= gamepadState.ThumbSticks.Left.Y;

                return Vector2.Multiply(inputDirection, speedFromLoopTime(5)); // reutrn direction + speed

            }
        }

        public float MouseRotation()
        {
            Vector2 pos = inverseMatrixMouse;
            
            // substracts the position of the player so that the rotation will be correct according to the player position. 
            pos -= position;
            // return the rotation value based on the mouse position. 
            return (float)Math.Atan2(pos.Y, pos.X); 
        }
        
        public float GamepadRotation()
        {;
            // Doing basically the same thing as for MouseRotation() but now with gamepad. We also need to store the rotation from last update
            // so that it does not reset when you let go of the stick. also using GamePadDeadZone.Circular so you get a smoother rotation without
            // it sticking to 0, 90, 180, 270 degrees. 
            GamePadState gamepadState = GamePad.GetState( PlayerIndex.One, GamePadDeadZone.Circular);
            if (gamepadState.ThumbSticks.Right.X >= .10 || gamepadState.ThumbSticks.Right.X <= -.10 ||
                 gamepadState.ThumbSticks.Right.Y >= .10 || gamepadState.ThumbSticks.Right.Y <= -.10)
            {
                oldroation = (float)Math.Atan2(gamepadState.ThumbSticks.Right.X, gamepadState.ThumbSticks.Right.Y) -89.5f;
                return oldroation;
            }

            return oldroation;
              
        }

        // Animation logic
        public void MovementUpdate(GameTime gameTime)
        {
            // Update position
            position += Vector2.Multiply(Direction, (float)gameTime.ElapsedGameTime.TotalSeconds);

            // if movement
            if (Direction.X > 1 ||
                Direction.X < -1 ||
                Direction.Y > 1 ||
                Direction.Y < -1)
            {
                walk();
                Animate(gameTime);
            }
            
            // if standing still
            if (Direction.X == 0 &&
                Direction.Y == 0 )
            {
                idle();
                Animate(gameTime);
            }
        }

        public float speedFromLoopTime(float speed)
        {
            return speed * 60; 
        }

        // Aniamtions 
        // Going to replace these with dictonaries, they store the animation values. 
        public void idle()
        {
            startFrame = new Point(0, 0);
            frameSize = new Point(67, 90);
            sheetSize = new Point(1, 1);
        }

        public void walk()
        {
            startFrame = new Point(0, 0);
            frameSize = new Point(67, 90);
            sheetSize = new Point(8, 1);
        }

        // Set input
        public void setKeys(Keys right, Keys left, Keys up, Keys down, Keys roll, Keys jump)
        {
            keyRight = right;
            keyLeft = left;
            keyUp = up;
            keyDown = down;
            keyRoll = roll;
            keyJump = jump;
        }
    }     
}
