using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Switcharoo.Right.Weapons
{
    class MobWeapon : Weapon
    {
        private const int PROJECTILE_SPEED = 10;
        public MobWeapon(Texture2D bullet, SoundEffect sound) : base(bullet,sound, 5, 20)
        {
        }

        public override void Fire(int x, int y)
        {
            RightGame.AddProjectile(new Projectile(bulletTexture, Color.Red, x, y, ProjectileWidth, ProjectileHeight, new Vector2(0, 1) * PROJECTILE_SPEED, 1, true));
            shootSound.Play(.01f, -1f, 0f);
        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
