using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Switcharoo.Right;
using Switcharoo.Player;
using Switcharoo.Left;
using Switcharoo.UI;
using Microsoft.Xna.Framework.Audio;
using Switcharoo.Right.Mobs;
using System.Collections.Generic;

namespace Switcharoo
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>\
    /// 

    public class Game1 : Game
    {
        public enum GameState
        {
            MainMenu,
            InstructionMenu,
            PauseMenu,
            Game,
            GameOver,
            VictoryMenu
        }
        public enum Aroo
        {
            Switch,
            Swap,
            Reverse
        }
        public static Controls leftControls = new Controls(Keys.A, Keys.D, Keys.W, Keys.S, Keys.Space);
        public static Controls rightControls = new Controls(Keys.J, Keys.L, Keys.I, Keys.K, Keys.Space);
        //public static Controls leftControllerControls = new Controls();
        public const int WIDTH = 1400;
        public const int HEIGHT = 900;
        GraphicsDeviceManager graphics;

        SpriteBatch spriteBatch;
        RightGame right;
        LeftGame left;
        Texture2D title;
        Texture2D gameOverBG1;
        private int bg1X;
        private int bg2X;
        static HealthPool hp;
        bool swapping = false;
        int swap = 1;
        float timer = 0;
        private Vector2 drawOffset = Vector2.Zero;
        private float compress = 1;
        private const float MAX_COMPRESS = .33f;
        private bool compressed = false;
        private bool depressed = false;
        public float CompressFactor { get { return compress; } }
        public Vector2 DrawOffset { get { return drawOffset; } }
        private Vector2 winLoc;
        private int gameDone = 0;
        Texture2D switchTexture, switchTexturePrime;
        Texture2D reverseTexture;
        private Texture2D swapTexture;
        SpriteFont timerFont, gameOverFont;
        private Vector2 gameOverMes;
        private int SWITCH_TIME_SECONDS;
        private SoundEffect switcharooSound;
        private GameState gState;
        private Texture2D fade;
        private Texture2D progress;
        private Texture2D emptyProgressCirle, filledProgressCircle;
        private Texture2D victoryExplosion;
        private List<Vector3> victoryExplosions;
        private SoundEffect explosionSound;
        private GameState gameState
        {
            get { return gState; }
            set
            {
                switch (value)
                {
                    case GameState.MainMenu:
                        this.IsMouseVisible = true;
                        break;
                    case GameState.PauseMenu:
                        this.IsMouseVisible = true;
                        break;
                    case GameState.Game:
                        this.IsMouseVisible = false;
                        if(gState == GameState.MainMenu || gState == GameState.InstructionMenu || gState == GameState.GameOver || gState == GameState.VictoryMenu)
                        {
                            switch (random.Next(3))
                            {
                                case 0:
                                    aroo = Aroo.Switch;
                                    break;
                                case 1:
                                    aroo = Aroo.Swap;
                                    break;
                                case 2:
                                    aroo = Aroo.Reverse;
                                    break;
                            }
                            right = new RightGame(graphics, spriteBatch, this);
                            right.LoadContent();
                            left = new LeftGame(graphics, spriteBatch, this);
                            left.LoadContent();
                            hp = new HealthPool(Content.Load<Texture2D>("Heart"), Content.Load<SoundEffect>("GameSounds/playerdamage"), this, 42, 36, 10);
                            progressTimer = 0;
                            counter = 0;
                            swapping = false;
                            swap = 1;
                            timer = 0;
                            drawOffset = Vector2.Zero;
                            compress = 1;
                            compressed = false;
                            depressed = false;
                            swappingTimer = 0;
                            waiting = false;
                            startDelayTimer = 0;
                            SWITCH_TIME_SECONDS = 20;
                        }
                        break;
                    case GameState.GameOver:
                        this.IsMouseVisible = true;
                        gameOverTimer = 0;
                        break;
                    case GameState.InstructionMenu:
                        this.IsMouseVisible = true;
                        break;
                    case GameState.VictoryMenu:
                        this.IsMouseVisible = true;
                        gameOverTimer = -5000;
                        explosionTimer = 0;
                        victoryExplosions = new List<Vector3>();
                        break;
                }
                gState = value;
            }
        }
        private Button mainMenuPlay;
        private Button mainMenuExit;
        private Button pauseMenuResume;
        private Button pauseMenuMainMenu;
        private Button mainMenuInstructions;
        private Button instructionMainMenu;
        private Button gameOverExit;
        private Button gameOverMainMenu;
        private Button gameOverPlayAgain;
        private float startDelayTimer = 0;
        public const float PLAY_START_DELAY = 3000;
        public Random random;
        private Aroo aroo;
        private int SWAPPING_DURATION = 2000;
        private float progressTimer;
        private float swappingTimer = 0;
        private bool waiting = false;
        private Texture2D pause, instructions, bossMarker;
        private Texture2D[] countDown;
        private SoundEffect gameOverSound;
        private float gameOverTimer;
        private const int FADE_DURATION = 3000;
        private static int counter;
        public static int Difficulty { get { return counter; } }
        public static float Volume { get; set; }

        private float explosionTimer;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = WIDTH,
                PreferredBackBufferHeight = HEIGHT
            };
            Content.RootDirectory = "Content";
            Window.AllowUserResizing = false;
            this.IsMouseVisible = true;
            this.Window.Position = new Point(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2 - WIDTH / 2, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2 - HEIGHT / 2 - 50);
            gameState = GameState.MainMenu;
            gameDone = 0;
            random = new Random();
            int rand = random.Next(3);
            Volume = .4f;
            switch (rand)
            {
                case 0:
                    aroo = Aroo.Switch;
                    break;
                case 1:
                    aroo = Aroo.Swap;
                    break;
                case 2:
                    aroo = Aroo.Reverse;
                    break;
            }
            bg1X = 0;
            bg2X = 0;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            right = new RightGame(graphics, spriteBatch, this);
            right.LoadContent();
            left = new LeftGame(graphics, spriteBatch, this);
            left.LoadContent();
            title = Content.Load<Texture2D>("Title");
            hp = new HealthPool(Content.Load<Texture2D>("Heart"), Content.Load<SoundEffect>("GameSounds/playerdamage"), this, 42, 36, 10);
            switchTexture = Content.Load<Texture2D>("Switcharoo");
            switchTexturePrime = Content.Load<Texture2D>("SwitcharooSwitch");
            reverseTexture = Content.Load<Texture2D>("Reversearoo");
            swapTexture = Content.Load<Texture2D>("Swaparoo");
            timerFont = Content.Load<SpriteFont>("timerSpritefont");
            fade = Content.Load<Texture2D>("Fade");
            pause = Content.Load<Texture2D>("Pause");
            progress = Content.Load<Texture2D>("Progress");
            emptyProgressCirle = Content.Load<Texture2D>("UnfilledCircle");
            filledProgressCircle = Content.Load<Texture2D>("FilledCircle");
            explosionSound = Content.Load<SoundEffect>("GameSounds/Explosion");
            bossMarker = Content.Load<Texture2D>("BossLabel");
            gameOverBG1 = Content.Load<Texture2D>("GameOverBG");
            countDown = new Texture2D[3];
            countDown[0] = Content.Load<Texture2D>("1");
            countDown[1] = Content.Load<Texture2D>("2");
            countDown[2] = Content.Load<Texture2D>("3");
            Texture2D buttonTexture = Content.Load<Texture2D>("Button");
            SpriteFont buttonFont = Content.Load<SpriteFont>("ButtonFont");
            gameOverFont = Content.Load<SpriteFont>("GameOverSpritefont");
            gameOverMes = gameOverFont.MeasureString("Game Over");
            instructions = Content.Load<Texture2D>("Instructions");
            SoundEffect buttonClick = Content.Load<SoundEffect>("GameSounds/click");
            victoryExplosion = Content.Load<Texture2D>("DeadBoss");
            mainMenuPlay = new Button(buttonTexture, WIDTH / 2 - 400 / 2, 325, 400, 150, buttonClick, buttonFont, "Play");
            mainMenuPlay.OnClick += MainMenuPlayClick;
            mainMenuExit = new Button(buttonTexture, WIDTH / 2 - 400 / 2, 675, 400, 150, buttonClick, buttonFont, "Exit");
            mainMenuExit.OnClick += () => { Exit(); };
            mainMenuInstructions = new Button(buttonTexture, WIDTH / 2 - 400 / 2, 500, 400, 150, buttonClick, buttonFont, "Instructions");
            mainMenuInstructions.OnClick += () => { gameState = GameState.InstructionMenu;  };
            pauseMenuResume = new Button(buttonTexture, WIDTH / 2 - 400 / 2, 325, 400, 200, buttonClick, buttonFont, "Play");
            pauseMenuResume.OnClick += PauseMenuResumeClick; 
            pauseMenuMainMenu = new Button(buttonTexture, WIDTH / 2 - 400 / 2, 600, 400, 200, buttonClick, buttonFont, "Main Menu");
            pauseMenuMainMenu.OnClick += () => { gameState = GameState.MainMenu; };
            instructionMainMenu = new Button(buttonTexture, WIDTH -315, HEIGHT - 140, 250, 100, buttonClick, buttonFont, "Main Menu");
            instructionMainMenu.OnClick += () => { gameState = GameState.MainMenu; };
            gameOverExit = new Button(buttonTexture, WIDTH - 300 - 50, HEIGHT - 150 - 50, 300, 150, buttonClick, buttonFont, "Exit");
            gameOverExit.OnClick += () => { Exit(); };
            gameOverPlayAgain = new Button(buttonTexture, WIDTH/2 - 300/2, HEIGHT - 150 - 50, 300, 150, buttonClick, buttonFont, "Play Again");
            gameOverPlayAgain.OnClick += () => { gameState = GameState.Game; };
            gameOverMainMenu = new Button(buttonTexture, 100 / 2, HEIGHT - 150 - 50, 300, 150, buttonClick, buttonFont, "Main Menu");
            gameOverMainMenu.OnClick += () => { gameState = GameState.MainMenu; };
            switcharooSound = Content.Load<SoundEffect>("GameSounds/switcharoo");
            gameOverSound = Content.Load<SoundEffect>("GameSounds/lose");
        }

        private void PauseMenuResumeClick()
        {
            gameState = GameState.Game;
        }

        private void MainMenuPlayClick()
        {
            gameState = GameState.Game;
        }
        
        protected override void UnloadContent()
        {
        }
        
        protected override void Update(GameTime gameTime)
        {
            switch (gameState)
            {
                case GameState.MainMenu:
                    UpdateMainMenu(gameTime);
                    break;
                case GameState.PauseMenu:
                    UpdatePauseMenu(gameTime);
                    break;
                case GameState.Game:
                    startDelayTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    gameDone = 0;
                    if (startDelayTimer < PLAY_START_DELAY)
                    {
                        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                            gameState = GameState.PauseMenu;
                        left.Update(gameTime);
                        right.Update(gameTime);
                    }
                    else
                    {
                        UpdateGame(gameTime);
                    }
                    break;
                case GameState.GameOver:
                    UpdateGameOver(gameTime);
                    break;
                case GameState.InstructionMenu:
                    UpdateInstructionMenu(gameTime);
                    break;
                case GameState.VictoryMenu:
                    UpdateVictoryMenu(gameTime);
                    break;
            }

        }

        private void UpdateVictoryMenu(GameTime gameTime)
        {
            gameOverTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            explosionTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if(gameOverTimer < 0 && explosionTimer > 750)
            {
                explosionTimer = 0;
                int randWidth = random.Next(0, 500);
                int randHeight = random.Next(0, 500);
                int pitch = random.Next(11);
                if(pitch > 10)
                {
                    pitch = 10;
                }
                victoryExplosions.Add(new Vector3(winLoc.X + randWidth, winLoc.Y + randHeight, 1));
                explosionSound.Play(Volume, -pitch / 10f, 0f);
            }
            for(int i = 0; i < victoryExplosions.Count; i++)
            {
                Vector3 exp = victoryExplosions[i];
                victoryExplosions[i] = new Vector3(exp.X, exp.Y, exp.Z - .02f);
                if(exp.Z <= 0)
                {
                    exp.Z = 0;
                }
            }
            gameOverExit.Update(gameTime);
            gameOverMainMenu.Update(gameTime);
            gameOverPlayAgain.Update(gameTime);
        }

        private void UpdateGameOver(GameTime gameTime)
        {
            gameOverTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            gameOverExit.Update(gameTime);
            gameOverMainMenu.Update(gameTime);
            gameOverPlayAgain.Update(gameTime);
        }
        private void UpdateInstructionMenu(GameTime gameTime)
        {
            instructionMainMenu.Update(gameTime);
        }

        private void UpdatePauseMenu(GameTime gameTime)
        {
            pauseMenuResume.Update(gameTime);
            pauseMenuMainMenu.Update(gameTime);
        }

        private void UpdateMainMenu(GameTime gameTime)
        {
            mainMenuPlay.Update(gameTime);
            mainMenuExit.Update(gameTime);
            mainMenuInstructions.Update(gameTime);
        }

        private void UpdateGame(GameTime gameTime)
        {
            if (MobSpawner.bossSpawn)
                SWITCH_TIME_SECONDS = 10;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                gameState = GameState.PauseMenu;
            if (!swapping && !waiting)
            {
                left.Update(gameTime);
                right.Update(gameTime);
                hp.Update(gameTime);
                // TODO: Add your update logic here
                if (timer > SWITCH_TIME_SECONDS * 1000 && !swapping)
                {
                    swapping = true;
                    counter++;
                    switcharooSound.Play(Game1.Volume, -1f, 0f);
                }
                progressTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                base.Update(gameTime);
            }
            else if (swapping && !waiting)
            {
                switch (aroo)
                {
                    case Aroo.Switch:
                        if (drawOffset.Y >= HEIGHT - MAX_COMPRESS * HEIGHT && !compressed)
                        {
                            compressed = true;
                            drawOffset.Y = HEIGHT - MAX_COMPRESS * HEIGHT;
                        }
                        else if (!compressed && !depressed)
                        {
                            drawOffset.Y += 6;
                            compress -= (6f / (HEIGHT - MAX_COMPRESS * HEIGHT)) * (1 - MAX_COMPRESS);
                        }
                        if (compressed && !depressed)
                        {
                            drawOffset.X += 6;
                            if (drawOffset.X >= Game1.WIDTH / 2)
                            {
                                drawOffset.X = WIDTH / 2;
                                depressed = true;
                            }
                        }
                        if (compressed && depressed)
                        {
                            drawOffset.Y -= 6;
                            compress += (6f / (HEIGHT - MAX_COMPRESS * HEIGHT)) * (1 - MAX_COMPRESS);
                            if (compress >= 1)
                            {
                                compress = 1;
                                depressed = false;
                                compressed = false;
                                timer = 0;
                                swap = -swap;
                                drawOffset = Vector2.Zero;
                                swapping = false;
                                NewAroo();
                            }
                        }
                        break;
                    case Aroo.Swap:
                        Controls temp = left.Control;
                        left.ChangeControls(right.Control);
                        left.Swap();
                        right.ChangeControls(temp);
                        right.Swap();
                        waiting = true;
                        swapping = false;
                        break;
                    case Aroo.Reverse:
                        Controls lc = left.Control;
                        Controls rc = right.Control;
                        left.ChangeControls(new Controls(lc.RightKey, lc.LeftKey, lc.DownKey, lc.UpKey, lc.FireKey));
                        right.ChangeControls(new Controls(rc.RightKey, rc.LeftKey, rc.DownKey, rc.UpKey, rc.FireKey));
                        waiting = true;
                        swapping = false;
                        break;
                }
            }
            else if (waiting)
            {
                swappingTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if(swappingTimer > SWAPPING_DURATION)
                {
                    timer = 0;
                    waiting = false;
                    swappingTimer = 0;
                    NewAroo();
                }
            }
        }

        private void NewAroo()
        {
            int rand = random.Next(3);
            switch (rand)
            {
                case 0:
                    aroo = Aroo.Switch;
                    break;
                case 1:
                    aroo = Aroo.Swap;
                    break;
                case 2:
                    aroo = Aroo.Reverse;
                    break;
            }
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            switch (gameState)
            {
                case GameState.MainMenu:
                    DrawMainMenu(gameTime);
                    break;
                case GameState.PauseMenu:
                    DrawPauseMenu(gameTime);
                    break;
                case GameState.Game:
                    DrawGame(gameTime);
                    break;
                case GameState.GameOver:
                    DrawGameOver(gameTime);
                    break;
                case GameState.InstructionMenu:
                    DrawInstructionMenu(gameTime);
                    break;
                case GameState.VictoryMenu:
                    DrawVictoryMenu(gameTime);
                    break;
            }

            base.Draw(gameTime);
        }

        private void DrawVictoryMenu(GameTime gameTime)
        {
            if (gameOverTimer < 0)
            {
                DrawGame(gameTime);
                DrawExplosions(gameTime);
            }
            if (gameOverTimer <= FADE_DURATION && gameOverTimer >= 0)
            {
                DrawGame(gameTime);
                spriteBatch.Begin();
                spriteBatch.Draw(fade, new Rectangle(0, 0, WIDTH, HEIGHT), new Color(Color.Black, gameOverTimer / FADE_DURATION));
            }
            else if(gameOverTimer >= 0)
            {
                spriteBatch.Begin();
                if (gameDone == 0)
                {
                    gameDone = (int)gameTime.TotalGameTime.TotalSeconds;
                }
                if (bg1X >= 0 && bg1X < gameOverBG1.Width)
                {
                    bg1X += 1;
                    bg2X = bg1X - gameOverBG1.Width;
                    spriteBatch.Draw(gameOverBG1, new Vector2(bg1X, 0), Color.White);
                    spriteBatch.Draw(gameOverBG1, new Vector2(bg2X, 0), Color.White);
                }
                else if (bg2X >= 0)
                {
                    bg2X += 1;
                    bg1X = bg2X - gameOverBG1.Width;
                    spriteBatch.Draw(gameOverBG1, new Vector2(bg1X, 0), Color.White);
                    spriteBatch.Draw(gameOverBG1, new Vector2(bg2X, 0), Color.White);
                }
                spriteBatch.DrawString(gameOverFont, "Victory!", new Vector2(WIDTH / 2 - gameOverMes.X / 2, 150), Color.Yellow);
                Vector2 mes = gameOverFont.MeasureString("You survived the Switcharoo Boogaloo!");
                spriteBatch.DrawString(gameOverFont, ("You survived the Switcharoo Boogaloo!"), new Vector2(WIDTH / 2 - mes.X / 2, 400), Color.Aquamarine);
                gameOverExit.Draw(gameTime, spriteBatch);
                gameOverMainMenu.Draw(gameTime, spriteBatch);
                gameOverPlayAgain.Draw(gameTime, spriteBatch);
                if (gameOverTimer <= FADE_DURATION * 2)
                {
                    spriteBatch.Draw(fade, new Rectangle(0, 0, WIDTH, HEIGHT), new Color(Color.Black, 1 - (gameOverTimer - FADE_DURATION) / FADE_DURATION));
                }
            }
            spriteBatch.End();
        }

        private void DrawExplosions(GameTime gameTime)
        {
            foreach(Vector3 exp in victoryExplosions)
            {
                if(exp.Z > 0)
                {
                    Matrix drawMatrix = Matrix.Identity;
                    drawMatrix *= Matrix.CreateScale(exp.Z, exp.Z, 1);
                    drawMatrix *= Matrix.CreateTranslation(exp.X, exp.Y, 0);
                    spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, drawMatrix);
                    spriteBatch.Draw(victoryExplosion, new Rectangle(0, 0, 100, 100), Color.White);
                    spriteBatch.End();
                }
            }
        }

        private void DrawGameOver(GameTime gameTime)
        {
            if (gameOverTimer <= FADE_DURATION)
            {
                DrawGame(gameTime);
                spriteBatch.Begin();
                spriteBatch.Draw(fade, new Rectangle(0, 0, WIDTH, HEIGHT), new Color(Color.Black, gameOverTimer / FADE_DURATION));
            }
            else
            {
                if(gameDone == 0)
                {
                    gameDone = (int) gameTime.TotalGameTime.TotalSeconds;
                }
                spriteBatch.Begin();
                if(bg1X >= 0 && bg1X < gameOverBG1.Width)
                {
                    bg1X += 1;
                    bg2X = bg1X - gameOverBG1.Width;
                    spriteBatch.Draw(gameOverBG1, new Vector2(bg1X, 0), Color.White);
                    spriteBatch.Draw(gameOverBG1, new Vector2(bg2X, 0), Color.White);
                }
                else if(bg2X >= 0)
                {
                    bg2X += 1;
                    bg1X = bg2X - gameOverBG1.Width;
                    spriteBatch.Draw(gameOverBG1, new Vector2(bg1X, 0), Color.White);
                    spriteBatch.Draw(gameOverBG1, new Vector2(bg2X, 0), Color.White);
                }
                spriteBatch.DrawString(gameOverFont, "Game Over", new Vector2(WIDTH / 2 - gameOverMes.X / 2, 150), Color.Red);
                Vector2 mes = gameOverFont.MeasureString("You lasted " + gameDone + " seconds.");
                spriteBatch.DrawString(gameOverFont, ("You lasted " + gameDone + " seconds."), new Vector2(WIDTH / 2 - mes.X / 2, 400), Color.Red);
                gameOverExit.Draw(gameTime, spriteBatch);
                gameOverMainMenu.Draw(gameTime, spriteBatch);
                gameOverPlayAgain.Draw(gameTime, spriteBatch);
                if (gameOverTimer <= FADE_DURATION * 2)
                {
                    spriteBatch.Draw(fade, new Rectangle(0,0,WIDTH, HEIGHT), new Color(Color.Black, 1- (gameOverTimer - FADE_DURATION) / FADE_DURATION));
                }
            }
            spriteBatch.End();
        }

        private void DrawInstructionMenu(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(instructions, new Rectangle(0, 0, WIDTH, HEIGHT), Color.White);
            instructionMainMenu.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }

        private void DrawPauseMenu(GameTime gameTime)
        {
            DrawGame(gameTime);
            spriteBatch.Begin();
            spriteBatch.Draw(pause, new Rectangle(0, 0, WIDTH, HEIGHT), Color.White);
            pauseMenuResume.Draw(gameTime, spriteBatch);
            pauseMenuMainMenu.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }

        private void DrawMainMenu(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(title, new Rectangle(0, 0, WIDTH, HEIGHT), Color.White);
            mainMenuPlay.Draw(gameTime, spriteBatch);
            mainMenuExit.Draw(gameTime, spriteBatch);
            mainMenuInstructions.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }

        private void DrawGame(GameTime gameTime)
        {
            if (swap == -1)
            {
                right.Draw(gameTime, SplitGame.DrawSide.Left);
                left.Draw(gameTime, SplitGame.DrawSide.Right);
            }
            else
            {
                right.Draw(gameTime, SplitGame.DrawSide.Right);
                left.Draw(gameTime, SplitGame.DrawSide.Left);
            }
            spriteBatch.Begin();
            spriteBatch.Draw(progress, new Rectangle(WIDTH / 2 - 10 / 2, 0, 10, HEIGHT), Color.Blue);
            float res = progressTimer / 1000;
            if (res < MobSpawner.BOSS_SPAWN_DIFF * SWITCH_TIME_SECONDS && !MobSpawner.bossSpawn)
            {
                spriteBatch.Draw(progress, new Rectangle(WIDTH / 2 - 10 / 2, 0, 10, HEIGHT - (int)((res * HEIGHT) / (MobSpawner.BOSS_SPAWN_DIFF * SWITCH_TIME_SECONDS))), Color.Black);
            }
            int pro = HEIGHT - (int)((res * HEIGHT) / (MobSpawner.BOSS_SPAWN_DIFF * SWITCH_TIME_SECONDS));
            int circleNum = MobSpawner.BOSS_SPAWN_DIFF - 1;
            int spacing = (HEIGHT - circleNum * 32) / circleNum;
            for (int i = 0; i < circleNum; i++)
            {
                int circleHeight = spacing * (i + 1) + (i * 2);
                if (pro <= circleHeight + 1)
                {
                    if (swap == -1)
                    {
                        spriteBatch.Draw(filledProgressCircle, new Rectangle(WIDTH / 2 - 32 / 2, circleHeight, 32, 32), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                    }
                    else
                    {
                        spriteBatch.Draw(filledProgressCircle, new Rectangle(WIDTH / 2 - 32 / 2, circleHeight, 32, 32), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0);
                    }
                }
                else
                {
                    if (swap == -1)
                    {
                        spriteBatch.Draw(emptyProgressCirle, new Rectangle(WIDTH / 2 - 32 / 2, circleHeight, 32, 32), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                    }
                    else
                    {
                        spriteBatch.Draw(emptyProgressCirle, new Rectangle(WIDTH / 2 - 32 / 2, circleHeight, 32, 32), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0);
                    }
                }
            }
            spriteBatch.Draw(bossMarker, new Rectangle(WIDTH / 2 - 100 / 2, 0, 100, 50), Color.White);
            if (startDelayTimer < PLAY_START_DELAY)
            {
                spriteBatch.Draw(countDown[2 - (int)(startDelayTimer / 1000)], new Rectangle(WIDTH / 2 - 100 / 2, HEIGHT / 2 - 100 / 2, 100, 100), Color.White);
            }
            

            if (!swapping && !waiting)
            {
                String arooString = "Switcharoo!";
                switch (aroo)
                {
                    case Aroo.Reverse:
                        arooString = "Reversearoo!";
                        break;
                    case Aroo.Swap:
                        arooString = "Swaparoo!";
                        break;
                    case Aroo.Switch:
                        arooString = "Switcharoo!";
                        break;
                }
                Vector2 mes = timerFont.MeasureString((SWITCH_TIME_SECONDS - (int)timer / 1000) + " Seconds till ");
                Vector2 mes2 = timerFont.MeasureString(arooString);
                Color textColor = Color.Black;
                Color otherSide = Color.White;
                if (swap == -1)
                {
                    textColor = Color.White;
                    otherSide = Color.Black;
                }
                spriteBatch.DrawString(timerFont, (SWITCH_TIME_SECONDS - (int)timer / 1000) + " Seconds till", new Vector2(WIDTH / 2 + -mes.X - 5, HEIGHT - hp.Height - mes.Y), textColor);
                spriteBatch.DrawString(timerFont, arooString, new Vector2(WIDTH / 2 + 15, HEIGHT - hp.Height - mes2.Y - 2), otherSide);
                if ((SWITCH_TIME_SECONDS - (int)timer / 1000) <= 3 && (SWITCH_TIME_SECONDS - (int)timer / 1000) >= 1)
                {
                    spriteBatch.Draw(countDown[(SWITCH_TIME_SECONDS - (int)timer / 1000) - 1], new Rectangle(WIDTH / 2 - 100 / 2, HEIGHT / 2 - 100 / 2, 100, 100), Color.White);
                }
                hp.Draw(gameTime, spriteBatch);
            }else if (swapping || waiting)
            {
                switch (aroo)
                {
                    case Aroo.Reverse:
                        spriteBatch.Draw(reverseTexture, new Rectangle(300, 300, Game1.WIDTH - 600, Game1.HEIGHT - 600), Color.White);
                        break;
                    case Aroo.Swap:
                        spriteBatch.Draw(swapTexture, new Rectangle(300, 300, Game1.WIDTH - 600, Game1.HEIGHT - 600), Color.White);
                        break;
                    case Aroo.Switch:
                        if (swap == -1)
                        {
                            spriteBatch.Draw(switchTexturePrime, new Rectangle(300, 300, Game1.WIDTH - 600, Game1.HEIGHT - 600), Color.White);

                        }
                        else
                        {
                            spriteBatch.Draw(switchTexture, new Rectangle(300, 300, Game1.WIDTH - 600, Game1.HEIGHT - 600), Color.White);

                        }
                        break;
                }

            }

            spriteBatch.End();
        }

        public static void PlayerHit(int damage)
        {
            hp.Hit(damage);
        }
        public void EndGame()
        {
            gameState = GameState.GameOver;
            gameOverSound.Play(Volume, 0f, 0f);
        }

        public void Victory(Vector2 loc)
        {
            gameState = GameState.VictoryMenu;
            winLoc = loc;
        }

    }
}
