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
    abstract class Sprite : DrawData
    {

        // For animation
        public Point frameSize { get; set; }
        protected Point currentFrame;
        public Point sheetSize;
        public Point startFrame = new Point(0, 0);
        private int timeSinceLastFrame = 0;
        private const int defaultMillisecondsPerFrame = 90;
        public int millisecondsPerFrame { get; set; }
        public Vector2 speed { get; set; }
        public bool loop = true;
        public bool stopLoop = false; 
        
        public Dictionary<string, Point[]> animationList = new Dictionary<string,Point[]>(); 

        // Constructors

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
            float collisionScale, Point currentFrame, Point sheetSize, Vector2 speed, float rotationOffset)
            : this(textureImage, position, frameSize, collisionScale, currentFrame,
            sheetSize, speed, new Vector2(textureImage.Width/2, textureImage.Height/2), 1, 0f)
        {
        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
            float collisionScale, Point currentFrame, Point sheetSize, Vector2 speed)
            : this(textureImage, position, frameSize, collisionScale, currentFrame,
            sheetSize, speed, defaultMillisecondsPerFrame, new Vector2(textureImage.Width/2, textureImage.Height/2), 1, 0)
        {
        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
            float collisionScale, Point currentFrame, Point sheetSize, Vector2 speed, Vector2 origin, float scale, float rotationOffset)
            : this(textureImage, position, frameSize, collisionScale, currentFrame,
            sheetSize, speed, defaultMillisecondsPerFrame, origin, scale, rotationOffset)
        {
        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
            float collisionScale, Point currentFrame, Point sheetSize, Vector2 speed,
            int millisecondsPerFrame, Vector2 origin, float scale, float rotationOffset)
            : base(textureImage, origin, position, scale, 0, rotationOffset)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.frameSize = frameSize;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.speed = speed;
            this.millisecondsPerFrame = millisecondsPerFrame;
            this.collisionScale = collisionScale;

            collisionRect = new Rectangle((int)position.X, (int)position.Y, (int)(frameSize.X * collisionScale), (int)(frameSize.Y * collisionScale));
        }

        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {
            
        }

        // Simple out of bound check ( not collision based )
        public bool IsOutOfBounds(Rectangle clientRectangle)
        {
            if (position.X < -frameSize.X ||
                position.X > clientRectangle.Width ||
                position.Y < -frameSize.Y ||
                position.Y > clientRectangle.Height)
            {
                return true;
            }

            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureImage, position, new Rectangle((currentFrame.X * frameSize.X) + startFrame.X, (currentFrame.Y * frameSize.Y) + startFrame.Y, frameSize.X, frameSize.Y),
            Color.White, rotation + rotationOffset, new Vector2((origin.X), (origin.Y)), scale, SpriteEffects.None, 0);
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
                    if(currentFrame.Y != sheetSize.Y)
                        ++currentFrame.Y;
                }

                if (currentFrame.Y >= sheetSize.Y)
                {
                    if (!loop)
                    {
                        stopLoop = true; 
                    }
                        currentFrame.Y = 0;
                }
            }
        }

        public void addAnimation(string name, Point startFrame, Point frameSize, Point sheetSize)
        {
            Point[] animation = new Point[3] { startFrame, frameSize, sheetSize };
            
            animationList.Add(name, animation); 
        }

        public void playAnimation(string name, bool play, GameTime gameTime)
        {
            Point[] animation; 
            animationList.TryGetValue(name, out animation);
            this.startFrame = animation[0];
            this.frameSize = animation[1];
            this.sheetSize = animation[2];

            if (play)
                Animate(gameTime); 
        }

        public abstract Vector2 Direction
        {
            get;
        }
    }
}
