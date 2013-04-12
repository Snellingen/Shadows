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
        public bool isDead = false;
        public bool wasWalking = false;
        public bool deathLoaded = false;

        public Point deathAnimation { get; set; }

        public Vector2 enemyPos { get; set; }
        Vector2 oldDriection;
        public int life = 100;

        // Used for gamepad to store last roatation


        public AiControlledSprite(Texture2D textureImage, Vector2 position, Point frameSize, float collisionScale, Point currentFrame, Point sheetSize, Vector2 speed, Vector2 origin, float scale, float rotationOffset)
            : base(textureImage, position, frameSize, collisionScale, currentFrame, sheetSize, speed, origin, scale, rotationOffset)
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
                    return Vector2.Negate(oldDriection);
                }
                inputDirection = aimVector(Rotation());

                return Vector2.Multiply(inputDirection, speedFromLoopTime(1)); // reutrn direction + speed

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
            if (active)
            {

                collisionRect.X = (int)(position.X - (frameSize.X * collisionScale) + (origin.X * collisionScale));
                collisionRect.Y = (int)(position.Y - (frameSize.Y * collisionScale) + (origin.Y * collisionScale));
                collisionRect.Height = collisionRect.Width;

                if ((Direction.X > 0) || Direction.X < 0 && !collision)
                    lastDirection = Direction;

                oldDriection = Direction;

                rotation = Rotation();
            }

            base.Update(gameTime, clientBounds);
        }

        public Vector2 aimVector(float angle)
        {
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        // Animation logic
        public void MovementUpdate(GameTime gameTime)
        {
            // Update position
            if (active)
                position += Vector2.Multiply(Direction, (float)gameTime.ElapsedGameTime.TotalSeconds);


            if (isDead)
            {
                playAnimation("Death", true, gameTime);
            }

            else
            {

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
