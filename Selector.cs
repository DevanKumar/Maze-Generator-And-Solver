using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AStarVisualizer
{
    class Selector : Tile
    {
        String type;
        Color origColor;
        public Selector(Texture2D texture, Vector2 position, Color color, Vector2 scale, String type) : base(texture, position, color, scale) 
        {
            this.type = type;
            origColor = color;
        }

        public void Clicked()
        {
            if (InputManager.CurrentMouseState.LeftButton == ButtonState.Pressed && Hitbox.Contains(InputManager.MousePosition()))
            {
                Color = Color.Black;
                InputManager.NewColor = origColor;
            }
            else 
            {
                Color = origColor;
            }
        }
    }
}
