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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class CollisionManager : Microsoft.Xna.Framework.GameComponent
    {
        public Rectangle bounds; 

        public CollisionManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        // Checks if sprite is out of bounds, returns true if it is.  
        public bool IsOutOfBounds(Rectangle clientRectangle, Vector2 position, Point frameSize)
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

        public override void Update(GameTime gameTime)
        {
           

            base.Update(gameTime);
        }
    }
}
