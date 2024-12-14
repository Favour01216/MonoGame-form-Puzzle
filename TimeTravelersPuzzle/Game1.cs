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

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _timer = new Timer(60f, _font, new Vector2(10, 10)); //add the font  to this line  where _font is 


            // TODO: use this.Content to load your game content here
        }

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
                    }
                    break;

                case GameState.Playing:
                    
                    _timer.Update(gameTime);

                    if (_timer.IsGameOver())
                    {
                        _currentState = GameState.GameOver;
                    }
                    break;

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
           
            switch (_currentState)
            {
                case GameState.MainMenu:
                    // draw maine menu here 

                    break;

                case GameState.Playing:
                    //draw game here 
                    _timer.Draw(_spriteBatch);
                    break;

                case GameState.GameOver:
                    // drow game over here
                    break;
            }
            // TODO: Add your drawing code here
            _timer.Draw(_spriteBatch);
            base.Draw(gameTime);
        }
    }
}
