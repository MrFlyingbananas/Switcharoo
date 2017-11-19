using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Switcharoo.Right.Weapons
{
    class Projectile
    {
        private Texture2D bulletTexture;
        private int width, height;
        private Vector2 vel, loc;
        private Color color;
        public int Y
        {
            get { return (int)loc.Y; }
        }

        public int Damage { get; private set; }
        public bool MobShot { get; private set; }
        public Rectangle Rectangle
        {
            get { return new Rectangle((int)loc.X, (int)loc.Y, width, height); }
        }

        public int X { get { return (int)loc.X; } }

        public int Width { get { return width; } }

        public Projectile(Texture2D bulletTexture, Color color, int x, int y, int width, int height, Vector2 velocity, int damage, bool mobShot)
        {
            this.bulletTexture = bulletTexture;
            loc = new Vector2(x, y);
            this.width = width;
            this.height = height;
            this.vel = velocity;
            this.color = color;
            this.Damage = damage;
            MobShot = mobShot;
        }

        public void Update(GameTime gameTime)
        {
            loc.X += vel.X;
            loc.Y += vel.Y;
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bulletTexture, new Rectangle((int)loc.X, (int)loc.Y, width, height), color);
        }
    }
}
