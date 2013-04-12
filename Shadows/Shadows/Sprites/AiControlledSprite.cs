using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Shadows
{
    class AiControlledSprite : Sprite
    {
        Vector2 lastDirection = Vector2.Zero;

        public bool collision = false;
        public bool isWalking = false;
        public bool wasWalking = false;
        Vector2 enemyPos { get; set; }

        // Used for gamepad to store last roatation


        public AiControlledSprite (Texture2D textureImage, Vector2 position, Point frameSize,
            float collisionScale, Point currentFrame, Point sheetSize, Vector2 speed, float rotationOffset)
            : base(textureImage, position, frameSize, collisionScale, currentFrame,
            sheetSize, speed, new Vector2(textureImage.Width/2, textureImage.Height/2), 1, 0f)
        {
        }

        // Basic input logic
        public override Vector2 Direction
        {
            get
            {
                // Stores the vector direction
                Vector2 inputDirection = Vector2.Zero;

                if (collision)
                {
                    collision = false;
                    return inputDirection;
                }

                inputDirection = aimVector(Rotation()); 

                return Vector2.Multiply(inputDirection, speedFromLoopTime(5)); // reutrn direction + speed

            }
        }

        public float Rotation()
        {
            // substracts the position of the player so that the rotation will be correct according to the player position. 
            enemyPos -= position;
            // return the rotation value based on the mouse position. 
            return (float)Math.Atan2(enemyPos.Y, enemyPos.X);
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            // Moves the sprite based on direction
            MovementUpdate(gameTime);
            collisionRect.X = (int)(position.X - (frameSize.X * collisionScale) + (origin.X * collisionScale));
            collisionRect.Y = (int)(position.Y - (frameSize.Y * collisionScale) + (origin.Y * collisionScale));
            collisionRect.Height = collisionRect.Width;

            // collision ahead

            collisionRect.X += (int)((Direction.X / (speed.X * 2)) * collisionScale);
            collisionRect.Y += (int)((Direction.Y / (speed.Y * 2)) * collisionScale);

            rotation = Rotation(); 
            //rotation = GamepadRotation();

            base.Update(gameTime, clientBounds);
            if ((Direction.X > 0) || Direction.X < 0 && !collision)
                lastDirection = Direction;
        }

        public Vector2 aimVector(float angle)
        {
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
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
                playAnimation("walk", true, gameTime);
                if (!isWalking)
                {
                    wasWalking = true;
                }
                isWalking = true;
            }

            // if standing still
            if (Direction.X == 0 &&
                Direction.Y == 0)
            {
                playAnimation("idle", true, gameTime);
                if (isWalking)
                {
                    wasWalking = false;
                }
                isWalking = false;
            }
        }

        public void Collision()
        {
            collision = true;
        }

        public float speedFromLoopTime(float speed)
        {
            return speed * 60;
        }
    }
}
