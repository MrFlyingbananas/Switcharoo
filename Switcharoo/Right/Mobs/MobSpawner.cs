using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Switcharoo.Right.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Switcharoo.Left;

namespace Switcharoo.Right.Mobs
{
    public class MobSpawner
    {
        public enum MobType
        {
            LargeBoss,
            BasicMob,
            ShootMob,
            FastMob,
            SideMob
        }
        static int wave = 0;
        public static Mob LargeBoss { get; private set; }
        public static Mob BasicMob { get; private set; }
        public static Mob BasicShootingMob { get; private set; }
        public static Texture2D basicEnemyTexture, basicShootingTexture, mobBulletTexture;
        private static AnimationState[] noShootAnimations, shootAnimations;
        private static List<WaveLocation> spawnLocations;
        private static Random random;
        private static SoundEffect mobShoot;
        private static SoundEffect bossShoot;
        private static int prevWave = 0;
        private static int WAVE_COOLDOWN = 4000;
        private static float waveTimer = WAVE_COOLDOWN;
        public static bool bossSpawn = false;
        public static int BOSS_SPAWN_DIFF = 6;
        public static void LoadContent(Game1 game)
        {
            mobBulletTexture = game.Content.Load<Texture2D>("Bullet");
            Texture2D tex = game.Content.Load<Texture2D>("BossSpriteSheet");
            AnimationState[] animations = new AnimationState[1];
            animations[0] = new AnimationState(AnimationState.AnimationType.Idle, 0, 0, 4, 100f);
            noShootAnimations = animations;
            shootAnimations = new AnimationState[1];
            shootAnimations[0] = new AnimationState(AnimationState.AnimationType.Idle, 1, 0, 4, 100f);
            BossWeapon[] bossWeapons = new BossWeapon[2];
            SoundEffect sound = game.Content.Load<SoundEffect>("GameSounds/bossshoot");
            bossShoot = sound;
            bossWeapons[0] = new BossWeapon(mobBulletTexture, sound);
            bossWeapons[1] = new BossWeapon(mobBulletTexture, sound);
            Healthbar health = new Healthbar(game.Content.Load<Texture2D>("healthbar"), 50, 10, Game1.WIDTH / 2 - 100, 50);
            LargeBoss = new Boss(health, tex, game.Content.Load<SoundEffect>("GameSounds/bossdefeat"), animations, 88, 89, new Vector2(Game1.WIDTH / 4 - (90 * 4) / 2, -80 * 4), 1, 90 * 4, 80 * 4, bossWeapons);
            basicEnemyTexture = game.Content.Load<Texture2D>("EnemySpriteSheet");
            BasicMob = new BasicMob(basicEnemyTexture, noShootAnimations, 43, 42, new Vector2(0, 0), 5, 45, 40);
            basicShootingTexture = game.Content.Load<Texture2D>("EnemySpriteSheet");
            sound = game.Content.Load<SoundEffect>("GameSounds/playershoot");
            mobShoot = sound;
            Weapon shootWeapon = new MobWeapon(mobBulletTexture, sound);
            BasicShootingMob = new ShootingMob(basicEnemyTexture, noShootAnimations, 43, 42, new Vector2(0, 0), 5, 45, 40, shootWeapon);
            random = new Random();

            spawnLocations = new List<WaveLocation>();

            MobSpawn[] spawns = new MobSpawn[4];
            int spacing = (Game1.WIDTH / 2 - BasicMob.MobWidth * 4) / 4;
            int offset = (Game1.WIDTH / 4 - (spacing * 2) + BasicMob.MobWidth/2);
            for (int i = 0; i < spawns.Length; i++)
            {
                spawns[i] = new MobSpawn(new Vector2(spacing * i + offset, -BasicMob.MobHeight), 0f, MobType.FastMob);
            }
            spawnLocations.Add(new WaveLocation("The Quad", spawns));

            spawns = new MobSpawn[8];
            spacing = BasicMob.MobWidth;
            offset = (Game1.WIDTH / 2 - BasicMob.MobWidth * 4);
            for (int i = 0; i < spawns.Length / 2; i++)
            {
                spawns[i] = new MobSpawn(new Vector2(spacing * i, -BasicMob.MobHeight), 0f, MobType.ShootMob);
            }
            for (int i = spawns.Length / 2; i < spawns.Length; i++)
            {
                spawns[i] = new MobSpawn(new Vector2(spacing * (i-4) + offset, -BasicMob.MobHeight), 0f, MobType.FastMob);
            }
            spawnLocations.Add(new WaveLocation("Double Trouble", spawns));

            spawns = new MobSpawn[4];
            spacing = (Game1.WIDTH / 2 - BasicMob.MobWidth * 4) / 4;
            offset = (Game1.WIDTH / 4 - (spacing * 2) + BasicMob.MobWidth / 2);
            for (int i = 0; i < spawns.Length; i++)
            {
                int yoff = 0;
                if(i != 0 && i != 3)
                {
                    yoff = -BasicMob.MobHeight;
                    spawns[i] = new MobSpawn(new Vector2(spacing * i + offset, -BasicMob.MobHeight + yoff), 0f, MobType.BasicMob);
                }
                else
                {
                    spawns[i] = new MobSpawn(new Vector2(spacing * i + offset, -BasicMob.MobHeight + yoff), 0f, MobType.ShootMob);
                }
            }
            spawnLocations.Add(new WaveLocation("The Quad Offset", spawns));

            spawns = new MobSpawn[(Game1.WIDTH/2)/BasicMob.MobWidth];
            spacing = BasicMob.MobWidth;
            offset = (Game1.WIDTH / 2 - ((Game1.WIDTH / 2) / BasicMob.MobWidth) * BasicMob.MobWidth)/2;
            for (int i = 0; i < spawns.Length; i++)
            {
                if (i % 3 != 0)
                {
                    spawns[i] = new MobSpawn(new Vector2(spacing * i + offset, -BasicMob.MobHeight), 0f, MobType.BasicMob);
                }
                else
                {
                    spawns[i] = new MobSpawn(new Vector2(spacing * i + offset, -BasicMob.MobHeight), 0f, MobType.ShootMob);
                }
            }
            spawnLocations.Add(new WaveLocation("The Wall", spawns));

            spawns = new MobSpawn[((Game1.WIDTH / 2) / BasicMob.MobWidth) - 8];
            spacing = BasicMob.MobWidth;
            offset = (Game1.WIDTH / 2 - ((Game1.WIDTH / 2) / BasicMob.MobWidth) * BasicMob.MobWidth) / 2 + BasicMob.MobWidth*4;
            for (int i = 0; i < spawns.Length; i++)
            {
                spawns[i] = new MobSpawn(new Vector2(spacing * i + offset, -BasicMob.MobHeight), 0f, MobType.ShootMob);
            }
            spawnLocations.Add(new WaveLocation("Firing Squad", spawns));

            spawns = new MobSpawn[4];
            spacing = Game1.WIDTH / 2 - BasicMob.MobWidth;
            offset = 0;
            for (int i = 0; i < spawns.Length / 2; i++)
            {
                spawns[i] = new MobSpawn(new Vector2(spacing * i, -BasicMob.MobHeight), 180f*i, MobType.SideMob);
            }
            for (int i = spawns.Length / 2; i < spawns.Length; i++)
            {
                spawns[i] = new MobSpawn(new Vector2(spacing * (i - 2), -BasicMob.MobHeight*2), 180f * (i-2), MobType.SideMob);
            }
            spawnLocations.Add(new WaveLocation("Sidewinder", spawns));
            
        }
        public static void SpawnBoss()
        {
            bossSpawn = true;
            RightGame.HasMobs = false;
            RightGame.AddMob(LargeBoss);
            RightGame.MaxHeight = LargeBoss.MobHeight + Boss.BOSS_STOP_Y * (3 / 2f);
        }
        public static void Update(GameTime gameTime)
        {
            waveTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (waveTimer > WAVE_COOLDOWN && !bossSpawn)
            {
                waveTimer = 0;
                SpawnWave();
            }
            if(bossSpawn && !RightGame.HasMobs)
            {
                bossSpawn = false;
                LeftGame.justSwitchedBoss = true;
                waveTimer = 0;
            }
        }

