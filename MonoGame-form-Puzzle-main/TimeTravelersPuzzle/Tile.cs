using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TimeTravelersPuzzle
{
    public class Tile
    {
        public int Number { get; }
        public Point Position { get; set; }
        public Texture2D Texture { get; }
        public bool IsEmpty { get; }

        public Tile(int number, Point position, Texture2D texture, bool isEmpty)
        {
            Number = number;
            Position = position;
            Texture = texture;
            IsEmpty = isEmpty;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            if (!IsEmpty)
            {
                Rectangle destination = new Rectangle(
                    (Position.Y * 100) + (int)offset.X,
                    (Position.X * 100) + (int)offset.Y,
                    100,
                    100
                );
                spriteBatch.Draw(Texture, destination, Color.White);
            }
        }

    }
}