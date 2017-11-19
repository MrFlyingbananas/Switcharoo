using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Switcharoo.Right.Mobs
{
    class BasicMob : Mob
    {
        const int MAX_HEALTH = 15;
        public BasicMob(Texture2D spriteSheet, AnimationState[] animations, int frameWidth, int frameHeight, Vector2 loc, int speed, int mobWidth, int mobHeight) : base(spriteSheet, animations, frameWidth, frameHeight, loc, speed, mobWidth, mobHeight, MAX_HEALTH)
        {
        }
        public override void Update(GameTime gameTime)
        {
            loc.Y += speed;
            base.Update(gameTime);
        }
    }
}
