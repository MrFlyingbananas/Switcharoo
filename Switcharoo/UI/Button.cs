using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Switcharoo.UI
{
    class Button
    {
        public delegate void EventHandler();
        public event EventHandler OnClick;
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        private Rectangle rect;
        private SoundEffect click;
        private Texture2D texture;
        private MouseState prev, curr;
        private bool pressed = false;
        private SpriteFont font;
        private String text;
        Vector2 mes;
        public Button(Texture2D texture, int x, int y, int width, int height, SoundEffect click, SpriteFont font, String text)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.click = click;
            this.texture = texture;
            this.font = font;
            this.text = text;
            rect = new Rectangle(x, y, width, height);
            prev = Mouse.GetState();
            curr = prev;
            mes = font.MeasureString(text);
        }

        public void Update(GameTime gameTime)
        {
            curr = Mouse.GetState();
            if (rect.Contains(Mouse.GetState().Position))
            {
                if(prev.LeftButton == ButtonState.Pressed && curr.LeftButton == ButtonState.Released)
                {
                    if(OnClick != null)
                    {
                        OnClick();
                    }
                    if (click != null)
                    {
                        click.Play(Game1.Volume, .3f, 0f);
                    }
                    pressed = false;
                }else if(prev.LeftButton == ButtonState.Released && curr.LeftButton == ButtonState.Pressed)
                {
                    pressed = true;
                }
            }
            if(pressed && curr.LeftButton == ButtonState.Released)
            {
                pressed = false;
            }
            prev = curr;
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle(rect.X-1, rect.Y-1, rect.Width + 8, rect.Height + 8), Color.DarkSlateGray);
            if (pressed)
            {
                spriteBatch.Draw(texture, new Rectangle(rect.X+5,rect.Y+5,rect.Width,rect.Height), Color.Gray);
                spriteBatch.DrawString(font, text, new Vector2(rect.X + (rect.Width - mes.X) / 2 + 5, rect.Y + (rect.Height - mes.Y) / 2 + 5), Color.Black);

            }
            else
            {
                spriteBatch.Draw(texture, rect, Color.Gray);
                spriteBatch.DrawString(font, text, new Vector2(rect.X + (rect.Width - mes.X) / 2, rect.Y + (rect.Height - mes.Y) / 2), Color.Black);

            }
        }
    }
}
