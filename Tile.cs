using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DiscreteStructuresAE2
{
    internal class Tile : Sprite
    {
        TileStatus Status;
        public Tile(Texture2D texture, Vector2 position, Color color, Vector2 scale) : base(texture, position, color, scale, 0)
        {
        }
        public override void Update(Vector2 offset, Color color, Vector2 boardPosition)
        {
            if (Hitbox.Contains(InputManager.MousePosition() - offset) && InputManager.CurrentMouseState.LeftButton == ButtonState.Pressed)
            {
                if (color == Color.Red && !InputManager.EndNode.Created)
                {
                    InputManager.EndNode.Created = true;
                    InputManager.EndNode.Position = boardPosition;
                }
                else if (color == Color.LimeGreen && !InputManager.StartNode.Created)
                {
                    InputManager.StartNode.Created = true;
                    InputManager.StartNode.Position = boardPosition;
                }
                Color = color;
            }
            if(Color == Color.LimeGreen)
            {
                Status = TileStatus.START;
            }
            else if(Color == Color.Red)
            {
                Status = TileStatus.END;
            }
            else
            {
                Status = TileStatus.CLEAR;
            }
        }
        public Point GetPosition(int width, int height) => new Point((int) Position.X / width, (int) Position.Y / height);
        public TileStatus GetTileStatus() => Status;
    }
}
