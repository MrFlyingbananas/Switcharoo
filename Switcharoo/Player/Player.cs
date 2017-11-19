using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Switcharoo.Player
{
    abstract class Player
    {
        protected Texture2D spriteSheet;
        protected Controls controls;

        public Player(Texture2D spriteSheet, Controls controls)
        {
            this.spriteSheet = spriteSheet;
            this.controls = controls;
        }
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public Controls ChangeControls(Controls controls)
        {
            Controls prev = this.controls;
            this.controls = controls;
            return prev;
        }
        public abstract void Swap();
    }
}
