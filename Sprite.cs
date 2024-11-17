using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DiscreteStructuresAE2
{
    internal abstract class Sprite
    {
        public Texture2D Texture;
        public Vector2 Position;
        public Color Color;
        public Vector2 Scale;
        public float Rotation;
        public Rectangle Hitbox => new Rectangle(Position.ToPoint(), SizeAsPoint(Texture, Scale));
        public Sprite(Texture2D texture, Vector2 position, Color color, Vector2 scale, float rotation)
        {
            Texture = texture;
            Position = position;
            Color = color;
            Scale = scale;
            Rotation = rotation;
        }

        // Draw draws the sprite to the screen
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
        }

        // Update is to be overrided by every class inheriting Sprite
        public virtual void Update(Vector2 offset, Color color, Vector2 position)
        {
        }

        // SizeAsPoint gets the size of the sprite as a point
        private Point SizeAsPoint(Texture2D texture, Vector2 scale)
        {
            return new Point((int)(texture.Width * scale.X), (int)(texture.Height * scale.Y));
        }
    }
}
