using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiscreteStructuresAE2
{
    internal class Grid
    { 
        public Tile[,] Board { get; private set; }
        public Vector2 Offset { get; private set; }
        public Vector2 TileSize { get; private set; }
        public Grid(int width, int height, Texture2D texture, Vector2 offset, Vector2 scale)
        {
            Board = new Tile[width, height];
            Offset = offset;
            TileSize = scale;
            for(int col = 0; col < Board.GetLength(0); col++)
            {
                for(int row = 0; row < Board.GetLength(1); row++)
                {
                    Board[col, row] = new Tile(texture, new Vector2(scale.X * col, scale.Y * row), Color.White, scale);
                }
            }
        }
        public void Update()
        {
            for (int col = 0; col < Board.GetLength(0); col++)
            {
                for (int row = 0; row < Board.GetLength(1); row++)
                {
                    if (!(InputManager.EndNode.Created && InputManager.NewColor == Color.Red) && !(InputManager.StartNode.Created && InputManager.NewColor == Color.LimeGreen))
                    {
                        Board[col, row].Update(Offset, InputManager.NewColor, new Vector2(col, row));
                        InputManager.Generated = false;
                    }
                }
            }
            if (Board[(int)InputManager.StartNode.Position.X, (int)InputManager.StartNode.Position.Y].Color != Color.LimeGreen)
            {
                InputManager.StartNode.Created = false;
                InputManager.Generated = false;
            }
            if (Board[(int)InputManager.EndNode.Position.X, (int)InputManager.EndNode.Position.Y].Color != Color.Red)
            {   
                InputManager.EndNode.Created = false;
                InputManager.Generated = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int row = 0; row < Board.GetLength(0); row++)
            {
                for (int col = 0; col < Board.GetLength(1); col++)
                {
                    spriteBatch.Draw(Board[row,col].Texture, new Vector2((int) (Offset.X + (Board[row, col].Texture.Width * Board[row, col].Scale.X) * row), (int) (Offset.Y + (Board[row, col].Texture.Height * Board[row, col].Scale.Y) * col)),  null, Board[row,col].Color, 0f, Vector2.Zero, Board[row, col].Scale, SpriteEffects.None, 0f);
                }
            }
        }
        
    }
}
