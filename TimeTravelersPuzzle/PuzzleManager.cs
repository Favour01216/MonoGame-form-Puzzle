using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace TimeTravelersPuzzle
{
    internal class PuzzleManager
    {
        public const int GridSize = 3; 
        private Tile[,] _tiles; 
        private Tile _emptyTile;
        private Random _random = new Random();

        public PuzzleManager()
        {
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            _tiles = new Tile[GridSize, GridSize];
            int tileNumber = 1;

            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    bool isEmpty = (row == GridSize - 1 && col == GridSize - 1);

                    _tiles[row, col] = new Tile(tileNumber, new Point(row, col), isEmpty);

                    if (isEmpty)
                        _emptyTile = _tiles[row, col];

                    tileNumber++;
                }
            }
        }

        public void ShuffleTiles()
        {
            for (int i = 0; i < 100; i++)
            {
                List<Tile> neighbors = GetNeighbors(_emptyTile.Position);

                if (neighbors.Count > 0)
                {
                    int randomIndex = _random.Next(neighbors.Count);
                    Tile randomTile = neighbors[randomIndex];

                    SwapWithEmptyTile(randomTile);
                }
            }
        }

        public Tile[,] GetTiles()
        {
            return _tiles;
        }

        public Point GetEmptyTilePosition()
        {
            return _emptyTile.Position;
        }

        public void SwapWithEmptyTile(Tile tile)
        {
            Point tempPos = tile.Position;

            tile.Position = _emptyTile.Position;
            _emptyTile.Position = tempPos;

            _tiles[tile.Position.X, tile.Position.Y] = tile;
            _tiles[_emptyTile.Position.X, _emptyTile.Position.Y] = _emptyTile;
        }

        public bool IsSolved()
        {
            int expectedNumber = 1;

            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {

                    if (_tiles[row, col].IsEmpty)
                        continue;

                    if (_tiles[row, col].Number != expectedNumber)
                        return false;

                    expectedNumber++;
                }
            }

            return true;
        }

        private List<Tile> GetNeighbors(Point position)
        {
            List<Tile> neighbors = new List<Tile>();

            Point[] directions = { new Point(0, -1), new Point(0, 1), new Point(-1, 0), new Point(1, 0) };

            foreach (var direction in directions)
            {
                int newRow = position.X + direction.X;
                int newCol = position.Y + direction.Y;

                if (newRow >= 0 && newRow < GridSize && newCol >= 0 && newCol < GridSize)
                {
                    neighbors.Add(_tiles[newRow, newCol]);
                }
            }

            return neighbors;
        }
    }
}
