using Microsoft.Xna.Framework;
using static Switcharoo.Right.Mobs.MobSpawner;

namespace Switcharoo.Right.Mobs
{
    public class MobSpawn
    {
        public Vector2 Location { get; private set; }
        public MobType Type { get; private set; }
        public float Rotation { get; private set; }
        public MobSpawn(Vector2 loc, float rotation, MobType type)
        {
            this.Location = loc;
            this.Type = type;
            this.Rotation = rotation;
        }
    }
}