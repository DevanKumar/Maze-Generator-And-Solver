using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DiscreteStructuresAE2
{
    // The Selector class inherits the Sprite class and is used to make the buttons
    // that allow you to place a start/end node or erase a start/end node
    internal class Selector : Sprite
    {
        Color OrigColor;
        public Selector(Texture2D texture, Vector2 position, Color color, Vector2 scale) : base(texture, position, color, scale, 0) 
        {
            OrigColor = color;
        }

        // Clicked updates InputManager.NewColor to the color of the Selector which
        // correlates to Status change of the Tile the user clicks on next
        public void Clicked()
        {
            if (InputManager.CurrentMouseState.LeftButton == ButtonState.Pressed && Hitbox.Contains(InputManager.MousePosition()))
            {
                Color = Color.Black;
                InputManager.NewColor = OrigColor;
            }
            else 
            {
                Color = OrigColor;
            }
        }
    }
}
