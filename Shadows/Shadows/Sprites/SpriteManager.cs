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
        SpriteBatch spriteBatch;
        UserControlledSprite player;
        Texture2D line;
        Texture2D walls;

        CollisionManager collisionManager; 
        

        Matrix viewMatrix;
        Vector2 inverseMatrixMosue; 

        float timer;

        public SpriteManager(Game game)
            : base(game)
        {
        }

        public Vector2 GetPlayerPosition()
        {
            return player.GetPostion;
        }

        public Texture2D GetPlayerTexture()
        {
            return player.textureImage; 
        }

        public override void Initialize()
        {
            collisionManager = (CollisionManager)Game.Services.GetService(typeof(CollisionManager)); 

            ResetSpawnTime();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            line = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            line.SetData(new[] { Color.White });
            player = new UserControlledSprite(Game.Content.Load<Texture2D>(@"Sprites\soldier_spritesheet"), new Vector2(100, 100), new Point(67, 90), 0.5f, new Point(0, 1), new Point(8, 1), new Vector2(6, 6));
            // add player animation 
            player.addAnimation("walk", new Point(0,0), new Point(67, 90), new Point(8, 1));
            player.addAnimation("idle", new Point(0, 0), new Point(67, 90), new Point(1, 1)); 

            walls = Game.Content.Load<Texture2D>(@"World\ShadowHouse");
            
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.Milliseconds;

            // Update Play

            if(collisionManager.IsOutOfBounds(player.GetPostion, player.frameSize))
                player.Collision();


            if (collisionManager.pixelPerfectCollision(player.collisionRect, walls))
            {
                player.Collision();
                
            }
                

            player.setInverseMatrixMouse(inverseMatrixMosue);
            player.Update(gameTime, Game.Window.ClientBounds);
            base.Update(gameTime);
        }

        public void UpdateSprites(GameTime gameTime)
        {
        }

        public void worldCollision()
        {
            
        }


        public void SpawnEnemy()
        { 
        }

        public void ResetSpawnTime()
        {
        }

        public void setViewMatrix(Matrix viewMatrix)
        {
            this.viewMatrix = viewMatrix;
        }

        public void setInverseMatrixMosue( Vector2 pos)
        {
            this.inverseMatrixMosue = pos; 
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, viewMatrix);

            // Draw player
            player.Draw(gameTime, spriteBatch);

            spriteBatch.Draw(walls, Vector2.Zero, Color.White);

            DrawLine(line, 1, Color.Red, player.GetPostion, 1900f);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        void DrawLine(Texture2D blank,
              float width, Color color, Vector2 point1, float length)
        {
            float angle = player.rotation;
            spriteBatch.Draw(blank, point1, null, color * 0.2f,
                       angle, Vector2.Zero, new Vector2(length, width),
                       SpriteEffects.None, 0);
        }
    }
}
