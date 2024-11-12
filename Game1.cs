using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using MonoGame.Extended;

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

        GenerateButton generateButton;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 850;
            graphics.ApplyChanges();
            InputManager.NewColor = Color.White;
            InputManager.Generated = false;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            var gridPixel = new Texture2D(GraphicsDevice, 1, 1);
            gridPixel.SetData(new Color[] { Color.White });
            var startPixel = new Texture2D(GraphicsDevice, 1, 1);
            startPixel.SetData(new Color[] { Color.LimeGreen });
            var endPixel = new Texture2D(GraphicsDevice, 1, 1);
            endPixel.SetData(new Color[] { Color.Red });
            var erasePixel = new Texture2D(GraphicsDevice, 1, 1);
            erasePixel.SetData(new Color[] { Color.White });
            var generatePixel = new Texture2D(GraphicsDevice, 1, 1);
            generatePixel.SetData(new Color[] { Color.LightPink });

            startSelector = new Selector(startPixel, new Vector2(GraphicsDevice.Viewport.Width - 60, 25), Color.LimeGreen, new Vector2(50, 50));
            endSelector = new Selector(endPixel, new Vector2(GraphicsDevice.Viewport.Width - 60, 105), Color.Red, new Vector2(50, 50));
            eraseSelector = new Selector(erasePixel, new Vector2(GraphicsDevice.Viewport.Width - 60, 185), Color.White, new Vector2(50, 50));

            generateButton = new GenerateButton(generatePixel, new Vector2(GraphicsDevice.Viewport.Width - 60, 770), Color.LightPink, new Vector2(50, 50));

            grid = new Grid(10, 10, gridPixel, new Vector2(25, 25), new Vector2(20, 20));
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

            generateButton.Clicked(grid.Board, grid.TileSize);

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

            generateButton.Draw(spriteBatch);

            grid.Draw(spriteBatch);

            if (InputManager.Generated)
            {
                for (int i = 0; i < generateButton.DijkstraPath.Count - 1; i++)
                {
                    Point startVertex = generateButton.DijkstraPath[i].Value;
                    Vector2 edgeStart = new Vector2(grid.Board[startVertex.X, startVertex.Y].Position.X + (grid.TileSize.X / 2) + grid.Offset.X, grid.Board[startVertex.X, startVertex.Y].Position.Y + (grid.TileSize.Y / 2) + grid.Offset.Y);
                    Point endVertex = generateButton.DijkstraPath[i + 1].Value;
                    Vector2 edgeEnd = new Vector2(grid.Board[endVertex.X, endVertex.Y].Position.X + (grid.TileSize.X / 2) + grid.Offset.X, grid.Board[endVertex.X, endVertex.Y].Position.Y + (grid.TileSize.Y / 2) + grid.Offset.Y);
                    spriteBatch.DrawLine(edgeStart, edgeEnd, Color.DarkBlue);
                }
                foreach(Rectangle currWall in generateButton.Walls)
                {
                    spriteBatch.DrawRectangle(new Rectangle(currWall.X + (int)grid.Offset.X, currWall.Y + (int)grid.Offset.Y, currWall.Width, currWall.Height), Color.Pink);
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
