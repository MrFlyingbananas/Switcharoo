using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Switcharoo.PowerUps
{
    class Component
    {

        public Vector2 position;
        private Texture2D componentTexture;

        public Component(Texture2D spriteSheet, Vector2 position)
        {
            componentTexture = spriteSheet;
            this.position = position;
        }

        public void Update(GameTime gameTime)
        {
           

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(componentTexture, position, Color.White);   
        }

    }
}
