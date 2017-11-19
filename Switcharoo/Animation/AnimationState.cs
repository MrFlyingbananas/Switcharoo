
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Switcharoo
{
    public class AnimationState
    {
        AnimationType animType;
        int animRow, minFrame, maxFrame;
        float frameInterval;
        public enum AnimationType
        {
            MovingLeft,
            MovingRight,
            MovingUp,
            MovingIdle,
            Idle, 
            Swap
        }
        public AnimationState(AnimationType type, int animationRow, int minFrame, int maxFrame, float frameInterval)
        {
            this.animType = type;
            this.animRow = animationRow;
            this.minFrame = minFrame;
            this.maxFrame = maxFrame;
            this.frameInterval = frameInterval;
        }

        public AnimationType AnimType { get => animType; }
        public int AnimRow { get => animRow; }
        public int MinFrame { get => minFrame; }
        public int MaxFrame { get => maxFrame; }
        public float FrameInterval { get => frameInterval; }
    }
}
