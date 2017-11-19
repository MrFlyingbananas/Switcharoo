using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Switcharoo
{
    public abstract class SplitGame
    {
        public enum DrawSide
        {
            Left,
            Right

        }
        protected GraphicsDeviceManager graphics;
        protected SpriteBatch spriteBatch;
        protected Game1 game;
        protected Vector2 offset;

        public SplitGame(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, Game1 game)
        {
            this.graphics = graphics;
            this.spriteBatch = spriteBatch;
            this.game = game;
            offset = new Vector2(graphics.PreferredBackBufferWidth/2, 0);
        }
        abstract public void LoadContent();
        abstract public void Update(GameTime gameTime);
        abstract public void Draw(GameTime gameTime, DrawSide drawSide);
        abstract public void ChangeControls(Controls controls);
        public abstract void Swap();
    }
}
