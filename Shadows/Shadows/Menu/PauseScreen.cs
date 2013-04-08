using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Shadows
{
    class PauseScreen : GameScreen
    {
        MenuComponent menuComponent;
        Texture2D image;
        Rectangle imageRectangle;
        public int SelectedIndex
        {
            get { return menuComponent.SelectedIndex; }
            set { menuComponent.SelectedIndex = value; }
        }
        public PauseScreen(Game game,
        SpriteBatch spriteBatch,
        SpriteFont spriteFont,
        Texture2D image)
            : base(game, spriteBatch)
        {
            string[] menuItems = { "Continue", "End Game" };
            menuComponent = new MenuComponent(game, spriteBatch, spriteFont, menuItems);
            Components.Add(menuComponent);
            this.image = image;
            imageRectangle = new Rectangle(
            0,
            0,
            Game.Window.ClientBounds.Width,
            Game.Window.ClientBounds.Height);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            spriteBatch.Draw(image, imageRectangle, Color.White * 0.5f);
            spriteBatch.End(); 
            base.Draw(gameTime);
        }
    }
}