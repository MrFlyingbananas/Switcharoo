using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Switcharoo.Right.Weapons;
namespace Switcharoo.Right
{
    class RightPlayer : Player.Player
    {
        private const int SPEED = 5;
        private const int WIDTH = 39;
        private const int HEIGHT = 44;
        private Vector2 loc;
        private AnimationHandler animHandler;
        private float doubleTapTimer = 0;
        private KeyboardState prevKeyboard;
        private Keys keyTapped = Keys.F24;
        private const int BOOST_DURATION = 20;
        private float boostTimer = BOOST_DURATION + 1;
        private const int DOUBLE_TAP_TIME = 250;
        private const int BOOST_COOLDOWN = 1100;
        private int boostDir = 0;
        private const int BOOST_SPEED = 100;
        private Weapon weapon;
        private const int HIT_DURATION = 1500;
        private const int BLINK_DURATION = 250;
        private float hitTimer = HIT_DURATION;
        public bool IsDead { get; private set; }

        public RightPlayer(Texture2D spriteSheet, Controls controls) : base(spriteSheet, controls)
        {
            AnimationState[] animations = new AnimationState[2];
            animations[0] = new AnimationState(AnimationState.AnimationType.Idle, 1, 0, 4, 100f);
            animations[1] = new AnimationState(AnimationState.AnimationType.Swap, 0, 0, 4, 100f);
            animHandler = new AnimationHandler(animations, animations[0], 37, 47);
            this.controls = controls;
            loc = new Vector2(Game1.WIDTH / 4, 750);
            prevKeyboard = Keyboard.GetState();
            weapon = RightGame.BasicWeapon;
            IsDead = false;
        }
        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(controls.LeftKey) && !prevKeyboard.IsKeyDown(controls.LeftKey))
            {
                if (keyTapped == controls.LeftKey)
                {
                    keyTapped = Keys.F24;
                    if (doubleTapTimer <= DOUBLE_TAP_TIME)
                    {
                        boostDir = -1;
                        if (boostTimer > BOOST_COOLDOWN)
                            boostTimer = 0;
                    }
                }
                else
                {
                    doubleTapTimer = 0;
                    keyTapped = controls.LeftKey;
                }
            }
            if (keyboard.IsKeyDown(controls.RightKey) && !prevKeyboard.IsKeyDown(controls.RightKey))
            {
                if (keyTapped == controls.RightKey)
                {
                    keyTapped = Keys.F24;
                    if (doubleTapTimer <= DOUBLE_TAP_TIME)
                    {
                        boostDir = 1;
                        if(boostTimer > BOOST_COOLDOWN)
                            boostTimer = 0;
                    }
                }
                else
                {
                    doubleTapTimer = 0;
                    keyTapped = controls.RightKey;
                }
            }
            Vector2 dir = Vector2.Zero;
            if(keyboard.IsKeyDown(controls.LeftKey) && keyboard.IsKeyDown(controls.RightKey))
            {
                dir.X = 0;
            }
            else if (keyboard.IsKeyDown(controls.LeftKey))
            {
                dir.X = -1;
            } else if (keyboard.IsKeyDown(controls.RightKey))
            {
                dir.X = 1;
            }
            
            if (keyboard.IsKeyDown(controls.UpKey) && keyboard.IsKeyDown(controls.DownKey))
            {
                dir.Y = 0;
            }
            else if (keyboard.IsKeyDown(controls.UpKey))
            {
                dir.Y = -1;
            }
            else if (keyboard.IsKeyDown(controls.DownKey))
            {
                dir.Y = 1;
            }
            if (keyboard.IsKeyDown(controls.FireKey))
            {
                weapon.Fire((int)loc.X + WIDTH/2 + -weapon.ProjectileWidth/2, (int)loc.Y + weapon.ProjectileHeight);
            }
            if(loc.X + dir.X * SPEED + WIDTH > Game1.WIDTH/2)
            {
                loc.X = Game1.WIDTH / 2 - WIDTH;
            }else if(loc.X + dir.X * SPEED < 0)
            {
                loc.X = 0;
            }
            else
            {
                loc.X += dir.X * SPEED;
            }
            if (loc.Y + dir.Y * SPEED + HEIGHT > Game1.HEIGHT)
            {
                loc.Y = Game1.HEIGHT - HEIGHT;
            }
            else if (loc.Y + dir.Y * SPEED < RightGame.MaxHeight)
            {
                loc.Y = RightGame.MaxHeight;
            }
            else
            {
            loc.Y += dir.Y * SPEED;
            }

            animHandler.Update(gameTime);
            weapon.Update(gameTime);
            doubleTapTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            boostTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            hitTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (boostTimer <= BOOST_DURATION)
            {
                loc.X += boostDir * BOOST_SPEED;
            }
            
            
            prevKeyboard = keyboard;
        }

        public bool Intersect(Rectangle rectangle)
        {
            return new Rectangle((int)loc.X, (int)loc.Y, WIDTH, HEIGHT).Intersects(rectangle);
        }

        public void Hit(int damage)
        {
            if (hitTimer > HIT_DURATION)
            {
                Game1.PlayerHit(damage);
                hitTimer = 0;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Color color = Color.White;
            if (hitTimer < HIT_DURATION)
            {
                color = Color.DarkRed;
            }
            if (hitTimer < HIT_DURATION)
            {
                if (((int)hitTimer/BLINK_DURATION) % 2 != 0)
                {
                    spriteBatch.Draw(spriteSheet, new Rectangle((int)loc.X, (int)loc.Y, WIDTH, HEIGHT), animHandler.SourceRect, color);
                }
            }
            else
            {
                spriteBatch.Draw(spriteSheet, new Rectangle((int)loc.X, (int)loc.Y, WIDTH, HEIGHT), animHandler.SourceRect, color);
            }
        }
        public override void Swap()
        {
            if (animHandler.CurrentAnimation == AnimationState.AnimationType.Idle)
            {
                animHandler.CurrentAnimation = AnimationState.AnimationType.Swap;
            }
            else
            {
                animHandler.CurrentAnimation = AnimationState.AnimationType.Idle;
            }
        }
    }
}
