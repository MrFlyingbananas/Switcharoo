using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Switcharoo.Right.Mobs
{
    public abstract class Mob
    {
        protected Texture2D spriteSheet;
        protected AnimationHandler animationHandler;
        protected Vector2 loc;
        protected int mobWidth, mobHeight;
        protected int speed;
        protected int health;
        protected const int HIT_DURATION = 50;
        protected float hitTimer = HIT_DURATION;
        public bool IsDead { get; protected set; }

        public Vector2 Location
        {
            get { return loc; }
        }
        public int MobWidth { get { return mobWidth; } }
        public int MobHeight { get { return mobHeight; } }

        public Rectangle Rectangle { get { return new Rectangle((int)loc.X, (int)loc.Y, mobWidth, mobHeight); } }

        public Mob(Texture2D spriteSheet, AnimationState[] animations, int frameWidth, int frameHeight, Vector2 loc, int speed, int mobWidth, int mobHeight, int health)
        {
            this.spriteSheet = spriteSheet;
            this.animationHandler = new AnimationHandler(animations, animations[0], frameWidth, frameHeight);
            this.mobWidth = mobWidth;
            this.mobHeight = mobHeight;
            this.loc = loc;
            this.speed = speed;
            this.health = health;
            IsDead = false;
        }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Color color = Color.White;
            if(hitTimer < HIT_DURATION)
            {
                color = Color.Red;
            }
            spriteBatch.Draw(spriteSheet, new Rectangle((int)loc.X, (int)loc.Y, mobWidth, mobHeight), animationHandler.SourceRect, color);
        }
        public virtual void Update(GameTime gameTime)
        {
            animationHandler.Update(gameTime);
            if(loc.Y > Game1.HEIGHT)
            {
                IsDead = true;
            }
            hitTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }
        public virtual void Hit(int damage)
        {
            health -= damage;
            if(health <= 0)
            {
                IsDead = true;
            }
            hitTimer = 0;
        }
        internal bool Intersect(Rectangle rectangle)
        {
            return new Rectangle((int)loc.X, (int)loc.Y, mobWidth, mobHeight).Intersects(rectangle);
        }
    }
}
