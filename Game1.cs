//AndrewB50
//Frogger
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Audio;

namespace Frogger
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        const int LANES = 15; 		//horizontal lanes on the screen
        const int START_LANE = 13;  	// where the frog starts
        const int END_LANE = 1;     	// where the frog ends
        const int CAR_ROWS = 10;
        const int TIMER_ROW = 14;
        const int SCORE_ROW = 0;
        const int LIVES_LEFT_ROW = 0;
        const int FROG_LIVES = 5;
        const int TIME_TO_GET_HOME = 30; 	// seconds
        const int TICKS_PER_SECOND = 60;
        const int ROW_HEIGHT = 32;       	// This is 1/15 of 480. Change if you change the screen height

        // Sizes of obstacles
        const float BULLDOZER_WIDTH = 1.5f; // relative to frog's size
        const float TRACTOR_WIDTH = 1.33f;
        const float RACE_CAR_WIDTH = 1.0f;
        const float BEETLE_WIDTH = 1.25f;

        // Points
        const int POINTS_FOR_MOVING_UP = 10;
        const int POINTS_FOR_REACHING_TOP = 50;
        const int POINTS_FOR_EACH_SECOND_LEFT = 10;
        const int POINTS_FOR_SAVING_ALL_FROGS = 1000;

        Color TEXT_COLOR = Color.Black;

        Rectangle screen = new Rectangle(0, 0, 800, 480), gameOverScreen = new Rectangle (0, 0, 800, 480), pauseScreen = new Rectangle(0, 0, 800, 480);
        Rectangle frogHome, splashRectangle, frogBoundaryRectangle;
        Texture2D splashTexture, backgroundTexture, pausedTexture, gameOverTexture;
        //SoundEffect splatSoundEffect, splashSoundEffect;
        //SoundEffectInstance splatSoundEffectInstance, splashSoundEffectInstance;
        Vector2 timerVector, scoreVector, livesLeftVector, frogsSavedVector;
        string timeString, scoreString, livesLeftString, frogsSavedString;
        int laneHeight;

        enum GameStates
        {
            Splash,
            Paused,
            Playing,
            GameOver
        }
        GameStates gameState;
        SpriteFont font;

        int timer;
        int livesLeft;
        int score;
        int frogsSaved; //number that make it all the way to the top
        Frogger frog;
        MovingItemList[] cars;

        KeyboardState kbd, prevKbd;
        bool enterPressed, endPressed, upPressed, downPressed, leftPressed, rightPressed;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            laneHeight = screen.Height / LANES;
            livesLeft = FROG_LIVES;
            timer = TIME_TO_GET_HOME * TICKS_PER_SECOND;
            timerVector = new Vector2(8, TIMER_ROW * ROW_HEIGHT);
            score = 0;
            frogsSaved = 0;
            splashRectangle = new Rectangle(0, 0, 800, 480);
            frogBoundaryRectangle = new Rectangle(screen.Left, screen.Top + ROW_HEIGHT, screen.Width, screen.Height - ROW_HEIGHT * 2);
            scoreVector = new Vector2(8, SCORE_ROW * ROW_HEIGHT);
            gameState = GameStates.Splash;
            timeString = "Time Left: ";
            livesLeftString = "Frogs Left: ";
            scoreString = "Score: ";
            frogsSavedString = "Frogs Saved: ";

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("Font");

            //splatSoundEffect = Content.Load<SoundEffect>("Splat");
            //splatSoundEffectInstance = splatSoundEffect.CreateInstance();

            //splashSoundEffect = Content.Load<SoundEffect>("Splash");
            //splashSoundEffectInstance = splashSoundEffect.CreateInstance();

            pausedTexture = Content.Load<Texture2D>("Generic Pause Screen");
            gameOverTexture = Content.Load<Texture2D>("Retro Game Over Screen");
            splashTexture = Content.Load<Texture2D>("FroggerSplash");
            backgroundTexture = Content.Load<Texture2D>("FroggerBackground");
            frogHome = new Rectangle(screen.Left + screen.Width / 2 - ROW_HEIGHT / 2, screen.Bottom - ROW_HEIGHT * 2, ROW_HEIGHT, ROW_HEIGHT);
            frog = new Frogger(Content.Load<Texture2D>("Frog100x100"), frogHome, true, frogBoundaryRectangle);

            cars = new MovingItemList[CAR_ROWS];

            //Bottom Lanes
            Rectangle carRectangle = new Rectangle(0, screen.Bottom - ROW_HEIGHT * 3, (int)(RACE_CAR_WIDTH * ROW_HEIGHT), ROW_HEIGHT);
            cars[0] = new MovingItemList(Content.Load<Texture2D>("BeetleWest100x100"), carRectangle, 2, Direction.West, screen);

            carRectangle = new Rectangle(0, screen.Bottom - ROW_HEIGHT * 4, (int)(BULLDOZER_WIDTH * ROW_HEIGHT), ROW_HEIGHT);
            cars[1] = new MovingItemList(Content.Load<Texture2D>("BulldozerEast100x100"), carRectangle, 2, Direction.East, screen);

            carRectangle = new Rectangle(0, screen.Bottom - ROW_HEIGHT * 5, (int)(TRACTOR_WIDTH * ROW_HEIGHT), ROW_HEIGHT);
            cars[2] = new MovingItemList(Content.Load<Texture2D>("TractorWest100x100"), carRectangle, 2, Direction.West, screen);

            carRectangle = new Rectangle(0, screen.Bottom - ROW_HEIGHT * 6, (int)(BULLDOZER_WIDTH * ROW_HEIGHT), ROW_HEIGHT);
            cars[3] = new MovingItemList(Content.Load<Texture2D>("BulldozerEast100x100"), carRectangle, 2, Direction.East, screen);

            carRectangle = new Rectangle(0, screen.Bottom - ROW_HEIGHT * 7, (int)(TRACTOR_WIDTH * ROW_HEIGHT), ROW_HEIGHT);
            cars[4] = new MovingItemList(Content.Load<Texture2D>("RaceCarWest100x100"), carRectangle, 2, Direction.West, screen);

            //Top Lanes
            carRectangle = new Rectangle(0, screen.Bottom - ROW_HEIGHT * 9, (int)(TRACTOR_WIDTH * ROW_HEIGHT), ROW_HEIGHT);
            cars[5] = new MovingItemList(Content.Load<Texture2D>("BeetleEast100x100"), carRectangle, 2, Direction.East, screen);

            carRectangle = new Rectangle(0, screen.Bottom - ROW_HEIGHT * 10, (int)(TRACTOR_WIDTH * ROW_HEIGHT), ROW_HEIGHT);
            cars[6] = new MovingItemList(Content.Load<Texture2D>("BullDozerWest100x100"), carRectangle, 2, Direction.West, screen);

            carRectangle = new Rectangle(0, screen.Bottom - ROW_HEIGHT * 11, (int)(TRACTOR_WIDTH * ROW_HEIGHT), ROW_HEIGHT);
            cars[7] = new MovingItemList(Content.Load<Texture2D>("TractorEast100x100"), carRectangle, 2, Direction.East, screen);

            carRectangle = new Rectangle(0, screen.Bottom - ROW_HEIGHT * 12, (int)(TRACTOR_WIDTH * ROW_HEIGHT), ROW_HEIGHT);
            cars[8] = new MovingItemList(Content.Load<Texture2D>("BullDozerWest100x100"), carRectangle, 2, Direction.West, screen);

            carRectangle = new Rectangle(0, screen.Bottom - ROW_HEIGHT * 13, (int)(TRACTOR_WIDTH * ROW_HEIGHT), ROW_HEIGHT);
            cars[9] = new MovingItemList(Content.Load<Texture2D>("RaceCarEast100x100"), carRectangle, 2, Direction.East, screen);

            float messageWidth = font.MeasureString("Lives left: 5").X;
            livesLeftVector = new Vector2(screen.Right - (int)messageWidth - 10, screen.Top);
            frogsSavedVector = new Vector2(screen.Right - (int)messageWidth - 50, TIMER_ROW * ROW_HEIGHT);
        }

        void getInput()
        {
            enterPressed = endPressed = upPressed = downPressed = leftPressed = rightPressed = false;
            if (kbd.IsKeyDown(Keys.Enter) && prevKbd.IsKeyUp(Keys.Enter))
                enterPressed = true;
            if (kbd.IsKeyDown(Keys.End) && prevKbd.IsKeyUp(Keys.End))
                endPressed = true;
            if (kbd.IsKeyDown(Keys.Up) && prevKbd.IsKeyUp(Keys.Up))
                upPressed = true;
            if (kbd.IsKeyDown(Keys.Down) && prevKbd.IsKeyUp(Keys.Down))
                downPressed = true;
            if (kbd.IsKeyDown(Keys.Left) && prevKbd.IsKeyUp(Keys.Left))
                leftPressed = true;
            if (kbd.IsKeyDown(Keys.Right) && prevKbd.IsKeyUp(Keys.Right))
                rightPressed = true;
        }

        void MoveFrog()
        {
            if (upPressed == true)
            {
                frog.Jump(Direction.North);
                score = score + 10;
            }
            if (downPressed == true)
                frog.Jump(Direction.South);
            if (leftPressed == true)
                frog.Jump(Direction.West);
            if (rightPressed == true)
                frog.Jump(Direction.East);
        }

        void UpdateState()
        {
            if (gameState == GameStates.Splash)
            {
                if (kbd.IsKeyDown(Keys.Enter) && prevKbd.IsKeyUp(Keys.Enter) || kbd.IsKeyDown(Keys.Space) && prevKbd.IsKeyUp(Keys.Space))
                    gameState = GameStates.Playing;

                else if (kbd.IsKeyDown(Keys.Escape) && prevKbd.IsKeyUp(Keys.Escape))
                    Exit();
            }

            if (gameState == GameStates.GameOver)
            {
                if (kbd.IsKeyDown(Keys.Enter) && prevKbd.IsKeyUp(Keys.Enter) || kbd.IsKeyDown(Keys.Space) && prevKbd.IsKeyUp(Keys.Space))
                {
                    gameState = GameStates.Splash;
                    livesLeft = 5;
                    frogsSaved = 0;
                    score = 0;
                }

                else if (kbd.IsKeyDown(Keys.Escape) && prevKbd.IsKeyUp(Keys.Escape))
                    Exit();
            }
        }

        void UpdateScore()
        {
            if (frog.thePosition.Y <= 33)
            {
                frog.thePosition = frogHome;
                frogsSaved++;
                livesLeft--;
                timer = 1800;
                score = score + 50;
            }
        }

        protected override void Update(GameTime gameTime)
        {
            kbd = Keyboard.GetState();
            getInput();
            UpdateState();

            if (gameState == GameStates.Playing)
            {
                cars[0].Update();
                cars[1].Update();
                cars[2].Update();
                cars[3].Update();
                cars[4].Update();
                cars[5].Update();
                cars[6].Update();
                cars[7].Update();
                cars[8].Update();
                cars[9].Update();
                timer--;
                MoveFrog();

                //Check For Collision
                for (int i = 0; i < CAR_ROWS; i++)
                {
                    if (cars[i].HasCollision(frog))
                    {
                        Console.WriteLine("Dead Frog");
                        livesLeft--;
                        frog.thePosition = frogHome;
                        timer = 1800;
                    }   
                }

                if (livesLeft == 0)
                {
                    gameState = GameStates.GameOver;
                }

                if (kbd.IsKeyDown(Keys.Escape) && prevKbd.IsKeyUp(Keys.Escape))
                {
                    Exit();
                }

                UpdateScore();
            }

            prevKbd = kbd;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            if (gameState == GameStates.Splash)
                spriteBatch.Draw(splashTexture, splashRectangle, Color.White);

            else if (gameState == GameStates.Playing)
            {
                spriteBatch.Draw(backgroundTexture, screen, Color.White);
                cars[0].Draw(spriteBatch);
                cars[1].Draw(spriteBatch);
                cars[2].Draw(spriteBatch);
                cars[3].Draw(spriteBatch);
                cars[4].Draw(spriteBatch);
                cars[5].Draw(spriteBatch);
                cars[6].Draw(spriteBatch);
                cars[7].Draw(spriteBatch);
                cars[8].Draw(spriteBatch);
                cars[9].Draw(spriteBatch);
                spriteBatch.DrawString(font, timeString + (timer / 60), timerVector, Color.Red);
                spriteBatch.DrawString(font, scoreString + score, scoreVector, Color.Red);
                spriteBatch.DrawString(font, livesLeftString + livesLeft, livesLeftVector, Color.Red);
                spriteBatch.DrawString(font, frogsSavedString + frogsSaved, frogsSavedVector, Color.Red);
                frog.Draw(spriteBatch);
            }

            else if (gameState == GameStates.GameOver)
            {
                spriteBatch.Draw(gameOverTexture, gameOverScreen, Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
