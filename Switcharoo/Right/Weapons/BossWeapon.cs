using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Switcharoo.Right.Weapons
{
    class BossWeapon : Weapon
    {
        private const int PROJECTILE_SPEED = 5;
        public BossWeapon(Texture2D bullet, SoundEffect sound) : base(bullet,sound, 20, 20)
        {
        }

        public override void Fire(int x, int y)
        {
            RightGame.AddProjectile(new Projectile(bulletTexture, Color.Red, x, y, ProjectileWidth, ProjectileHeight, new Vector2(.4f, 1) * PROJECTILE_SPEED, 1, true));
            RightGame.AddProjectile(new Projectile(bulletTexture, Color.Red, x, y, ProjectileWidth, ProjectileHeight, new Vector2(-.4f, 1) * PROJECTILE_SPEED, 1, true));
            RightGame.AddProjectile(new Projectile(bulletTexture, Color.Red, x, y, ProjectileWidth, ProjectileHeight, new Vector2(0f, 1) * PROJECTILE_SPEED, 1, true));
            shootSound.Play(.2f, -0f, 0f);
        }
        public void Fire2(int x, int y, float velX)
        {
            RightGame.AddProjectile(new Projectile(bulletTexture, Color.Red, x, y, ProjectileWidth, ProjectileHeight, new Vector2(velX/2, PROJECTILE_SPEED), 1, true));
            shootSound.Play(.2f, -0f, 0f);
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
