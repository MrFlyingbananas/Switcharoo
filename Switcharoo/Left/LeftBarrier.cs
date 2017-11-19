using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Switcharoo.Player;

namespace Switcharoo.Left
{
    class LeftBarrier
    {

        public Vector2 position;
        private Texture2D barrierTexture;

        private Random numGen;
        private bool isSet;
        private int columnAmt;

        private Rectangle barrierArea;

        private int barrierSpeed;

        public LeftBarrier(Texture2D spriteSheet, Vector2 position, int barrierSpeed, int columnAmt)
        {
            barrierTexture = spriteSheet;
            this.position = position;
            this.barrierSpeed = barrierSpeed;
            numGen = new Random();
            this.columnAmt = columnAmt;
            isSet = false;
            barrierArea = new Rectangle((int)position.X, (int)position.Y, barrierTexture.Width, barrierTexture.Height);
        }

        public void Update(GameTime gameTime)
        {
            if (isSet == false)
            {
                int column = numGen.Next(1, columnAmt + 1);
                switch (column)
                {
                    case 1:
                        position.X = 0;
                        break;
                    case 2:
                        position.X = (700 / columnAmt);
                        break;
                    case 3:
                        position.X = (700 * 2 / columnAmt);
                        break;
                    case 4:
                        position.X = (700 * 3 / columnAmt);
                        break;
                    case 5:
                        position.X = (700 * 4 / columnAmt);
                        break;
                    case 6:
                        position.X = (700 * 5 / columnAmt);
                        break;
                    case 7:
                        position.X = (700 * 6 / columnAmt);
                        break;
                }
                position.Y = -barrierTexture.Height;
                isSet = true;
            }
            if(isSet == true)
            {
                position.Y += barrierSpeed;
            }

            barrierArea = new Rectangle((int)position.X, (int)position.Y, barrierTexture.Width, barrierTexture.Height);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(barrierTexture, position, Color.White);
        }

        public Rectangle getRectangle()
        {
            return barrierArea;
        }

        public void setBarrierSpeed(int barrierSpeed)
        {
            this.barrierSpeed = barrierSpeed;
        }
        
    }
}
