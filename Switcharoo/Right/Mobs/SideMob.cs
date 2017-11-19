using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Switcharoo.Right.Weapons;
using System;

namespace Switcharoo.Right.Mobs
{
    public class SideMob : Mob
    {
        const int MAX_HEALTH = 40;
        private const int FIRE_INTERVAL = 1250;
        private float fireTimer = FIRE_INTERVAL;
        SideWeapon weapon;
        int dir;
        public SideMob(Texture2D spriteSheet, AnimationState[] animations, int frameWidth, int frameHeight, Vector2 loc, int speed, int mobWidth, int mobHeight, Weapon weapon, int dir) : base(spriteSheet, animations, frameWidth, frameHeight, loc, speed, mobWidth, mobHeight, MAX_HEALTH)
        {
            this.weapon = (SideWeapon)weapon;
            this.dir = dir;
            this.loc.X += mobWidth / 2;
        }
        public override void Update(GameTime gameTime)
        {
            fireTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (fireTimer >= FIRE_INTERVAL && loc.Y + MobHeight/2 > 0)
            {
                fireTimer = 0;
                weapon.Fire((int)loc.X, (int)loc.Y - weapon.ProjectileHeight/2, speed);
            }
            loc.Y += speed;
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Color color = Color.White;
            if (hitTimer < HIT_DURATION)
            {
                color = Color.Red;
            }
            float rotation = 90;
            if(dir == 1)
            {
                rotation = -90;
            }
            rotation = MathHelper.ToRadians(rotation);
            spriteBatch.Draw(spriteSheet, new Rectangle((int)loc.X, (int)loc.Y, mobWidth, mobHeight), animationHandler.SourceRect, color, rotation, new Vector2(mobWidth/2,mobHeight/2), SpriteEffects.None, 0);

        }
    }
}