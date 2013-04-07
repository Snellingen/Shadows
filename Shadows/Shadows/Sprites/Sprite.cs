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
    abstract class Sprite
    {
        // atributes 
        public Texture2D textureImage { get; set; }

        public Point frameSize { get; set; }
        protected Point currentFrame;
        public Point sheetSize { get; set; }
        public Point startFrame = new Point(0, 0);

        private int timeSinceLastFrame = 0;
        private const int defaultMillisecondsPerFrame = 90;
        public int millisecondsPerFrame { get; set; }
        public int collisionOffset { get; set; }
        public Vector2 speed { get; set; }
        protected Vector2 position;
        public Vector2 origin = new Vector2(34, 57);
        public float scale = 1;
        public float rotation = 0;


        // The collision rectangle for the sprite. 
        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                    (int)position.X + collisionOffset,
                    (int)position.Y + collisionOffset,
                    (frameSize.X - (collisionOffset * 2)) * (int)scale,
                    (frameSize.Y - (collisionOffset * 2)) * (int)scale);
            }
        }

        // Constructors
        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed)
            : this(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, defaultMillisecondsPerFrame)
        {
        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
         int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, float scale)
            : this(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, defaultMillisecondsPerFrame)
        {
            this.scale = scale;
        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,
            int millisecondsPerFrame)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.frameSize = frameSize;
            this.collisionOffset = collisionOffset;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.speed = speed;
            this.millisecondsPerFrame = millisecondsPerFrame;
        }


        public Vector2 GetPostion { get { return position; } }

        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {

        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureImage, position, new Rectangle((currentFrame.X * frameSize.X) + startFrame.X, (currentFrame.Y * frameSize.Y) + startFrame.Y, frameSize.X, frameSize.Y),
            Color.White, rotation + 89.5f, new Vector2((origin.X), (origin.Y)), scale, SpriteEffects.None, 0);
        }

        // Handles the anmiation logic by going through the spritesheet
        public void Animate(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame = 0;
                ++currentFrame.X;
                if (currentFrame.X >= sheetSize.X)
                {
                    currentFrame.X = 0;
                    ++currentFrame.Y;
                }
                if (currentFrame.Y >= sheetSize.Y)
                {
                    currentFrame.Y = 0;
                }
            }
        }

        public abstract Vector2 Direction
        {
            get;
        }
    }
}
