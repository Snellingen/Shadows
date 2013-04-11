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
    class Keybind
    {
        public Keys up {get; protected set;}
        public Keys down { get; protected set; }
        public Keys left { get; protected set; }
        public Keys right { get; protected set; }
        public Keys granade { get; protected set; }

        public Keybind(Keys up, Keys down, Keys left, Keys right, Keys granade)
        {
            this.up = up;
            this.down = down;
            this.left = left;
            this.right = right;
            this.granade = granade;
        }
    }
}
