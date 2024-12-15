using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TimeTravelersPuzzle
{
    public class Timer
    {
        private float _timeLeft;
        private SpriteFont _font;
        private Vector2 _position;

        public Timer(float countdownTime, SpriteFont font, Vector2 position)
        {
            _timeLeft = countdownTime;
            _font = font;
            _position = position;
        }

        public void Update(GameTime gameTime)
        {
            _timeLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timeLeft < 0)
            {
                _timeLeft = 0;
            }
        }

        public bool IsGameOver()
        {
            return _timeLeft <= 0;
        }

        public void Reset()
        {
            _timeLeft = 120f; // Reset to 2 minutes
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            string text = $"Time Left: {_timeLeft:0.00}s";
            spriteBatch.DrawString(_font, text, _position, Color.White);
        }
    }
}