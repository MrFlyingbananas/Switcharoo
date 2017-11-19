using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Switcharoo.Right.Weapons;
using Switcharoo.Right.Mobs;
using Microsoft.Xna.Framework.Audio;

namespace Switcharoo.Right
{
    class RightGame : SplitGame
    {
        RightPlayer player;
        Texture2D background;
        static List<Projectile> projectiles, removeProjectiles;
        static List<Mob> mobs, removeMobs;
        private float startDelayTimer = 0;
        public static bool HasMobs {
            get {return mobs.Count != 0;}
            set
            {
                mobs = new List<Mob>();
            }
        }
        public static Weapon BasicWeapon { get; private set; }
        public static float MaxHeight { get; set; }
        public Controls Control { get; private set; }

        public RightGame(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, Game1 game) : base(graphics, spriteBatch, game)
        {
            projectiles = new List<Projectile>();
            mobs = new List<Mob>();
            MaxHeight = 0;
        }

        public override void LoadContent()
        {
            Texture2D playerSprite = game.Content.Load<Texture2D>("PlayerSpriteSheet");
            background = game.Content.Load<Texture2D>("RightBG");
            Controls controls = Game1.rightControls;
            Control = controls;
            BasicWeapon = new BasicWeapon(game.Content.Load<Texture2D>("Bullet"), game.Content.Load<SoundEffect>("GameSounds/playershoot"));
            player = new RightPlayer(playerSprite, controls);
            MobSpawner.LoadContent(game);
        }

        public static void AddProjectile(Projectile projectile)
        {
            projectiles.Add(projectile);
        }
        public static void AddMob(Mob mob)
        {
            mobs.Add(mob);
        }


        public override void Update(GameTime gameTime)
        {
            startDelayTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if(startDelayTimer < Game1.PLAY_START_DELAY)
            {
                player.Update(gameTime);
                foreach (Projectile p in projectiles)
                {
                    p.Update(gameTime);
                }
                return;
            }
            MobSpawner.Update(gameTime);
            player.Update(gameTime);
            removeProjectiles = new List<Projectile>();
            removeMobs = new List<Mob>();
            foreach (Mob m in mobs)
            {
                m.Update(gameTime);

                if (player.Intersect(m.Rectangle) && !m.IsDead)
                {
                    m.Hit(1000000);
                    player.Hit(1);
                }
                if (m.IsDead)
                {
                    removeMobs.Add(m);
                }
            }
            foreach (Projectile p in projectiles)
            {
                p.Update(gameTime);
                if (!p.MobShot)
                {
                    foreach (Mob m in mobs)
                    {
                        if (m.Intersect(p.Rectangle))
                        {
                            m.Hit(p.Damage);
                            removeProjectiles.Add(p);
                        }
                        if (m.IsDead)
                        {
                            if(m.GetType() == typeof(Boss))
                            {
                                game.Victory(m.Location);
                            }
                        }
                    }
                }
                else
                {
                    if (player.Intersect(p.Rectangle))
                    {
                        player.Hit(p.Damage);
                        removeProjectiles.Add(p);
                    }
                }
                if(p.Y < -30 || p.Y > Game1.HEIGHT || p.X + p.Width < 0 || p.X > Game1.WIDTH/2)
                {
                    removeProjectiles.Add(p);
                }
            }
            foreach(Projectile p in removeProjectiles)
            {
                projectiles.Remove(p);
            }
            foreach (Mob m in removeMobs)
            {
                mobs.Remove(m);
            }
        }

        public override void Draw(GameTime gameTime, DrawSide drawSide)
        {
            Matrix drawMatrix = Matrix.Identity;
            drawMatrix *= Matrix.CreateScale(1, game.CompressFactor, 1);
            if (drawSide == DrawSide.Right)
            {
                drawMatrix *= Matrix.CreateTranslation(offset.X-game.DrawOffset.X, game.DrawOffset.Y, 0);
            }
            else
            {
                drawMatrix *= Matrix.CreateTranslation(game.DrawOffset.X, game.DrawOffset.Y, 0);
            }
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, drawMatrix);
            spriteBatch.Draw(background, new Rectangle(0, 0, graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight), null, Color.White);
            player.Draw(gameTime, spriteBatch);
            foreach (Projectile p in projectiles)
            {
                p.Draw(gameTime, spriteBatch);
            }
            foreach (Mob m in mobs)
            {
                m.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }

        public override void ChangeControls(Controls controls)
        {
            player.ChangeControls(controls);
            Control = controls;
        }

        public override void Swap()
        {
            player.Swap();
        }
    }
}
