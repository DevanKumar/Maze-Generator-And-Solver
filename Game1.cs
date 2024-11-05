using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using MonoGame.Extended;
using AStarVisualizer;

namespace DiscreteStructuresAE2
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        Grid grid;

        Selector startSelector;
        Selector endSelector;
        Selector eraseSelector;

        Color color;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 800;
            graphics.ApplyChanges();
            InputManager.NewColor = Color.White;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            color = Color.White;
            var gridPixel = new Texture2D(GraphicsDevice, 1, 1);
            gridPixel.SetData(new Color[] { Color.White });
            var startPixel = new Texture2D(GraphicsDevice, 1, 1);
            startPixel.SetData(new Color[] { Color.LimeGreen });
            var endPixel = new Texture2D(GraphicsDevice, 1, 1);
            endPixel.SetData(new Color[] { Color.Red });
            var erasePixel = new Texture2D(GraphicsDevice, 1, 1);
            erasePixel.SetData(new Color[] { Color.White });

            startSelector = new Selector(startPixel, new Vector2(GraphicsDevice.Viewport.Width - 60, 0), Color.LimeGreen, new Vector2(50, 50), "start");
            endSelector = new Selector(endPixel, new Vector2(GraphicsDevice.Viewport.Width - 60, 120), Color.Red, new Vector2(50, 50), "end");
            eraseSelector = new Selector(erasePixel, new Vector2(GraphicsDevice.Viewport.Width - 60, 240), Color.White, new Vector2(50, 50), "erase");

            grid = new Grid(50, 40, gridPixel, new Vector2(25, 0), new Vector2(20, 20));
        }

        protected override void Update(GameTime gameTime)
        {
            if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            InputManager.CurrentMouseState = Mouse.GetState();

            grid.Update();

            startSelector.Clicked();
            endSelector.Clicked();
            eraseSelector.Clicked();

            InputManager.PreviousMouseState = InputManager.CurrentMouseState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            startSelector.Draw(spriteBatch);
            endSelector.Draw(spriteBatch);
            eraseSelector.Draw(spriteBatch);

            grid.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
