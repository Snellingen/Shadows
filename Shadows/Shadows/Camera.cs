using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Shadows
{
    public class Camera
    {
        Vector2 position;
        Matrix viewMatrix;
        Vector2 screenSize;
        float zoom; 

        public Matrix ViewMatrix
        {
            get { return viewMatrix; }
        }

        public Camera(Vector2 size, float zoom)
        {
            this.screenSize = size;
            this.zoom = zoom; 
        }

        public void Update(Vector2 playerPosition)
        {
            position.X = playerPosition.X - (screenSize.X / 3);
            position.Y = playerPosition.Y - (screenSize.Y / 3);


            viewMatrix = Matrix.CreateTranslation(new Vector3(-position, 0)) * Matrix.CreateScale(new Vector3(zoom, zoom, 1));  
                ; 
        }
    }
}
