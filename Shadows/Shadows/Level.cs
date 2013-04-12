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
    public class Level
    {
        public Texture2D map { get; protected set; }
        public Texture2D miniMap { get; protected set; }
        public Vector2 playerSpawn { get; protected set; }
        public List<Vector2> zombieSpawns = new List<Vector2>();
        public Rectangle winZone { get; protected set; }
        public List<LightSource> Lights { get; protected set; }

        public Level(Texture2D map, Texture2D miniMap, Vector2 playerSpawn, Rectangle winZone, LightSource[] aLights)
            :this(map, miniMap, playerSpawn, winZone)
        {
            Lights = new List<LightSource>(aLights);
        }

        public Level (Texture2D map, Texture2D miniMap, Vector2 playerSpawn, Rectangle winZone){
            this.map = map;
            this.miniMap = miniMap;
            this.playerSpawn = playerSpawn;
            this.winZone = winZone;
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
