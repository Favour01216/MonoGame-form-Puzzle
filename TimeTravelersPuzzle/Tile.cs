using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TimeTravelersPuzzle
{
    internal class Tile
    {
        public int Number { get; private set; }
        public Point Position { get; set; }
        public Texture2D Texture { get; set; }
        public bool IsEmpty { get; private set; }

        public Tile(int number, Point position, bool isEmpty = false)
        {
            Number = number;
            Position = position;
            IsEmpty = isEmpty;
        }

        // Draw the tile
        public void Draw(SpriteBatch spriteBatch, Vector2 gridOffset, int tileSize)
        {
            if (!IsEmpty) // Don't draw the empty tile
            {
                Vector2 drawPosition = new Vector2(Position.Y * tileSize, Position.X * tileSize) + gridOffset;
                spriteBatch.Draw(Texture, new Rectangle((int)drawPosition.X, (int)drawPosition.Y, tileSize, tileSize), Color.White);
            }
        }
    }
}
