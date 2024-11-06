using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DiscreteStructuresAE2
{
    internal abstract class Sprite
    {
        public Texture2D Texture;
        public Vector2 Position;
        public Color Color;
        public Vector2 Scale;
        public float Rotation;
        public Rectangle Hitbox => new Rectangle(Position.ToPoint(), point(Texture, Scale));
        public Sprite(Texture2D texture, Vector2 position, Color color, Vector2 scale, float rotation)
        {
            Texture = texture;
            Position = position;
            Color = color;
            Scale = scale;
            Rotation = rotation;
        }
        public Point point(Texture2D texture, Vector2 scale)
        {
            return new Point((int) (texture.Width * scale.X), (int) (texture.Height * scale.Y));
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
        }
        public virtual void Update(Vector2 offset, Color color, Vector2 position)
        {
        }
    }
}
