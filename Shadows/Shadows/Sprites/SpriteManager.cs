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
            ResetSpawnTime();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            line = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            line.SetData(new[] { Color.White });
            player = new UserControlledSprite(Game.Content.Load<Texture2D>(@"Sprites\soldier_spritesheet"), Vector2.Zero, new Point(67, 90), 0, new Point(0, 1), new Point(8, 1), new Vector2(6, 6));
            walls = Game.Content.Load<Texture2D>(@"World\ShadowHouse");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.Milliseconds;

            // Update Player
            player.setInverseMatrixMouse(inverseMatrixMosue);
            player.Update(gameTime, Game.Window.ClientBounds);
            base.Update(gameTime);
        }

        public void UpdateSprites(GameTime gameTime)
        {
        }

        public void SpawnEnemy()
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

        public void ResetSpawnTime()
        {    
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, viewMatrix);

            // Draw player
            player.Draw(gameTime, spriteBatch);

            DrawLine(line, 1, Color.Red, player.GetPostion , 1900f);

            DrawLine(line, 1, Color.Red, player.GetPostion, 1000f);

            spriteBatch.Draw(walls, Vector2.Zero, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        void DrawLine(Texture2D blank,
              float width, Color color, Vector2 point1, float length)
        {
            float angle = player.rotation;
            spriteBatch.Draw(blank, point1, null, color,
                       angle, Vector2.Zero, new Vector2(length, width),
                       SpriteEffects.None, 0);
        }
    }
}
