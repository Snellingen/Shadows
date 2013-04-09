using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Shadows
{
    class Projectile : Sprite
    {
        public Color color { get; set; }

        public Projectile(Texture2D textureImage, Vector2 origin, int collisionOffset, Vector2 speed,  Color color)
            : base(textureImage, origin, new Point(textureImage.Width, textureImage.Height), 1f, new Point(0, 0), new Point(0, 0), speed)
        {
            this.color = color;
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            position += aimVector() * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime, clientBounds);
        }

        public Vector2 aimVector()
        {   
            Vector2 result = position - new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            result.Normalize();
            return result; 
        }

        public override Vector2 Direction
        {
            get { return speed; }
        }        
    }
}
