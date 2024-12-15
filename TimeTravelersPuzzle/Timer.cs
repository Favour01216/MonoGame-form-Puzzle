using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTravelersPuzzle
{
    internal class Timer
    {
        private float _totalTime;  
        private float _timeLeft;
        private bool _isGameOver;  

        private SpriteFont _font; 
        private Vector2 _position; 

        public Timer(float countdownTimeInSeconds, SpriteFont font, Vector2 position)
        {
            _totalTime = countdownTimeInSeconds;
            _timeLeft = countdownTimeInSeconds;
            _isGameOver = false;
            _font = font;
            _position = position;
        }

        // Update the timer based on the elapsed game time
        public void Update(GameTime gameTime)
        {
            if (!_isGameOver)
            {
                _timeLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_timeLeft <= 0)
                {
                    _timeLeft = 0;
                    _isGameOver = true;
                    TriggerGameOver();
                }
            }
        }

      
        public void Draw(SpriteBatch spriteBatch)
        {
            string timeText = $"Time Left: {Math.Max(0, _timeLeft):0}"; 
            spriteBatch.DrawString(_font, timeText, _position, Color.White);
        }

        
        private void TriggerGameOver()
        {
            
            System.Console.WriteLine("Game Over! Time has run out.");
        }
        public bool IsGameOver()
        {
            return _isGameOver;
        }


    }
}
