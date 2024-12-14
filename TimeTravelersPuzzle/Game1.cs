using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TimeTravelersPuzzle
{
    public class Game1 : Game
    {
        // creates the e num for game states 
        public enum GameState
        {
            MainMenu,
            Playing,
            GameOver
        }


        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Timer _timer;
        private GameState _currentState;
        private PuzzleManager _puzzleManager;
        private SpriteFont _font;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _currentState = GameState.MainMenu;
        }

        protected override void Initialize()
        {
            _puzzleManager = new PuzzleManager();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _font = Content.Load<SpriteFont>("DefaultFont");

            _timer = new Timer(60f, _font, new Vector2(10, 10)); //add the font  to this line  where _font is 

            Tile[,] tiles = _puzzleManager.GetTiles();
            foreach (var tile in tiles)
            {
                if (!tile.IsEmpty)
                {
                    tile.Texture = Content.Load<Texture2D>($"Tile{tile.Number}"); // Make sure Tile1.png,Tile2.png,etc are in the content folder
                }
            }
        }
            // TODO: use this.Content to load your game content here

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            _timer.Update(gameTime);
          
            //swickh statment to change  betwen menu game and game over
            switch (_currentState)
            {
                case GameState.MainMenu:
                    
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        _currentState = GameState.Playing;
                        _timer = new Timer(60f, _font, new Vector2(10, 10));
                        _puzzleManager.ShuffleTiles();
                    }
                    break;

                case GameState.Playing:
                    
                    _timer.Update(gameTime);

                    if (_timer.IsGameOver())
                    {
                        _currentState = GameState.GameOver;
                    }
                    break;

                    MouseState mouseState = Mouse.GetState();
                    if (mouseState.LeftButton == ButtonState.Pressed) 
                    {
                        Point mousePos = mouseState.Position.ToPoint();

                        int tileSize = 100; // Each tile is 100x100 pixels
                        int row = mousePos.Y / tileSize;
                        int col = mousePos.X / tileSize;

                        if (row >= 0 && row < 3 && col >= 0 && col < 3)
                        {
                            Tile clickedTile = _puzzleManager.GetTiles()[row, col];
                            if (!clickedTile.IsEmpty)
                            {
                                Point emptyPos = _puzzleManager.GetEmptyTilePosition();
                                bool isAdjacent = (Math.Abs(emptyPos.X - row) == 1 && emptyPos.Y == col) ||
                                                  (Math.Abs(emptyPos.Y - col) == 1 && emptyPos.X == row);

                                if (isAdjacent)
                                {
                                    _puzzleManager.SwapWithEmptyTile(clickedTile);

                                    if (_puzzleManager.IsSolved())
                                    {
                                        _currentState = GameState.GameOver;
                                    }
                                }
                            }
                        }
                    }
                    
                case GameState.GameOver:
                   
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        _currentState = GameState.MainMenu;
                    }
                    break;
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
             _spriteBatch.Begin();
           
            switch (_currentState)
            {
                case GameState.MainMenu:
                    _spriteBatch.DrawString(_font, "Welcome to Time Traveler's Puzzle!", new Vector2(100, 100), Color.White);
                    _spriteBatch.DrawString(_font, "Press ENTER to Start", new Vector2(100, 200), Color.White);
                    
                    break;

                case GameState.Playing:
                    Tile[,] tiles = _puzzleManager.GetTiles();
                    foreach (var tile in tiles)
                    {
                        tile.Draw(_spriteBatch, new Vector2(50, 50), 100); 
                    }
                    _timer.Draw(_spriteBatch);
                    break;

                case GameState.GameOver:
                   _spriteBatch.DrawString(_font, "Game Over!", new Vector2(200, 100), Color.White);
                   _spriteBatch.DrawString(_font, "Press ENTER to Restart", new Vector2(200, 200), Color.White);
                    break;
            }

            _spriteBatch.End();
            
            // TODO: Add your drawing code here
            _timer.Draw(_spriteBatch);
            base.Draw(gameTime);
        }
    }
}
