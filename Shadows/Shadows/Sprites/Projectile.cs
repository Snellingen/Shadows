using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Shadows
{
    class Projectile : Sprite
    {
        Vector2 startPosition;
        Vector2 destination;

        public Projectile(Texture2D textureImage, Vector2 startPosition, Vector2 speed, float rotation)
            : base(textureImage, startPosition, new Point(textureImage.Width, textureImage.Height), 1f, new Point(0, 0), new Point(0, 0), speed)
        {
            this.rotation = rotation;
            collisionRect.Width = 10;
            collisionRect.Height = 10;
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            position += aimVector(rotation) * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            origin = new Vector2(frameSize.X / 2, frameSize.Y);
            collisionRect.X = (int)(position.X);
            collisionRect.Y = (int)(position.Y);

            base.Update(gameTime, clientBounds);
        }

        public void spawn(Vector2 startPosition, Vector2 destination, float angle)
        {
            this.startPosition = startPosition;
            this.destination = destination;
            base.rotation = angle;

        }

        public Vector2 aimVector(float angle)
        {
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        public override Vector2 Direction
        {
            get { return speed; }
        }
    }
}
