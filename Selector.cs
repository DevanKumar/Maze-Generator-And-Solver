using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DiscreteStructuresAE2
{
    internal class Selector : Sprite
    {
        Color OrigColor;
        public Selector(Texture2D texture, Vector2 position, Color color, Vector2 scale) : base(texture, position, color, scale, 0) 
        {
            OrigColor = color;
        }

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
