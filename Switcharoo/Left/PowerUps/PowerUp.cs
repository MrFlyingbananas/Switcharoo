using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Switcharoo.PowerUps
{
    class PowerUp
    {

        public Vector2 position;
        private Texture2D powerUpTexture;
        private PowerUpType powerUp;
        private Rectangle powerUpArea;

        private Random randPos;
        private bool isSet;

        public enum PowerUpType
        {
            freezeBarriers,
            breakBarriers
        }

        public PowerUp(Texture2D spriteSheet, Vector2 position, PowerUpType powerUp)
        {
            powerUpTexture = spriteSheet;
            this.position = position;
            this.powerUp = powerUp;
            randPos = new Random();
            powerUpArea = new Rectangle((int)position.X, (int)position.Y, powerUpTexture.Width, powerUpTexture.Height);

            isSet = false;
        }

        public void Update(GameTime gameTime)
        {
            if (isSet == false)
            {
                int xCoordinate = randPos.Next(0, 675);
                position.X = xCoordinate;
                isSet = true;
            }

            if (isSet == true)
            {
                position.Y += 5;
            }

            powerUpArea = new Rectangle((int)position.X, (int)position.Y, powerUpTexture.Width, powerUpTexture.Height);


        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(powerUpTexture, position, Color.White);
        }

        public Rectangle getRectangle()
        {
            return powerUpArea;
        }

        public PowerUpType getPowerUpType()
        {
            return powerUp;
        }


    }
}
