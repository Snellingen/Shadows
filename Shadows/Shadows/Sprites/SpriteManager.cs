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
    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {

        // Stuff to draw

        List<DrawData> toDraw = new List<DrawData>();
        List<UserControlledSprite> players = new List<UserControlledSprite>();
        List<AiControlledSprite> zombies = new List<AiControlledSprite>();
        List<Projectile> bullets = new List<Projectile>();
        List<DrawData> miniMapDots = new List<DrawData>();
        List<DrawData> miniMapDotsZ = new List<DrawData>();
        List<Level> levels = new List<Level>();
        public Level currentLevel;
        public bool win { get; set; }
        public bool wrong { get; set; }
        public bool fail { get; set; }

        public int lvlNr { get; set; }

        Texture2D line;
        Texture2D debug;

        // Components and such
        SpriteBatch spriteBatch;
        SoundManager sound;
        CollisionManager collisionManager;
        InputManager input;

        // Camera related
        public Matrix viewMatrix { get; set; }
        public Vector2 inverseMatrixMosue { get; set; }

        float time = 0f;
        public bool isPaused = false;

        public SpriteManager(Game game)
            : base(game)
        {
        }

        public void setCurrentLevel(int nr)
        {
            if (nr <= levels.Count)
            {
                currentLevel = levels[nr - 1];
                lvlNr = nr;
            }
        }

        public Vector2 GetPlayerPosition(int playerIndex)
        {
            return players[0].GetPostion;
        }

        // ADDING STUFF
        #region addstuff
        // lets you add elements to draw
        public void addToDraw(string textureName, Vector2 position, float scale)
        {
            toDraw.Add(new DrawData(Game.Content.Load<Texture2D>(@"Sprite\" + textureName), position, scale));
        }

        // lets you add a Matrix for drawing
        public void addToDrawNoMatrix(string textureName, Vector2 position, float scale)
        {
            miniMapDots.Add(new DrawData(Game.Content.Load<Texture2D>(@"World\" + textureName), position, scale));
        }

        // Let's you add level
        public void addLevels(string Map, string minMap, Vector2 playerSpawn, Rectangle winZone)
        {
            levels.Add(new Level(Game.Content.Load<Texture2D>(@"World\" + Map), Game.Content.Load<Texture2D>(@"World\" + minMap), playerSpawn, winZone));
        }

        // let's you add level with light
        public void addLevels(string Map, string minMap, Vector2 playerSpawn, Rectangle winZone, LightSource[] lights)
        {
            levels.Add(new Level(Game.Content.Load<Texture2D>(@"World\" + Map), Game.Content.Load<Texture2D>(@"World\" + minMap), playerSpawn, winZone, lights));
        }

        // let's you add level with light abd zombies
        public void addLevels(string Map, string minMap, Vector2 playerSpawn, Rectangle winZone, LightSource[] lights, Vector2[] zombies)
        {
            levels.Add(new Level(Game.Content.Load<Texture2D>(@"World\" + Map), Game.Content.Load<Texture2D>(@"World\" + minMap), playerSpawn, winZone, lights, zombies));
        }

        // Let's you add players
        public void addPlayers(int playerIndex, Vector2 spawn)
        {
            miniMapDots.Add(new DrawData(Game.Content.Load<Texture2D>(@"Sprites\MouseTexture"), Vector2.Multiply(spawn, 0.2f), .5f, Color.Yellow));
            players.Add(new UserControlledSprite(Game.Content.Load<Texture2D>(@"Sprites\soldier_spritesheet"), spawn, new Point(67, 90), 0.5f, new Point(0, 1), new Point(8, 1), new Vector2(6, 6), new Vector2(34, 57), 1, 89.5f));
        }

        // let's you add players
        public void addZombies(Vector2 spawn)
        {
            miniMapDotsZ.Add(new DrawData(Game.Content.Load<Texture2D>(@"Sprites\MouseTexture"), Vector2.Multiply(spawn, 0.2f), .5f, Color.Red));
            zombies.Add(new AiControlledSprite(Game.Content.Load<Texture2D>(@"Sprites\zombie_spritesheet"), spawn, new Point(67, 90), 0.5f, new Point(0, 1), new Point(8, 1), new Vector2(2, 2), new Vector2(34, 57), 1, 89.5f));
        }

        // Let's you add zombies
        public void addZombies(IEnumerable<Vector2> spawns)
        {
            foreach (Vector2 spawn in spawns)
            {
                miniMapDotsZ.Add(new DrawData(Game.Content.Load<Texture2D>(@"Sprites\MouseTexture"), Vector2.Multiply(spawn, 0.2f), .5f, Color.Red));
                zombies.Add(new AiControlledSprite(Game.Content.Load<Texture2D>(@"Sprites\zombie_spritesheet"), spawn, new Point(67, 90), 0.5f, new Point(0, 1), new Point(8, 1), new Vector2(2, 2), new Vector2(34, 57), 1, 89.5f));
            }
        }

        public void setXbox(bool on)
        {
            players[0].usingGamepad = on;
        }

        #endregion

        public override void Initialize()
        {
            collisionManager = (CollisionManager)Game.Services.GetService(typeof(CollisionManager));
            sound = (SoundManager)Game.Services.GetService(typeof(SoundManager));
            input = (InputManager)Game.Services.GetService(typeof(InputManager));
            players[0].SetPostion = currentLevel.playerSpawn;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            line = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            line.SetData(new[] { Color.White });

            // for all player in players
            for (int i = 0; i < players.Count; i++)
            {
                // add player animation 
                players[i].addAnimation("walk", new Point(0, 0), new Point(67, 90), new Point(8, 1));
                players[i].addAnimation("idle", new Point(0, 0), new Point(67, 90), new Point(1, 1));
            }

            for (int i = 0; i < zombies.Count; i++)
            {
                // add player animation 
                zombies[i].addAnimation("walk", new Point(0, 0), new Point(67, 90), new Point(8, 1));
                zombies[i].addAnimation("idle", new Point(0, 0), new Point(67, 90), new Point(1, 1));
                zombies[i].addAnimation("Death", new Point(0, 0), new Point(86, 132), new Point(5, 2));
            }


            debug = Game.Content.Load<Texture2D>(@"Sprites\green");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            PlayerUpdate(gameTime);
            ZombieUpdate(gameTime);

            // if game not paused
            if (!isPaused)
            {
                // Update bullets
                for (int i = 0; i < bullets.Count; i++)
                {
                    // update collison rectangle
                    bullets[i].Update(gameTime, collisionManager.clientRectangle);

                    // Check bullet collision wall
                    if (collisionManager.IsOutOfBounds(bullets[i].GetPostion, players[0].frameSize) ||
                        collisionManager.pixelPerfectCollision(bullets[i].collisionRect, currentLevel.map))
                    {
                        // collision! 
                        bullets.RemoveAt(i);
                    }

                    else
                    {
                        // check collision zombiess
                        for (int j = 0; j < zombies.Count; j++)
                        {
                            if (bullets.Count >= 1)
                            {
                                if (collisionManager.rectrangleCollision(bullets[i].collisionRect, zombies[j].collisionRect))
                                {
                                    bullets.RemoveAt(i);
                                    zombies[j].life -= 25;
                                    if (zombies[j].life <= 0)
                                    {
                                        if (!zombies[j].isDead)
                                        {
                                            zombies[j].isDead = true;
                                            sound.PlaySound("zombie-3");

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            base.Update(gameTime);
        }

        // Upadets for players
        public void PlayerUpdate(GameTime gameTime)
        {
            // for all player in players
            for (int i = 0; i < players.Count; i++)
            {
                if (collisionManager.rectrangleCollision(players[i].collisionRect, currentLevel.winZone))
                {
                    foreach (AiControlledSprite zombie in zombies)
                    {
                        if (!zombie.isDead)
                        {
                            if (!wrong)
                            {
                                sound.PlaySound("buzz");
                                wrong = true;
                            }
                        }
                        else
                        {
                            if (!win)
                            {
                                sound.PlaySound("win");
                                sound.StopSoundLoop("run-loop");
                                win = true;
                                wrong = false;
                            }
                        }
                    }
                }

                foreach (AiControlledSprite zombie in zombies)
                {
                    if (collisionManager.rectrangleCollision(zombie.collisionRect, players[i].collisionRect))
                    {
                        //fail = true;
                    }
                }

                // test collision
                if (collisionManager.pixelPerfectCollision(players[i].collisionRect, currentLevel.map))
                {
                    // Collision! 
                    players[i].Collision();
                }

                miniMapDots[i].SetPostion = Vector2.Multiply(players[i].GetPostion, .2f);

                // Update player
                players[i].setInverseMatrixMouse(inverseMatrixMosue);
                players[i].Update(gameTime, Game.Window.ClientBounds);

                if (players[i].life <= 0)
                {
                    players[i].isDead = true;
                }

                // Play sounds for player
                Sound();

                // Shoot bullets
                if (players[i].isShooting())
                {
                    // Pause inbetween shots
                    // get elapsed time

                    time -= gameTime.ElapsedGameTime.Milliseconds;
                    if (time <= 0)
                    {
                        // Shoot! 
                        bullets.Add(new Projectile(Game.Content.Load<Texture2D>(@"Sprites\projectile"), players[i].GetPostion, new Vector2(1000, 1000), players[i].rotation));
                        time = 100f;
                    }
                }
            }
        }

        // Update for zombies
        public void ZombieUpdate(GameTime gameTime)
        {
            // for all player in players
            for (int i = 0; i < zombies.Count; i++)
            {
                if (players[0].circlesColliding((int)zombies[i].GetPostion.X, (int)zombies[i].GetPostion.Y, 50, (int)players[0].GetPostion.X, (int)players[0].GetPostion.Y, 200) ||
                    !zombies[i].active)
                {
                    // test collision
                    if (collisionManager.pixelPerfectCollision(zombies[i].collisionRect, currentLevel.map))
                    {
                        // Collision! 
                        zombies[i].Collision();
                    }

                    if (zombies[i].isDead)
                    {
                        if (!zombies[i].deathLoaded)
                        {
                            zombies[i].textureImage = Game.Content.Load<Texture2D>(@"Sprites/zombie_death");
                            zombies[i].millisecondsPerFrame = 200;
                            zombies[i].deathLoaded = true;
                            zombies[i].active = false;
                        }
                    }

                    miniMapDotsZ[i].SetPostion = Vector2.Multiply(zombies[i].GetPostion, .2f);

                    // Update player
                    zombies[i].Update(gameTime, Game.Window.ClientBounds);
                    zombies[i].enemyPos = players[0].GetPostion;
                }

                // Play sounds for zombie
                //PlayerSound();
            }
        }

        public void Sound()
        {
            // for all player in players
            for (int i = 0; i < players.Count; i++)
            {
                if (!players[i].wasWalking)
                {
                    sound.StopSoundLoop("run-loop");
                }

                if (players[i].wasWalking)
                {
                    sound.PlaySoundLoop("run-loop");
                }

                if (players[i].isShooting())
                {
                    sound.PlaySoundContinuously("shot", 100f);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (!isPaused)
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, viewMatrix);

                // draw WineBox
                spriteBatch.Draw(debug, currentLevel.winZone, Color.White * .2f);

                // Draw player
                // for all player in players
                for (int i = 0; i < players.Count; i++)
                {
                    players[i].Draw(spriteBatch);
                    DrawLine(line, 1, Color.Red, players[i].GetPostion, 1900f, i);
                }

                // Draw projectiles
                foreach (Projectile bullet in bullets)
                {
                    bullet.Draw(spriteBatch);
                }

                for (int i = 0; i < zombies.Count; i++)
                {
                    zombies[i].Draw(spriteBatch);
                }


                spriteBatch.Draw(currentLevel.map, Vector2.Zero, Color.White);

                /*foreach (Projectile bullet in bullets)
               {
                   spriteBatch.Draw(block, bullet.collisionRect, Color.White * 0.5f);
               }*/

                spriteBatch.End();
                // NoMatrix
                spriteBatch.Begin();

                spriteBatch.Draw(currentLevel.miniMap, Vector2.Zero, Color.White);

                foreach (DrawData dot in miniMapDots)
                {
                    dot.Draw(spriteBatch);
                }

                foreach (DrawData dot in miniMapDotsZ)
                {
                    dot.Draw(spriteBatch);
                }

                spriteBatch.End();
                base.Draw(gameTime);
            }

        }

        void DrawLine(Texture2D blank,
              float width, Color color, Vector2 point1, float length, int playerIndex)
        {
            float angle = players[playerIndex].rotation;
            spriteBatch.Draw(blank, point1, null, color * 0.2f,
                       angle, Vector2.Zero, new Vector2(length, width),
                       SpriteEffects.None, 0);
        }
    }
}
