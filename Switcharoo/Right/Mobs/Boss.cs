using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Switcharoo.Right.Weapons;
using Microsoft.Xna.Framework.Audio;

namespace Switcharoo.Right.Mobs
{
    class Boss : Mob
    {
        private const int FIRE_INTERVAL = 2000;
        float fireTimer = 0;
        Weapon[] weapons;
        bool drawHealth = false;
        Healthbar healthbar;
        const int MAX_HEALTH = 375;
        public const int BOSS_STOP_Y = 75;
        const int MAX_SIDE_SPEED = 6;
        const int MIN_SIDE_SPEED = 2;
        int sideSpeed;
        int flip = -1;
        bool sprinkle = false;
        bool startSprinkle = false;
        float sprinkleTimer = 0;
        private SoundEffect defeatSound;
        Random random;
        public Boss(Healthbar healthbar, Texture2D spriteSheet,SoundEffect defeatSound, AnimationState[] animations, int frameWidth, int frameHeight, Vector2 loc, int speed, int mobWidth, int mobHeight, Weapon[] weapons) : base(spriteSheet, animations, frameWidth, frameHeight, loc, speed, mobWidth, mobHeight, MAX_HEALTH)
        {
            this.weapons = weapons;
            this.defeatSound = defeatSound;
            random = new Random();
            sideSpeed = random.Next(MIN_SIDE_SPEED, MAX_SIDE_SPEED);
            this.healthbar = healthbar;
        }
        public override void Update(GameTime gameTime)
        {
            if(loc.Y >= BOSS_STOP_Y)
            {
                fireTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                sprinkleTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            if (sprinkle && sprinkleTimer > 250)
            {
                sprinkleTimer = 0;
                ((BossWeapon)weapons[0]).Fire2((int)loc.X + 20, (int)loc.Y - 50 + mobHeight, sideSpeed * flip);
                ((BossWeapon)weapons[1]).Fire2((int)loc.X + mobWidth - 40, (int)loc.Y - 50 + mobHeight, sideSpeed * flip);
            }
            if (fireTimer > FIRE_INTERVAL)
            {
                fireTimer = 0;
                sprinkleTimer = 0;
                if (!startSprinkle && !sprinkle)
                {
                    weapons[0].Fire((int)loc.X + 20, (int)loc.Y - 50 + mobHeight);
                    weapons[1].Fire((int)loc.X + mobWidth - 40, (int)loc.Y - 50 + mobHeight);
                    startSprinkle = true;
                }
                else if(startSprinkle && !sprinkle)
                {
                    sprinkle = true;
                    startSprinkle = false;
                }
                else if(sprinkle)
                {
                    sprinkle = false;
                }

                sideSpeed = random.Next(MIN_SIDE_SPEED, MAX_SIDE_SPEED);
            }
            animationHandler.Update(gameTime);
            if(loc.Y < BOSS_STOP_Y)
                loc.Y += speed;
            else
            {
                drawHealth = true;
                loc.X += sideSpeed * flip;
                if(loc.X <= 0 || loc.X + MobWidth >= Game1.WIDTH/2)
                {
                    flip = -flip;
                }
            }
            base.Update(gameTime);
        }
        public override void Hit(int damage)
        {
            base.Hit(damage);
            if (IsDead)
            {
                defeatSound.Play(Game1.Volume, 0f, 0f);
            }
            healthbar.SetPercent(health / (float)MAX_HEALTH);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (drawHealth)
            {
                healthbar.Draw(gameTime, spriteBatch);
            }
            base.Draw(gameTime, spriteBatch);
        }
    }
}
