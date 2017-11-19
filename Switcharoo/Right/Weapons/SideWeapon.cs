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
    class SideWeapon : Weapon
    {
        private const int PROJECTILE_SPEED = 10;
        private int dir;
        public SideWeapon(Texture2D bullet, SoundEffect sound, int dir) : base(bullet, sound, 20, 5)
        {
            this.dir = dir;
        }
        
        public void Fire(int x, int y, int ySpeed)
        {
            RightGame.AddProjectile(new Projectile(bulletTexture, Color.Red, x, y, ProjectileWidth, ProjectileHeight, new Vector2(dir * PROJECTILE_SPEED, ySpeed), 1, true));
            shootSound.Play(.1f, -1f, 0f);
        }

        public override void Fire(int x, int y)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
