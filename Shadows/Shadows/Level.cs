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
    class Level
    {
        Texture2D map { get; protected set; }
        Texture2D miniMap { get; protected set; }
        Vector2 playerSpawn { get; protected set; }
        List<Vector2> zombieSpawns = new List<Vector2>();


        public Level (Texture2D map, Texture2D miniMap, Vector2 playerSpawn){
            this.map = map;
            this.miniMap = miniMap;
            this.playerSpawn = playerSpawn;
        }

        public void addZombie(Vector2[] array)
        {
            foreach (Vector2 pos in array)
            {
                zombieSpawns.Add(pos);
            }
        }
    
     }
    


}
