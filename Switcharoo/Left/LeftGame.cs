using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Switcharoo.Player;
using Switcharoo.PowerUps;
using Microsoft.Xna.Framework.Audio;
using Switcharoo.Right.Mobs;

namespace Switcharoo.Left
{
    class LeftGame : SplitGame
    {

        private LeftPlayer player;
        private Texture2D playerTexture;
        private List<LeftBarrier> barriers;
        private Texture2D barrierTexture;
        private Texture2D freezeTexture;
        private Texture2D breakTexture;
        private Texture2D heartTexture;
        private Texture2D[] timers;
        private Component currentTimer;
        private Component freezeLabel;
        private Texture2D freezeLabelTexture;
        private Component smashLabel;
        private Texture2D smashLabelTexture;
        private PowerUp powerUp;
        private PowerUp.PowerUpType powerUpType;
        private int barrierSpeed;
        private int barrierBuffer;
        private int barrierBufferMax;
        private int barrierBufferMem;
        private long gameDuration;
        private int powerUpBuffer;
        private int freezeDuration;
        private int freezeTimer;
        private int breakingDuration;
        private int breakingTimer;
        private int difficulty;
        private int swap;
        private int columnAmt;
        private bool isFrozen;
        private bool isBoss;
        public static bool justSwitchedBoss;
        private bool isBreaking;
        private int clearInterval;
        private int clearAmount;
        private Random powerUpDecider;
        private Texture2D background;
        private float startDelayTimer = 0;
        private SoundEffect freezeCollectSound, smashCollectSound, breakSound;
        public Controls Control
        {
            get;
            private set;
        }
        public LeftGame(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, Game1 game) : base(graphics, spriteBatch, game)
        {
            barrierSpeed = 10;
            barrierBuffer = 0;
            barrierBufferMax = 400;
            gameDuration = 0;
            swap = 0;
            powerUpBuffer = 0;
            freezeDuration = 0;
            breakingDuration = 0;
            freezeTimer = 4;
            breakingTimer = 8;
            clearInterval = 1000;
            clearAmount = 1001;
            justSwitchedBoss = false;
            barrierBufferMem = 0;
            isBoss = MobSpawner.bossSpawn;
            difficulty = Game1.Difficulty;
            isFrozen = false;
            isBreaking = false;
            barriers = new List<LeftBarrier>();
            powerUpDecider = new Random();
            timers = new Texture2D[8];
        }

        public override void LoadContent()
        {
            playerTexture = game.Content.Load<Texture2D>("PlayerSpriteSheet");
            barrierTexture = game.Content.Load<Texture2D>("Barrier");
            freezeTexture = game.Content.Load<Texture2D>("Freeze");
            breakTexture = game.Content.Load<Texture2D>("break");
            heartTexture = game.Content.Load<Texture2D>("Heart");
            freezeLabelTexture = game.Content.Load<Texture2D>("FreezeLabel");
            smashLabelTexture = game.Content.Load<Texture2D>("Smash");
            freezeCollectSound = game.Content.Load<SoundEffect>("GameSounds/freeze");      
            smashCollectSound = game.Content.Load<SoundEffect>("GameSounds/smash");
            breakSound = game.Content.Load<SoundEffect>("GameSounds/breakblock");

            for (int i = 1; i <= timers.Length; i++)
            {
                timers[i-1] = game.Content.Load<Texture2D>(i + "");
            }
            GamePadCapabilities capabilities = GamePad.GetCapabilities(
                                               PlayerIndex.One);
            Controls controls;
                GamePadState state = GamePad.GetState(PlayerIndex.One);
                controls = Game1.leftControls;
                this.Control = controls;
            player = new LeftPlayer(playerTexture, controls, new Vector2(Game1.WIDTH/4, 750));
            background = game.Content.Load<Texture2D>("LeftBG");
            columnAmt = (graphics.PreferredBackBufferWidth / 37) / 5;
        }

