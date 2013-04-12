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

    public class CollisionManager : Microsoft.Xna.Framework.GameComponent
    {
        public Rectangle clientRectangle;

        protected RenderTarget2D collisionRender;
        SpriteBatch spriteBatch;

        public CollisionManager(Game game, Rectangle clientRectangle)
            : base(game)
        {
            this.clientRectangle = clientRectangle;
        }

        public override void Initialize()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            base.Initialize();
        }


        // Checks if sprite is out of bounds, returns true if it is.  
        public bool IsOutOfBounds(Vector2 position, Point frameSize)
        {
            if (position.X < 0 - frameSize.X)
                return true;
            if (position.Y < 0 - frameSize.Y)
                return true;
            if (position.X > clientRectangle.Width + frameSize.X)
                return true;
            if (position.Y > clientRectangle.Height + frameSize.Y)
                return true;

            return false;
        }

        public bool pixelPerfectCollision(Rectangle o, Texture2D collisionMap)
        {
            // Creates a collision texture inside the collision rectange o
            Texture2D CollisionCheck = CreateCollisionTexture(o, collisionMap);

            // Use GetData to fill in an array with all the colors of the pixels in the area of the collision texture
            int pixels = o.Width * o.Height;
            Color[] myColors = new Color[pixels];
            CollisionCheck.GetData<Color>(0, new Rectangle((int)(CollisionCheck.Width / 2 - o.Width / 2),
                (int)(CollisionCheck.Height / 2 - o.Height / 2), o.Width, o.Height), myColors, 0, pixels);

            // Cycle through all the colors in the array to see if any pixels are black, if so there's a collision.
            foreach (Color color in myColors)
            {
                if (color == Color.Black)
                {
                    return true;
                }
            }
            return false;
        }

        public bool rectrangleCollision(Rectangle rec1, Rectangle rec2)
        {
            if (rec1.Intersects(rec2))
            {
                return true;
            }
            return false;
        }

        public Texture2D CreateCollisionTexture(Rectangle o, Texture2D collisionMap)
        {
            collisionRender = new RenderTarget2D(Game.GraphicsDevice, o.Width, o.Height, false, SurfaceFormat.Color, DepthFormat.None);
            Game.GraphicsDevice.SetRenderTarget(collisionRender);
            Game.GraphicsDevice.Clear(ClearOptions.Target, Color.Red, 0, 0);

            spriteBatch.Begin();
            spriteBatch.Draw(collisionMap, new Rectangle(0, 0, o.Width, o.Height), o, Color.White);
            spriteBatch.End();
            Texture2D texture = (Texture2D)collisionRender;
            Game.GraphicsDevice.SetRenderTarget(null);
            return texture;


        }

        public override void Update(GameTime gameTime)
        {


            base.Update(gameTime);
        }
    }
}
