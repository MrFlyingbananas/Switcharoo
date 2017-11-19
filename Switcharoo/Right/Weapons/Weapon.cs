using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Switcharoo.Right.Weapons
{
    public abstract class Weapon
    {
        protected Texture2D bulletTexture;
        public int ProjectileWidth { get; private set; }
        public int ProjectileHeight { get; private set; }
        protected SoundEffect shootSound;
        public Weapon(Texture2D bullet, SoundEffect sound, int projectileWidth, int projectileHeight)
        {
            this.bulletTexture = bullet;
            this.ProjectileWidth = projectileWidth;
            this.ProjectileHeight = projectileHeight;
            this.shootSound = sound;
        }

        public abstract void Fire(int x, int y);
        public abstract void Update(GameTime gameTime);
    }
}
