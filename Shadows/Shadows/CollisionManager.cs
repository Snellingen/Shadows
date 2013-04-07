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

        public CollisionManager(Game game, Rectangle clientRectangle)
            : base(game)
        {
            this.clientRectangle = clientRectangle; 
        }

        public override void Initialize()
        {
            // TODO: Add your initialization code here

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

        public override void Update(GameTime gameTime)
        {
           

            base.Update(gameTime);
        }
    }
}
