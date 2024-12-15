using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TimeTravelersPuzzle;

namespace PuzzleGame
{
    public class PuzzleManager
    {
        public const int GridSize = 3; // 3x3 grid for the puzzle

        private Tile[,] _tiles;
        private Tile _emptyTile;

        public PuzzleManager(Texture2D[] tileTextures)
        {
            InitializeTiles(tileTextures);
        }

        private void InitializeTiles(Texture2D[] tileTextures)
        {
            _tiles = new Tile[GridSize, GridSize];
            int tileNumber = 1;

            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    bool isEmpty = (row == GridSize - 1 && col == GridSize - 1);

                    Texture2D texture = isEmpty ? null : tileTextures[tileNumber - 1];
                    _tiles[row, col] = new Tile(tileNumber, new Point(row, col), texture, isEmpty);

                    if (isEmpty)
                        _emptyTile = _tiles[row, col];

                    tileNumber++;
                }
            }
        }

        public void ShuffleTiles()
        {
            Random rand = new Random();
            for (int i = 0; i < 100; i++)
            {
                List<Tile> neighbors = GetNeighbors(_emptyTile.Position);
                Tile randomTile = neighbors[rand.Next(neighbors.Count)];
                SwapTiles(_emptyTile, randomTile);
            }
        }

        private List<Tile> GetNeighbors(Point position)
        {
            List<Tile> neighbors = new List<Tile>();
            Point[] directions = { new Point(0, -1), new Point(0, 1), new Point(-1, 0), new Point(1, 0) };

            foreach (var dir in directions)
            {
                int newRow = position.X + dir.X;
                int newCol = position.Y + dir.Y;

                if (newRow >= 0 && newRow < GridSize && newCol >= 0 && newCol < GridSize)
                {
                    neighbors.Add(_tiles[newRow, newCol]);
                }
            }

            return neighbors;
        }

        private void SwapTiles(Tile first, Tile second)
        {
            Point tempPosition = first.Position;
            first.Position = second.Position;
            second.Position = tempPosition;

            _tiles[first.Position.X, first.Position.Y] = first;
            _tiles[second.Position.X, second.Position.Y] = second;
        }

        public void HandleTileClick(Point mousePosition, int tileSize)
        {
            int col = mousePosition.X / tileSize;
            int row = mousePosition.Y / tileSize;

            if (row >= 0 && row < GridSize && col >= 0 && col < GridSize)
            {
                Tile clickedTile = _tiles[row, col];
                if (IsAdjacent(clickedTile, _emptyTile))
                {
                    SwapTiles(clickedTile, _emptyTile);
                }
            }
        }

        private bool IsAdjacent(Tile tile1, Tile tile2)
        {
            return (Math.Abs(tile1.Position.X - tile2.Position.X) == 1 && tile1.Position.Y == tile2.Position.Y) ||
                   (Math.Abs(tile1.Position.Y - tile2.Position.Y) == 1 && tile1.Position.X == tile2.Position.X);
        }

        public bool IsSolved()
        {
            int expectedNumber = 1;

            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    Tile tile = _tiles[row, col];
                    if (!tile.IsEmpty && tile.Number != expectedNumber)
                    {
                        return false;
                    }
                    expectedNumber++;
                }
            }

            return true;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            foreach (var tile in _tiles)
            {
                tile.Draw(spriteBatch, offset);
            }
        }
    }
}
