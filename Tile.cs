using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DiscreteStructuresAE2
{
    // This Tile class inherits my Sprite class and is used to create the white
    // squares that make up the "game board" in the visualizer
    internal class Tile : Sprite
    {
        private TileStatus Status;
        public Tile(Texture2D texture, Vector2 position, Color color, Vector2 scale) : base(texture, position, color, scale, 0)
        {
        }
        
        // Update makes any necessary changes to the color and TileStatus of a Tile
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

        // GetPosition takes in the width and height of this Tile and returns its position
        // within the 2D array of Tiles that make up the "game board"
        public Point GetPosition(int width, int height) => new Point((int) Position.X / width, (int) Position.Y / height);

        // GetTileStatus returns this Tile's current TileStatus
        public TileStatus GetTileStatus() => Status;
    }
}
