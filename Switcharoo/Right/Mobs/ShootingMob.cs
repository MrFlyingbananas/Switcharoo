using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Switcharoo.Right.Weapons;

namespace Switcharoo.Right.Mobs
{
    public class ShootingMob : Mob
    {
        const int MAX_HEALTH = 5;
        private const int FIRE_INTERVAL = 1000;
        private float fireTimer = FIRE_INTERVAL;
        Weapon weapon;
        public ShootingMob(Texture2D spriteSheet, AnimationState[] animations, int frameWidth, int frameHeight, Vector2 loc, int speed, int mobWidth, int mobHeight, Weapon weapon) : base(spriteSheet, animations, frameWidth, frameHeight, loc, speed, mobWidth, mobHeight, MAX_HEALTH)
        {
            this.weapon = weapon;
        }
        public override void Update(GameTime gameTime)
        {
            fireTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (fireTimer > FIRE_INTERVAL && loc.Y + MobHeight > 0)
            {
                fireTimer = 0;
                weapon.Fire((int)loc.X + MobWidth/2 - weapon.ProjectileWidth/2, (int)loc.Y - weapon.ProjectileHeight + mobHeight);
            }
            loc.Y += speed;
            base.Update(gameTime);
        }
    }
}