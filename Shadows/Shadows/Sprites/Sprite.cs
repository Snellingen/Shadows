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
        public float collisionScale { get; set; }
        public Vector2 speed { get; set; }
        protected Vector2 position;
        public Vector2 origin = new Vector2(34, 57);
        public float scale = 1;
        public float rotation = 0;

        public Dictionary<string, Point[]> animationList = new Dictionary<string,Point[]>(); 


        // The collision rectangle for the sprite. 
        public Rectangle collisionRect;
        

        // Constructors
        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
            float collisionScale, Point currentFrame, Point sheetSize, Vector2 speed)
            : this(textureImage, position, frameSize, collisionScale, currentFrame,
            sheetSize, speed, defaultMillisecondsPerFrame)
        {
        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
            float collisionScale, Point currentFrame, Point sheetSize, Vector2 speed, float scale)
            : this(textureImage, position, frameSize, collisionScale, currentFrame,
            sheetSize, speed, defaultMillisecondsPerFrame)
        {
            this.scale = scale;
        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
            float collisionScale, Point currentFrame, Point sheetSize, Vector2 speed,
            int millisecondsPerFrame)
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
