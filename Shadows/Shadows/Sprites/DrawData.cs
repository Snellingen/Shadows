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
    class DrawData
    {
        // atributes 
        public Texture2D textureImage { get; set; }
        protected Vector2 position;

        public Vector2 origin = Vector2.Zero;
        public float scale = 1;
        public float rotation = 0;
        public float rotationOffset = 0;

        // The collision rectangle for the sprite. 
        public Rectangle collisionRect;
        public float collisionScale { get; set; }

        // Constructors
        public DrawData(Texture2D textureImage, Vector2 origin, Vector2 position, float scale, float rotation, float rotatiOffset)
            : this(textureImage, origin, position, scale)
        {
            this.rotation = rotation;
            this.rotationOffset = rotatiOffset;
            this.origin = origin;
        }

        public DrawData(Texture2D textureImage, Vector2 origin, Vector2 position, float scale)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.scale = scale; 
        }

        public DrawData(Texture2D textureImage, Vector2 position, float scale)
        {
            collisionRect = new Rectangle((int)position.X, (int)position.Y, (int)(textureImage.Width * collisionScale), (int)(textureImage.Height * collisionScale));
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureImage, position, collisionRect, Color.White, rotation + rotationOffset, origin, scale, SpriteEffects.None, 0);
        }

        public Vector2 GetPostion { get { return position; } }

    }
}
