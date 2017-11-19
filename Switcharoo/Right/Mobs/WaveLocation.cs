using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using static Switcharoo.Right.Mobs.MobSpawner;

namespace Switcharoo.Right.Mobs
{
    class WaveLocation
    {
        private string waveName;
        private Vector2[] locs;
        MobSpawn[] spawns;
        public MobSpawn[] Spawns {get {return spawns;} }
        public WaveLocation(string waveName, MobSpawn[] spawns)
        {
            this.waveName = waveName;
            this.spawns = spawns;
        }
    }
}
