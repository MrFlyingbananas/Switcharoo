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
    class BasicWeapon : Weapon
    {
        private const int RELOAD_DURATION = 500;
        private float timer = RELOAD_DURATION;
        private const int PROJECTILE_SPEED = 20;
        private const int WEAPON_DAMAGE = 5;
        public BasicWeapon(Texture2D bullet, SoundEffect sound) : base(bullet, sound, 8, 20)
        {

        }
        public override void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }
        public override void Fire(int x, int y)
        {
            if(timer > RELOAD_DURATION)
            {
                timer = 0;
                RightGame.AddProjectile(new Projectile(bulletTexture, Color.Red, x, y, ProjectileWidth, ProjectileHeight, new Vector2(0, -1) * PROJECTILE_SPEED, WEAPON_DAMAGE, false));
                shootSound.Play(.1f, 0f, 0f);
            }

        }
    }
}
