using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimationType = Switcharoo.AnimationState.AnimationType;
namespace Switcharoo
{
    public class AnimationHandler
    {
        public AnimationType CurrentAnimation
        {
            get => currAnim.AnimType;
            set
            {
                if (value != currAnim.AnimType)
                {
                    AnimationState state = null;
                    foreach (AnimationState anim in animations)
                    {
                        if (anim.AnimType == value)
                        {
                            state = anim;
                            break;
                        }
                    }
                    if (state == null)
                    {
                        throw new Exception("Animation not found!");
                    }
                    else
                    {
                        currAnim = state;
                        animFrame = currAnim.MinFrame;
                        animTimer = 0;
                    }
                }
            }
        }

        private AnimationState[] animations;
        private AnimationState currAnim;
        private int animFrame, frameWidth, frameHeight;
        private float animTimer = 0;
        public AnimationHandler(AnimationState[] animations, AnimationState currAnim, int frameWidth, int frameHeight)
        {
            this.animations = animations;
            this.currAnim = currAnim;
            this.animFrame = currAnim.MinFrame;
            this.frameHeight = frameHeight;
            this.frameWidth = frameWidth;
        }
        public void Update(GameTime gameTime)
        {
            animTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (animTimer > currAnim.FrameInterval)
            {
                animTimer = 0;
                if (++animFrame >= currAnim.MaxFrame)
                {
                    animFrame = currAnim.MinFrame;
                }
            }
        }
        public Rectangle SourceRect
        {
            get => new Rectangle(animFrame * frameWidth, currAnim.AnimRow * frameHeight, frameWidth, frameHeight);
        }
    }
}
