using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Switcharoo.Player;
using Microsoft.Xna.Framework.Input;

namespace Switcharoo.Left
{

    class LeftPlayer : Player.Player
    {

        private Vector2 position;
        private Texture2D playerTexture;
        public bool isHit;
        private Rectangle playerArea;
        private int invulnerableBuffer;
        private const int HIT_DURATION = 100;
        private float hitTimer = HIT_DURATION;
        private const int TIMES_TO_BUFFER = 3;
        private const int BUFFER_INTERVAL = 1000;
        private int currentBufferIndex;
        private AnimationHandler animHandler;
        public LeftPlayer(Texture2D spriteSheet, Controls controls, Vector2 position) : base(spriteSheet, controls)
        {

            AnimationState[] animations = new AnimationState[2];
            animations[0] = new AnimationState(AnimationState.AnimationType.Idle, 0, 0, 4, 100f);
            animations[1] = new AnimationState(AnimationState.AnimationType.Swap, 1, 0, 4, 100f);
            animHandler = new AnimationHandler(animations, animations[0], 37, 47);
            this.position = position;
            playerTexture = spriteSheet;

            playerArea = new Rectangle((int)position.X, (int)position.Y, playerTexture.Width, playerTexture.Height);
            isHit = false;
            invulnerableBuffer = 0;
            currentBufferIndex = 1;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (isHit == true)
            {
                if(invulnerableBuffer > 250 && invulnerableBuffer < 750)
                {

                }
                else
                {
                    spriteBatch.Draw(playerTexture, position, animHandler.SourceRect, Color.Red);
                }
            }
            else
            {
                spriteBatch.Draw(playerTexture, position, animHandler.SourceRect, Color.White);
            }
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(controls.LeftKey) && position.X > 0)
            {
                position.X -= 10;
            }
            else if (keyboard.IsKeyDown(controls.RightKey) && position.X < 636)
            {
                position.X += 10;
            }
            if(isHit == true && currentBufferIndex <= TIMES_TO_BUFFER)
            {
                if(invulnerableBuffer < BUFFER_INTERVAL)
                {
                    invulnerableBuffer += gameTime.ElapsedGameTime.Milliseconds;
                }else if(invulnerableBuffer > BUFFER_INTERVAL)
                {
                    invulnerableBuffer = 0;
                    currentBufferIndex += 1;
                }
                    
            }else if(currentBufferIndex > TIMES_TO_BUFFER)
            {
                isHit = false;
                invulnerableBuffer = 0;
                currentBufferIndex = 1;
            }
            playerArea = new Rectangle((int)position.X, (int)position.Y, 37, 47);
            hitTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            animHandler.Update(gameTime);
        }

        public Rectangle getRectangle()
        {
            return playerArea;
        }

        public bool hitStatus()
        {
            return isHit;
        }

        public void setHit(bool hit)
        {
            isHit = hit;
        }

        public void Hit(int damage)
        {
            if (hitTimer > HIT_DURATION)
            {
                Game1.PlayerHit(damage);
                hitTimer = 0;
            }
        }

        public override void Swap()
        {
            if(animHandler.CurrentAnimation == AnimationState.AnimationType.Idle)
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
