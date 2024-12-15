using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using PuzzleGame;

namespace TimeTravelersPuzzle
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private enum GameState { MainMenu, Playing, GameOver }
        private GameState _currentState;

        private Texture2D _backgroundTexture;
        private Texture2D _referenceImage;
        private SpriteFont _font;

        private PuzzleManager _puzzleManager;
        private Timer _timer;

        private SoundEffect _playSound;
        private SoundEffect _winSound;
        private SoundEffect _loseSound;

        private Song _backgroundMusic; // Background music

        private Vector2 _puzzlePosition;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load assets
            _backgroundTexture = Content.Load<Texture2D>("Images/Background");
            _referenceImage = Content.Load<Texture2D>("Images/ReferenceImage");
            _font = Content.Load<SpriteFont>("Fonts/MyFont");

            // Load sound effects
            _playSound = Content.Load<SoundEffect>("Sounds/Play");
            _winSound = Content.Load<SoundEffect>("Sounds/Win");
            _loseSound = Content.Load<SoundEffect>("Sounds/Lose");

            // Load background music
            _backgroundMusic = Content.Load<Song>("Sounds/BackgroundMusic");

            // Play the background music
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.5f; // Set volume (range: 0.0 to 1.0)
            MediaPlayer.Play(_backgroundMusic);

            // Initialize puzzle manager
            Texture2D[] tileTextures = new Texture2D[8];
            for (int i = 1; i <= 8; i++)
            {
                tileTextures[i - 1] = Content.Load<Texture2D>($"Images/Tile{i}");
            }
            _puzzleManager = new PuzzleManager(tileTextures);

            // Center puzzle grid
            int tileSize = 100;
            int puzzleWidth = tileSize * PuzzleManager.GridSize;
            int puzzleHeight = tileSize * PuzzleManager.GridSize;
            _puzzlePosition = new Vector2(
                (_graphics.PreferredBackBufferWidth - puzzleWidth) / 2 + 150, // Offset to the right of the reference image
                (_graphics.PreferredBackBufferHeight - puzzleHeight) / 2
            );

            _timer = new Timer(120f, _font, new Vector2(10, 10));
            _currentState = GameState.MainMenu;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (_currentState)
            {
                case GameState.MainMenu:
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        _currentState = GameState.Playing;
                        _playSound.Play();
                        _puzzleManager.ShuffleTiles();
                        _timer.Reset();
                    }
                    break;

                case GameState.Playing:
                    _timer.Update(gameTime);

                    if (_timer.IsGameOver())
                    {
                        _currentState = GameState.GameOver;
                        _loseSound.Play();
                    }

                    HandleMouseInput();

                    if (_puzzleManager.IsSolved())
                    {
                        _currentState = GameState.GameOver;
                        _winSound.Play();
                    }
                    break;

                case GameState.GameOver:
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        _currentState = GameState.MainMenu;
                    }
                    break;
            }

            base.Update(gameTime);
        }

        private void HandleMouseInput()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Point mousePosition = new Point(Mouse.GetState().X, Mouse.GetState().Y);
                _puzzleManager.HandleTileClick(mousePosition - _puzzlePosition.ToPoint(), 100); // Adjust for centered position
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            switch (_currentState)
            {
                case GameState.MainMenu:
                    _spriteBatch.DrawString(_font, "Welcome to Puzzle Game!", new Vector2(100, 100), Color.White);
                    _spriteBatch.DrawString(_font, "Press ENTER to Start", new Vector2(100, 200), Color.White);
                    break;

                case GameState.Playing:
                    // Draw background
                    _spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);

                    // Draw reference image to the left
                    _spriteBatch.Draw(_referenceImage, new Rectangle(50, 100, 200, 200), Color.White);

                    // Draw puzzle tiles
                    _puzzleManager.Draw(_spriteBatch, _puzzlePosition);

                    // Draw timer
                    _timer.Draw(_spriteBatch);
                    break;

                case GameState.GameOver:
                    _spriteBatch.DrawString(_font, "Game Over!", new Vector2(100, 100), Color.White);
                    _spriteBatch.DrawString(_font, "Press ENTER to Restart", new Vector2(100, 200), Color.White);
                    break;
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
