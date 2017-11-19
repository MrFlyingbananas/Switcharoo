using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Switcharoo.Right.Mobs
{
    class Healthbar
    {
        Texture2D texture;
        int x, y, width, height;
        float maxFill;
        Rectangle bar, fill;
        public Healthbar(Texture2D texture, int x, int y, int width, int height)
        {
            this.texture = texture;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.maxFill = (int)(width - width * .05f);
            bar = new Rectangle(x, y, width, height);
            fill = new Rectangle((int)(x + width * .025f), (int)(y + height * .05f), (int)maxFill, (int)(height - height * .1f));
        }
        public void SetPercent(float percent)
        {
            fill = new Rectangle((int)(x + width * .025f), (int)(y + height * .05f), (int)(maxFill*percent), (int)(height - height * .1f));
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Rectangle redFill = new Rectangle((int)(x + width * .025f), (int)(y + height * .05f), (int)maxFill, (int)(height - height * .1f));

            spriteBatch.Draw(texture, bar, Color.Gray);
            spriteBatch.Draw(texture, redFill, Color.Red);
            spriteBatch.Draw(texture, fill, Color.Green);

        }
    }
}