        private static void SpawnWave()
        {
            wave++;
            if (Game1.Difficulty == BOSS_SPAWN_DIFF)
            {
                SpawnBoss();
            }
            else
            {
                for (int w = 0; w < 1 + Game1.Difficulty / 2; w++)
                {
                    int rand = random.Next(spawnLocations.Count);
                    while (rand == prevWave)
                    {
                        rand = random.Next(spawnLocations.Count);
                    }
                    WaveLocation waveLoc = spawnLocations.ElementAt(rand);
                    MobSpawn[] spawns = waveLoc.Spawns;
                    for (int i = 0; i < spawns.Length; i++)
                    {
                        SpawnEnemy(spawns[i]);
                    }
                    prevWave = rand;
                }
            }
        }

        private static void SpawnEnemy(MobSpawn spawn)
        {
            MobType type = spawn.Type;
            int x = (int)spawn.Location.X;
            int y = (int)spawn.Location.Y;

            switch (type)
            {
                case MobType.BasicMob:
                    RightGame.AddMob(new BasicMob(basicEnemyTexture, noShootAnimations, 43, 42, new Vector2(x, y), 4, 43, 42));
                break;
                case MobType.LargeBoss:
                    SpawnBoss();
                break;
                case MobType.ShootMob:
                    Weapon shootWeapon = new MobWeapon(mobBulletTexture, mobShoot);
                    RightGame.AddMob(new ShootingMob(basicShootingTexture, shootAnimations, 43, 42, new Vector2(x, y), 4, 43, 42, shootWeapon));
                break;
                case MobType.FastMob:
                    RightGame.AddMob(new BasicMob(basicEnemyTexture, noShootAnimations, 43, 42, new Vector2(x, y), 8, 43, 42));
                break;
                case MobType.SideMob:
                    int flip = 1;
                    if(spawn.Rotation != 0)
                    {
                        flip = -1;
                    }
                    Weapon sideWeapon = new SideWeapon(mobBulletTexture, mobShoot, flip);
                    RightGame.AddMob(new SideMob(basicShootingTexture, shootAnimations, 43, 42, new Vector2(x, y), 4, 43, 42, sideWeapon, flip));
                break;
            }
        }
    }
}
