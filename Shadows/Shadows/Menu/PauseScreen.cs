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
    class PauseScreen : GameScreen
    {
        MenuComponent menuComponent;
        Texture2D image;
        Rectangle imageRectangle;
        InputManager input;
        KeyboardState oldState;
        SpriteFont spriteFont;
        bool songPaused = false;


        public int SelectedIndex
        {
            get { return menuComponent.SelectedIndex; }
            set { menuComponent.SelectedIndex = value; }
        }
        public PauseScreen(Game game, SpriteBatch spriteBatch, SpriteFont spriteFont, Texture2D image)
            : base(game, spriteBatch)
        {
            string[] menuItems = { "Continue", "End Game", "Next Song", "Pause Song", "Stop Song" };
            menuComponent = new MenuComponent(game, spriteBatch, spriteFont, menuItems);
            Components.Add(menuComponent);

            this.spriteFont = spriteFont; 

            this.image = image;
            imageRectangle = new Rectangle(
            0,
            0,
            Game.Window.ClientBounds.Width,
            Game.Window.ClientBounds.Height);
        }
        public override void Update(GameTime gameTime)
        {
            input = (InputManager)Game.Services.GetService(typeof(InputManager));
            if (Keyboard.GetState().IsKeyUp(Keys.Enter) && oldState.IsKeyDown(Keys.Enter))
            {
                if (menuComponent.SelectedIndex == 0)
                {
                    Events.FireMyEvent(Selected.Continue);
                }

                else if (menuComponent.SelectedIndex == 1)
                {
                    Events.FireMyEvent(Selected.ExitGame);
                }
                else if (menuComponent.SelectedIndex == 2)
                {
                    Events.FireMyEvent(Selected.NextSong);
                }

                else if (menuComponent.SelectedIndex == 3)
                {
                    if (!songPaused)
                    {
                        Events.FireMyEvent(Selected.PauseSong);
                        menuComponent.replaceItem(3, "Resume Song");
                        songPaused = true;
                    }
                    else
                    {
                        Events.FireMyEvent(Selected.PauseSong);
                        menuComponent.replaceItem(3, "Resume Song");
                    }

                }

                else if (menuComponent.SelectedIndex == 4)
                {
                    Events.FireMyEvent(Selected.StopSong);
                }

            }
            oldState = Keyboard.GetState(); 
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