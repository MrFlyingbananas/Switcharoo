using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Switcharoo.Player
{
    class HealthPool
    {
        Texture2D heartTexture;
        float health;
        int heartWidth, heartHeight;
        SoundEffect hit;
        public int Height { get; private set; }
        private Game1 game;
        public HealthPool(Texture2D heartTexture, SoundEffect hit, Game1 game, int heartWidth, int heartHeight, int health)
        {
            this.health = health;
            this.heartHeight = heartHeight;
            this.heartWidth = heartWidth;
            this.heartTexture = heartTexture;
            Height = heartHeight + 10;
            this.hit = hit;
            this.game = game;
        }
        public void Hit(int damage)
        {
            hit.Play(Game1.Volume, 0f, 0f);
            health -= damage*.5f;
            if(health <= 0)
            {
                game.EndGame();
            }
        }
        public void Update(GameTime gameTIme)
        {

        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(health > 0)
            {
                int startX = Game1.WIDTH / 2 - (int)(heartWidth * health) / 2;
                int y = Game1.HEIGHT - (heartHeight + 10);
                if (health - (int)health != 0)
                {
                    startX -= heartWidth / 4;
                }
                for (int i = 0; i < health - 1; i++)
                {
                    spriteBatch.Draw(heartTexture, new Rectangle(startX + heartWidth * i, y, heartWidth, heartHeight), Color.White);
                }
                if (health - (int)health != 0)
                {
                    spriteBatch.Draw(heartTexture, new Rectangle(startX + heartWidth * (int)(health), y, heartWidth / 2, heartHeight), new Rectangle(0, 0, heartWidth / 2, heartHeight), Color.White);
                }
                else
                {
                    spriteBatch.Draw(heartTexture, new Rectangle(startX + heartWidth * (int)(health - 1), y, heartWidth, heartHeight), Color.White);
                }
            }
            
        }
    }
}
