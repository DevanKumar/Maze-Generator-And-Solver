using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AStarVisualizer
{
    class Grid
    { 
        Tile[,] grid;
        Vector2 offset;
        public Grid(int width, int height, Texture2D texture, Vector2 offset, Vector2 scale)
        {
            grid = new Tile[width, height];
            this.offset = offset;
            for(int row = 0; row < grid.GetLength(0); row++)
            {
                for(int col = 0; col < grid.GetLength(1); col++)
                {
                    grid[row, col] = new Tile(texture, new Vector2(scale.X * row, scale.Y * col), Color.White, scale);
                }
            }
        }
        public void Update()
        {
            for (int row = 0; row < grid.GetLength(0); row++)
            {
                for (int col = 0; col < grid.GetLength(1); col++)
                {
                    if (!(InputManager.EndNode.Created && InputManager.NewColor == Color.Red) && !(InputManager.StartNode.Created && InputManager.NewColor == Color.LimeGreen))
                    {
                        grid[row, col].Update(offset, InputManager.NewColor, new Vector2(row, col));
                    }
                }
            }
            if (grid[(int)InputManager.StartNode.Position.X, (int)InputManager.StartNode.Position.Y].Color != Color.LimeGreen)
            {
                InputManager.StartNode.Created = false;
            }
            if (grid[(int)InputManager.EndNode.Position.X, (int)InputManager.EndNode.Position.Y].Color != Color.Red)
            {   
                InputManager.EndNode.Created = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int row = 0; row < grid.GetLength(0); row++)
            {
                for (int col = 0; col < grid.GetLength(1); col++)
                {
                    spriteBatch.Draw(grid[row,col].Texture, new Vector2((int) (offset.X + (grid[row, col].Texture.Width * grid[row, col].Scale.X) * row), (int) (offset.Y + (grid[row, col].Texture.Height * grid[row, col].Scale.Y) * col)),  null, grid[row,col].Color, 0f, Vector2.Zero, grid[row, col].Scale, SpriteEffects.None, 0f);
                }
            }
        }
        
    }
}
