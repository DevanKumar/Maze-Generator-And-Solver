using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace DiscreteStructuresAE2
{
    // Game1 inherits MonoGame's (the NuGet package that I used) Game interface 
    // giving me a format for how to put my visualizer together which is then
    // handled by the code making up MonoGame and the auto generated code in
    // the Program.cs file
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;

        private Grid grid;

        private Selector startSelector;
        private Selector endSelector;
        private Selector eraseSelector;

        private GenerateButton generateButton;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        // Initialize is the place for me to initialize any variables or
        // the size of the visualizer's window
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 850;
            graphics.ApplyChanges();
            InputManager.NewColor = Color.White;
            InputManager.Generated = false;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>("Text");
            base.Initialize();
        }

        // LoadContent is where I initialized all the objects
        // that will show up on the screen
        protected override void LoadContent()
        {
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

            generateButton = new GenerateButton(generatePixel, new Vector2(GraphicsDevice.Viewport.Width - 60, 770), Color.HotPink, new Vector2(50, 50));

            grid = new Grid(40, 40, gridPixel, new Vector2(25, 25), new Vector2(20, 20));
        }

        // Update is where I update all the objects on the screen as well as
        // any needed button states from the keyboard or mouse
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

        // Draw is where all the objects are drawn to the screen
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            startSelector.Draw(spriteBatch);
            endSelector.Draw(spriteBatch);
            eraseSelector.Draw(spriteBatch);

            spriteBatch.DrawString(spriteFont, "Place a Start Node:", new Vector2(startSelector.Position.X - 180, startSelector.Position.Y + 10), Color.White);
            spriteBatch.DrawString(spriteFont, "Place a End Node:", new Vector2(endSelector.Position.X - 175, endSelector.Position.Y + 10), Color.White);
            spriteBatch.DrawString(spriteFont, "Erase a Node:", new Vector2(eraseSelector.Position.X - 138, eraseSelector.Position.Y + 10), Color.White);

            generateButton.Draw(spriteBatch);
            spriteBatch.DrawString(spriteFont, "Generate and Solve a Maze:", new Vector2(generateButton.Position.X - 260, generateButton.Position.Y + 10), Color.White);

            grid.Draw(spriteBatch);

            if (InputManager.Generated)
            {
                for (int i = 0; i < generateButton.DijkstraPath.Count - 1; i++)
                {
                    Point startVertex = generateButton.DijkstraPath[i].Value;
                    Vector2 edgeStart = new Vector2(grid.Board[startVertex.X, startVertex.Y].Position.X + (grid.TileSize.X / 2) + grid.Offset.X, grid.Board[startVertex.X, startVertex.Y].Position.Y + (grid.TileSize.Y / 2) + grid.Offset.Y);
                    Point endVertex = generateButton.DijkstraPath[i + 1].Value;
                    Vector2 edgeEnd = new Vector2(grid.Board[endVertex.X, endVertex.Y].Position.X + (grid.TileSize.X / 2) + grid.Offset.X, grid.Board[endVertex.X, endVertex.Y].Position.Y + (grid.TileSize.Y / 2) + grid.Offset.Y);
                    spriteBatch.DrawLine(edgeStart, edgeEnd, Color.HotPink, 3);
                }
                foreach(Rectangle currWall in generateButton.Walls)
                {
                    spriteBatch.DrawRectangle(new Rectangle(currWall.X + (int)grid.Offset.X, currWall.Y + (int)grid.Offset.Y, currWall.Width, currWall.Height), Color.Black);
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