        public override void Update(GameTime gameTime)
        {
            isBoss = MobSpawner.bossSpawn;
            startDelayTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            gameDuration += gameTime.ElapsedGameTime.Milliseconds;
            if (gameDuration > 5 * 1000 && barrierBufferMax > 100 && isBoss == false)
            {
                swap = 0;
                if(justSwitchedBoss == true)
                {
                    barrierBufferMax = barrierBufferMem;
                    gameDuration = 0;
                    justSwitchedBoss = false;
                }
                else if(justSwitchedBoss == false)
                {
                    barrierBufferMax -= ((difficulty + 1) * 25);
                    gameDuration = 0;
                }
            }
            else if (isBoss == true)
            {
                if(swap == 0)
                {
                    barrierBufferMem = barrierBufferMax;
                    barrierBufferMax = 100;
                    swap = 1;
                }
            }
            if(startDelayTimer < Game1.PLAY_START_DELAY)
            {
                player.Update(gameTime);
                return;
            }
            player.Update(gameTime);
            if(isFrozen == false && isBreaking == false)
            {
                currentTimer = null;
            }
            if(player.isHit == true && clearAmount < clearInterval)
            {
                clearAmount += gameTime.ElapsedGameTime.Milliseconds;
            }
            if(isFrozen == false)
            {
                barrierBuffer += gameTime.ElapsedGameTime.Milliseconds;
                freezeTimer = 4;
                freezeLabel = null;
            }
            powerUpBuffer += gameTime.ElapsedGameTime.Milliseconds;
            if(isFrozen == true)
            {
                freezeDuration += gameTime.ElapsedGameTime.Milliseconds;
                currentTimer = new Component(timers[freezeTimer - 1], new Vector2(625, 25));
                freezeLabel = new Component(freezeLabelTexture, new Vector2(475, 15));
                if(freezeDuration == 992 || freezeDuration == 2000 || freezeDuration == 3008)
                {
                    freezeTimer -= 1;
                }
            }
            if(isBreaking == true)
            {
                breakingDuration += gameTime.ElapsedGameTime.Milliseconds;
                currentTimer = new Component(timers[breakingTimer - 1], new Vector2(625, 25));
                smashLabel = new Component(smashLabelTexture, new Vector2(475, 15));
                if (breakingDuration == 992 || breakingDuration == 2000 || breakingDuration == 3008 || breakingDuration == 400
                    || breakingDuration == 5008 || breakingDuration == 6000 || breakingDuration == 7008)
                {
                    breakingTimer -= 1;
                }
            }
            else
            {
                breakingTimer = 8;
                smashLabel = null;
            }
            if(freezeDuration > 4000)
            {
                freezeDuration = 0;
                isFrozen = false;
                foreach(LeftBarrier b in barriers)
                {
                    b.setBarrierSpeed(barrierSpeed);
                }
            }
            if(breakingDuration > 8000)
            {
                breakingDuration = 0;
                isBreaking = false;
            }
            if (barrierBuffer >= barrierBufferMax && clearAmount > clearInterval)
            {
                barriers.Add(new LeftBarrier(barrierTexture, new Vector2(0, 0), barrierSpeed, columnAmt));
                barrierBuffer = 0;
            }
            
            for(int i = 0; i < barriers.Count; i++)
            {
                barriers[i].Update(gameTime);
                if (player.hitStatus() == false && player.getRectangle().Intersects(barriers[i].getRectangle()))
                {
                    if(isBreaking == true)
                    {
                        barriers.Remove(barriers[i]);
                        breakSound.Play(Game1.Volume, 0f, 0f);
                    }
                    else
                    {
                        clearAmount = 0;
                        barriers.Clear();
                        player.setHit(true);
                        player.Hit(1);
                    }
                }
            }
            if(powerUpBuffer >= 8000)
            {
                int type = powerUpDecider.Next(0,2);
                switch (type)
                {
                    case 0:
                        powerUp = new PowerUp(freezeTexture, new Vector2(0, 0), PowerUp.PowerUpType.freezeBarriers);
                        break;
                    case 1:
                        powerUp = new PowerUp(breakTexture, new Vector2(0, 0), PowerUp.PowerUpType.breakBarriers);
                        break;
                }
                powerUpBuffer = 0;
            }
            for(int i = 0; i < barriers.Count; i++)
            {
                if(barriers[i].position.Y > 900)
                {
                    barriers.Remove(barriers[i]);
                }
            }
            if (powerUp != null && player.getRectangle().Intersects(powerUp.getRectangle()))
            {
                powerUpType = powerUp.getPowerUpType();
                powerUp = null;
                switch (powerUpType)
                {
                    case PowerUp.PowerUpType.freezeBarriers:
                        isFrozen = true;
                        freezeCollectSound.Play(Game1.Volume, 0f, 0f);
                        foreach(LeftBarrier b in barriers)
                        {
                            b.setBarrierSpeed(0);
                        }
                        break;
                    case PowerUp.PowerUpType.breakBarriers:
                        smashCollectSound.Play(Game1.Volume, 0f, 0f);
                        isBreaking = true;
                        break;
                }
            }
            if (powerUp != null)
            {
                powerUp.Update(gameTime);
                if(powerUp.position.Y > 900)
                {
                    powerUp = null;
                }
            }

        }

        public override void Draw(GameTime gameTime, DrawSide drawSide)
        {
            Matrix drawMatrix = Matrix.Identity;
            drawMatrix *=
            drawMatrix *= Matrix.CreateScale(1, game.CompressFactor, 1);
            if (drawSide == DrawSide.Left)
            {
                drawMatrix *= Matrix.CreateTranslation(game.DrawOffset.X, 0, 0);
            }
            else
            {
                drawMatrix *= Matrix.CreateTranslation(offset.X - game.DrawOffset.X, 0, 0);
            }
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, drawMatrix);
            spriteBatch.Draw(background, new Rectangle(0, 0, graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight), null, Color.White);
            player.Draw(gameTime, spriteBatch);
            for(int i = 0; i < barriers.Count; i++)
            {
                barriers[i].Draw(gameTime, spriteBatch);
            }
            if(powerUp != null)
            {
                powerUp.Draw(gameTime, spriteBatch);
            }
            if(currentTimer != null)
            {
                currentTimer.Draw(gameTime, spriteBatch);
            }
            if(smashLabel != null)
            {
                smashLabel.Draw(gameTime, spriteBatch);
            }
            if(freezeLabel != null)
            {
                freezeLabel.Draw(gameTime, spriteBatch);
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
