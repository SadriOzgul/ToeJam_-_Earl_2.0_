using Assimp;
using Gum.Forms;
using Gum.Forms.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGameGum;
using SharpDX.X3DAudio;
using Syncfusion.XForms.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Xamarin.Forms.Xaml;


namespace ToeJam___Earl_2._0_
{
    public class Game1 : Game
    { // from here until line 233 + is an example of a class.
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        //Implementing the BeeEnemny scrpit within Game1. Referencing to the BeeEnemey for the bees.
        private BeeEnemy _bees;
        private EnemyDemon _demon;


        // The reference to the AnimatedTexture for the character
        private AnimatedTexture _IdleEarl;
        private ToeJam _Toejam;
        // The rotation of the character on screen
        private const float rotation = 0;
        private float presentRotation = 0; 
        // The scale of the character, how big it is drawn
        private const float scale = 3.0f;
        private const float _ToeJamScale = 3.0f;
        // The draw order of the sprite
        private const float depth = 0.5f;
        // The game visible area
        private Viewport viewport;
        // The position to draw the character. Current and last direction records the last frame from the user input. 
        private Vector2 characterPos, currentDirection, lastDirection, PresentTwoPos, ToeJamPos;

        // ToeJam Auto-Movement
        //private Vector2 toeJamTarget = new Vector2(600, 200);
        //private Vector2 toeJamFacing = new Vector2(1, 0);
        //private float toeJamSpeed = 80f;
        // ToeJam wandering AI
        //private Random toeJamRandom = new Random();
        //private string toeJamCurrentAnim = "";

        // --- Earl Hurt System ---
        private bool _earlHurt = false;
        private float _earlHurtTimer = 0f;
        private float _earlHurtDuration = 0.5f; // half a second

        // ----- Earl Health / Death -----
        private int _earlHealth = 3;
        private bool _earlDead = false;


        public enum LevelState
        {
            Gameplay,
            EnterElevator,
            InsideElevator,
            GoingUp,
            ExitElevator,
            NextLevel
        }

        private LevelState _levelState = LevelState.Gameplay;

        // Elevator trigger zone
        private Rectangle _elevatorRect;

        // Elevator animations
        private AnimatedTexture _elevatorIdle;
        private AnimatedTexture _elevatorEnter;
        private AnimatedTexture _elevatorInside;
        private AnimatedTexture _elevatorGoingUp;
        private AnimatedTexture _elevatorExit;

        private bool AnimationJustLooped(AnimatedTexture anim, int totalFrames)
        {
            // If the animation just reset back to frame 0, it finished one cycle
            return anim.Frame == 0 && anim.TotalElapsed < 0.05f;
        }

        private bool EnterAnimationFinished()
        {
        return AnimationJustLooped(_elevatorEnter, 7);
        }

        private bool GoingUpAnimationFinished()
        {
        return AnimationJustLooped(_elevatorGoingUp, 8);
         }

        private bool ExitAnimationFinished()
        {
        return AnimationJustLooped(_elevatorExit, 7);
        }




        // --- Elevator Cutscene Control ---
        private bool _inElevatorCutscene = false;
        private float _insideElevatorTimer = 0f;
        private float _insideElevatorDelay = 0.5f; // half-second pause before going up


        // ---- Level 2 Cake ----
        private Texture2D _cakeTexture;
        private Rectangle _cakeRect;
        private bool _cakeVisible = true;


        // How many frames/images are included in the animation
        //In this case, 3 for the idle state.
        private const int frames = 3;
        // How many frames should be drawn each second, how fast does the animation run?
        // In other words, how fast the animated sprite (Earl for example) is moving.
        private const int framesPerSec = 3;
        // Character's speed of walking.
        private float Speed = 100f;
        // True or False type that prevents restart of animation.
        private bool _idle = true;
        private bool _newInput = false; // Checks new input from the user 
        private bool _right, _left, _up, _down = false;
        private bool _GamePause, _CheckPause = false; // Pausing the game.
        private StaticObjects  _Santa; // Using Santa as a test for static objects.

        // Simple 2D cross product helper (returns a float) - ToeJam 
        //private float Cross(Vector2 a, Vector2 b)
        //{
        //    return a.X * b.Y - a.Y * b.X;
        //}



        private List<PresentsScrpit> _presents; // creating a sprite list for all presents. 
        private List<Shapes> _shapes; // testing the use of rectangle with sprite shapes.
        Texture2D _MenuOpenTexture, _MenuClosedTexture, _earlBarTexture, _numberOneTexture, _numberTwoTexture, _weinerTexture, _currentLevelTexture;
        private Texture2D _RectangleImage, _CircleImage, _JetPackPresent, _PresentTwo; // Should've add _JetPackPresent within StaticObjects...
        bool _isMenuOpenSpriteVisible = false; // Initially hidden
        bool _isMenuClosedSpriteVisible = false; // Initially hidden
        private Rectangle _spriteRectangle, _spriteshapeRectangle, _CircleRectangle, _PresentRectangeOne, _PresentRectangeTwo, _spriteToeJamshapeRectangle;
        SoundEffect burpsoundEffect, meepsoundEffect;
        Song backgroundmusic;
        SoundEffect youchSound;
        SoundEffectInstance youchInstance;
        SoundEffectInstance burpinstance, meepinstance;
        bool _RectangleVisible = true;
        bool _CircleVisible = true;
        bool _PresentOneVisible = true;
        bool _PresentTwoVisible = true;
        private Camera2D _camera;
        private float _shapesSpeed = 60f;

        Rectangle _earlBarRect;
        Rectangle _numberRect;
        Rectangle _textRect;
        Rectangle _BlacksquareRect;

        //Function - Something you call 
        //Class - NOT A FUNCTION. Its a folder that contains certain properties that work together. Properties such as varibles and functions 
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _IdleEarl = new AnimatedTexture(new Vector2(0,0), _spriteRectangle, rotation, scale, depth);
          //  _Toejam = new ToeJam(Vector2.Zero, _spriteToeJamshapeRectangle, rotation, _ToeJamScale, depth);
            _Santa = new StaticObjects(new Vector2(30,50), _spriteshapeRectangle, rotation, scale, depth);
      
            //_AngryBees = new StaticObjects(new Vector2 (30, 150), rotation, scale, depth); // Static object example.
        }

        


        protected override void Initialize() 
        {
           
            _presents = new List<PresentsScrpit>();

            base.Initialize();
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here

            // Load LEVEL ONLY FIRST TIME
            if (LevelManager.CurrentLevel == 1)
                LevelManager.LoadLevel(Content, GraphicsDevice, 1);

            // Load cake for Level 2
            if (LevelManager.CurrentLevel == 2)
            {
                _cakeTexture = Content.Load<Texture2D>("Cake");   // your Cake.xnb
                _cakeRect = new Rectangle(400, 250, 70, 70);       // cake position & size
                _cakeVisible = true;                               // reset when entering L2
            }


            _spriteshapeRectangle = new Rectangle(100, 0, 100, 100);
            //  _spriteToeJamshapeRectangle = new Rectangle(100, 0, 100, 100);
            //_CircleRectangle = new Rectangle(100, 300, 100, 100);
            _PresentRectangeOne = new Rectangle(800, 500, 50, 50);  // New spot
            _PresentRectangeTwo = new Rectangle(820, 600, 70, 50);  // New spot


            //Texture2D _JetPackPresent = Content.Load<Texture2D>("JetPackPresent");
            //_presents.Add(new PresentsScrpit(jetPackPresent, new Vector2(4, 15), 2.4f));

            //_Toejam.Load(Content, "__IdleToeJam", frames, framesPerSec);
            _IdleEarl.Load(Content, "Idle_Earl", frames, framesPerSec);

            _earlBarTexture = Content.Load<Texture2D>("Earl_Bar");
            _numberOneTexture = Content.Load<Texture2D>("one");
            _numberTwoTexture = Content.Load<Texture2D>("two");
            _weinerTexture = Content.Load<Texture2D>("weiner");
            _currentLevelTexture = _numberOneTexture;   // Default to Level 1


            _earlBarRect = new Rectangle(0, 390, 800, 95);
            _numberRect = new Rectangle(373, 400, 50, 65);
            _textRect = new Rectangle(635, 413, 150, 25);
            _BlacksquareRect = new Rectangle(0, 390, 800, 95);

            // Elevator trigger area
            _elevatorRect = LevelManager.ElevatorRect;

            // adjust as needed

            // Elevator animations (UPDATE FRAME COUNTS TO YOURS!!)
            _elevatorIdle = new AnimatedTexture(Vector2.Zero, new Rectangle(), 0, 3f, 0.4f);
            _elevatorIdle.Load(Content, "Idle_Elevator", 2, 1);

            _elevatorEnter = new AnimatedTexture(Vector2.Zero, new Rectangle(), 0, 3f, 0.4f);
            _elevatorEnter.Load(Content, "Earl_Evevator", 7, 1);

            _elevatorInside = new AnimatedTexture(Vector2.Zero, new Rectangle(), 0, 3f, 0.4f);
            _elevatorInside.Load(Content, "Inside_Evevator", 8, 1);

            _elevatorGoingUp = new AnimatedTexture(Vector2.Zero, new Rectangle(), 0, 3f, 0.4f);
            _elevatorGoingUp.Load(Content, "Going_Up_Evevator", 8, 1);

            _elevatorExit = new AnimatedTexture(Vector2.Zero, new Rectangle(), 0, 3f, 0.4f);
            _elevatorExit.Load(Content, "Exiting_Evevator", 13, 1);


            _Santa.Load(Content, "SampleSanta", 0, 0);
            _PresentTwo = Content.Load<Texture2D>("PresentTwo");
            _MenuOpenTexture = Content.Load<Texture2D>("Menu-Opened"); // Replace with your texture name
            _MenuClosedTexture = Content.Load<Texture2D>("Menu-Closed"); // Replace with your texture name
            //_RectangleImage = Content.Load<Texture2D>("BlackSquare");

            using (var stream = File.OpenRead("Content/BlackSquare.png"))
            {
                _RectangleImage = Texture2D.FromStream(GraphicsDevice, stream);
            }
            using (var stream = File.OpenRead("Content/Circle.png"))
            {
                _CircleImage = Texture2D.FromStream(GraphicsDevice, stream);
            }
            using (var stream = File.OpenRead("Content/JetPackPresent_.png"))
            {
                _JetPackPresent = Texture2D.FromStream(GraphicsDevice, stream);
            }




            // Non animated sprites, frame = 1.
            viewport = _graphics.GraphicsDevice.Viewport;

            // Start Earl near elevator on Level 1
            characterPos = new Vector2(700, 900);

            _spriteRectangle = new Rectangle((int)characterPos.X, (int)characterPos.Y, 40, 60);

            // Earl's rectangle. 

            string musicPath = Path.Combine(Content.RootDirectory, "ThemeSong.wav");
            backgroundmusic = Song.FromUri("ThemeSong", new Uri(Path.GetFullPath(musicPath)));
            MediaPlayer.Volume = 0.5f; // Theme's volume.
            // Load the WAV file directly (bypasses Content Pipeline)
            using (var stream = File.OpenRead(Path.Combine(Content.RootDirectory, "Burp.wav")))
            {
                burpsoundEffect = SoundEffect.FromStream(stream);
            }
            burpinstance = burpsoundEffect.CreateInstance();
            using (var stream = File.OpenRead(Path.Combine(Content.RootDirectory, "Meep.wav")))
            {
                meepsoundEffect = SoundEffect.FromStream(stream);
            }
            meepinstance = meepsoundEffect.CreateInstance();

            using (var stream = File.OpenRead(Path.Combine(Content.RootDirectory, "Yeouch.wav")))
            {
                youchSound = SoundEffect.FromStream(stream);
            }
            youchInstance = youchSound.CreateInstance();

            _camera = new Camera2D(GraphicsDevice.Viewport);

        }



        private void LoadNextLevel()
        {
            LevelManager.CurrentLevel++;
            // Update UI level number texture
            switch (LevelManager.CurrentLevel)
            {
                case 1:
                    _currentLevelTexture = _numberOneTexture;
                    break;
                case 2:
                    _currentLevelTexture = _numberTwoTexture;
                    break;
                
            }

            LevelManager.LoadLevel(Content, GraphicsDevice, LevelManager.CurrentLevel);

            if (LevelManager.CurrentLevel == 1)
                characterPos = new Vector2(950, 420);

            if (LevelManager.CurrentLevel == 2)
                characterPos = new Vector2(500, 520);

            if (LevelManager.CurrentLevel == 3)
                characterPos = new Vector2(200, 100);


            // Reset Level 2 items
            if (LevelManager.CurrentLevel == 2)
            {
                _cakeTexture = Content.Load<Texture2D>("Cake");
                _cakeRect = new Rectangle(400, 250, 70, 70);
                _cakeVisible = true;
            }


            // Reset Earl
           // characterPos = new Vector2(600, 400);
            Speed = 130;
            _earlHurt = false;

            // Re-create the demon list
            // Re-create the demon list
            if (LevelManager.EnemyPositions.Count > 0)
            {
                Vector2 pos = LevelManager.EnemyPositions[0];
                _demon = new EnemyDemon(
                    pos,
                    pos + new Vector2(-100, 0),
                    pos + new Vector2(100, 0)
                );
                _demon.Load(Content);
            }
            else
            {
                // No demon in this level
                _demon = null;
            }


            // Update elevator rectangle
            _elevatorRect = LevelManager.ElevatorRect;

            _levelState = LevelState.Gameplay;
        }
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseState mouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState(); // Current state of the keyboard.
                                                               // ✅ Center Earl's hitbox on his sprite
            _spriteRectangle.X = (int)characterPos.X;
            _spriteRectangle.Y = (int)characterPos.Y;

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;


            if (!_GamePause) // If game is NOT paused 
            {

                if (MediaPlayer.State != MediaState.Playing)
                {
                    MediaPlayer.Play(backgroundmusic);
                    MediaPlayer.IsRepeating = true; // Optional: Set to loop
                }

                if (keyboardState.IsKeyDown(Keys.Up))
                {
                    MediaPlayer.Volume = MathHelper.Clamp(MediaPlayer.Volume + 0.1f, 0f, 1f);

                }

                if (keyboardState.IsKeyDown(Keys.Down))
                {
                    MediaPlayer.Volume = MathHelper.Clamp(MediaPlayer.Volume - 0.1f, 0f, 1f);

                }

                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    burpinstance.Play();
                   
                }

                if (keyboardState.IsKeyDown(Keys.Right))
                {
                    meepinstance.Play();
                }

                //if ( _RectangleVisible && _spriteRectangle.Intersects(_spriteshapeRectangle))
                //{
                  //  _RectangleVisible = false; // Make it disappear
                //}

                //if (_CircleVisible && _spriteRectangle.Intersects(_CircleRectangle))
                //{
                    //_CircleVisible = false; // Make it disappear
                //}
                
               // if(_RectangleVisible && _CircleVisible && _CircleRectangle.Intersects(_spriteshapeRectangle))
                //{
                 //   _RectangleVisible = false;
                  //  _CircleVisible = false;
                //}

                if (_PresentOneVisible && _spriteRectangle.Intersects(_PresentRectangeOne))
                {
                    presentRotation += 0.05f;



                }

                if (_PresentOneVisible && _spriteRectangle.Intersects(_PresentRectangeOne) && keyboardState.IsKeyDown(Keys.T))
                {
                    _PresentOneVisible = false;

                }

                if (_PresentTwoVisible && _spriteRectangle.Intersects(_PresentRectangeTwo) && keyboardState.IsKeyDown(Keys.T))
                {
                    _PresentTwoVisible = false;

                }

                // ---- Cake Pickup (Level 2) ----
                if (LevelManager.CurrentLevel == 2 && _cakeVisible)
                {
                    if (_spriteRectangle.Intersects(_cakeRect) && keyboardState.IsKeyDown(Keys.T))
                    {
                        _cakeVisible = false;
                        burpinstance.Play();
                        // TODO: give Earl health / power / score etc.
                    }
                }



                float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                
                currentDirection.X = 0; //Idle direction.
                currentDirection.Y = 0;
                _newInput = false;
                _idle = true;
                _right = false;
                _left = false;
                _up = false;
                _down = false;

                if (!_earlHurt && !_inElevatorCutscene)
                {
                    // ✅ ALL your W A S D movement code goes here
                    if (keyboardState.IsKeyDown(Keys.W))
                {
                    // Move Up
                    currentDirection.Y = 1;
                    currentDirection.X = 0;
                    characterPos.Y -= Speed * deltaTime;
                    _idle = false;
                    _right = false;
                    _left = false;
                    _up = true;
                    _down = false;
                }
                if (keyboardState.IsKeyDown(Keys.S))
                {
                    // Move Down
                    currentDirection.Y = -1;
                    currentDirection.X = 0;
                    characterPos.Y += Speed * deltaTime;
                    _idle = false;
                    _right = false;
                    _left = false;
                    _up = false;
                    _down = true;
                }
                if (keyboardState.IsKeyDown(Keys.A))
                {
                    // Move Left
                    currentDirection.Y = 0;
                    currentDirection.X = -1;
                    characterPos.X -= Speed * deltaTime;
                    _idle = false;
                    _right = false;
                    _left = true;
                    _up = false;
                    _down = false;
                }
                if (keyboardState.IsKeyDown(Keys.D))
                {
                    // Move Right
                    currentDirection.Y = 0;
                    currentDirection.X = 1;
                    characterPos.X += Speed * deltaTime;
                    _idle = false;
                    _right = true;
                    _left = false;
                    _up = false;
                    _down = false;

                }

                }



                // Check for key press to open/close the sprite (e.g., 'O' for open, 'C' for close)
                if (keyboardState.IsKeyDown(Keys.C))
                {
                    _isMenuOpenSpriteVisible = true; // Make sprite visible
                }
                if (keyboardState.IsKeyDown(Keys.O))
                {
                    _isMenuClosedSpriteVisible = true;
                    _isMenuOpenSpriteVisible = false; // Hide sprite
                   
                }
                if (keyboardState.IsKeyDown(Keys.I))
                {
                    _isMenuClosedSpriteVisible = false;
                    _isMenuOpenSpriteVisible = false;
                    // Hide sprite

                }

                _CircleRectangle.Y -= (int)(_shapesSpeed * deltaTime); // Moving Circle's Y-position.
                _spriteshapeRectangle.Y += (int)(_shapesSpeed * deltaTime); // Moving Square's Y-position.

                // --- TOEJAM RANDOM WANDER AI ---
              //  Vector2 toeJamDirection = toeJamTarget - ToeJamPos;

                // If ToeJam arrives near the target → choose a NEW random point
                //if (toeJamDirection.Length() < 10f)
                //{
                 //   toeJamTarget = new Vector2(
                  //      toeJamRandom.Next(50, 750),
                  //      toeJamRandom.Next(50, 450)
                   // );
                //}
                //else
                //{
                  //  toeJamDirection.Normalize();
                    //ToeJamPos += toeJamDirection * toeJamSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    //toeJamFacing = toeJamDirection;

                    //float dotRight = Vector2.Dot(toeJamFacing, new Vector2(1, 0));
                    //float dotLeft = Vector2.Dot(toeJamFacing, new Vector2(-1, 0));
                    //float dotUp = Vector2.Dot(toeJamFacing, new Vector2(0, -1));
                    //float dotDown = Vector2.Dot(toeJamFacing, new Vector2(0, 1));

                    //string newAnim = toeJamCurrentAnim;

                    // Cross product test (optional)
                    //float crossVal = Cross(toeJamFacing, new Vector2(1, 0));
                    // cross > 0 → facing slightly left
                    // cross < 0 → facing slightly right
                    // cross = 0 → exactly horizontal


                    // Choose animation name
                    //if (dotRight > 0.7f)
                    //    newAnim = "ToeJamRight";
                    //else if (dotLeft > 0.7f)
                    //    newAnim = "ToeJam_Left";
                    //else if (dotUp > 0.7f)
                    //    newAnim = "ToeJamForward";
                    //else if (dotDown > 0.7f)
                    //    newAnim = "ToeJamBack";

                    // Load animation ONLY when it changes
                    //if (newAnim != toeJamCurrentAnim)
                    //{
                    //    toeJamCurrentAnim = newAnim;
                    //    _Toejam.Load(Content, toeJamCurrentAnim, 6, 6);
                    //}

                //}

                // Always animate ToeJam
               // _Toejam.updateFrame((float)gameTime.ElapsedGameTime.TotalSeconds);



                if (!_earlHurt && lastDirection != currentDirection) // Trying to load on a single frame, not restarting every frame as the user moves. Prevents restarting the animation every time.
                {
                    
                    if (currentDirection.X > 0) // Moving to the right. 
                    {
                        _IdleEarl.Load(Content, "Earl__Right", 7, 7);


                        if (keyboardState.IsKeyDown(Keys.N))
                        {
                            Speed = 50f;
                            _IdleEarl.Load(Content, "_Earl_Sneaking_Right", 4, 4);
                        }
                        else
                        {
                            Speed = 100f;
                        }

                        if (!_PresentOneVisible && keyboardState.IsKeyDown(Keys.NumPad1))
                        {
                            Speed = 250f;
                            _IdleEarl.Load(Content, "Earl_RocketSkatesRight", 2, 5);

                        }
                    }
                    else if (currentDirection.X < 0)
                    {
                        _IdleEarl.Load(Content, "Earl__Left", 7, 7);

                        if (keyboardState.IsKeyDown(Keys.N))
                        {
                            Speed = 50f;
                            _IdleEarl.Load(Content, "_Earl_Sneaking_Left", 4, 5);

                        }
                        else
                        {
                            Speed = 100f;
                        }
                        if (!_PresentOneVisible && keyboardState.IsKeyDown(Keys.NumPad1))
                        {
                            Speed = 250f;
                            _IdleEarl.Load(Content, "Earl_RocketSkatesLeft", 2, 5);

                        }

                    }
                    else if (currentDirection.Y < 0)
                    {
                        _IdleEarl.Load(Content, "Earl_Forward", 6, 7);

                        if (keyboardState.IsKeyDown(Keys.N))
                        {
                            Speed = 50f;
                            _IdleEarl.Load(Content, "_Earl_Sneaking_Forward", 4, 5);
                        }
                        else
                        {
                            Speed = 100f;
                        }
                        if (!_PresentOneVisible && keyboardState.IsKeyDown(Keys.NumPad1))
                        {
                            Speed = 250f;
                            _IdleEarl.Load(Content, "Earl_RocketSkatesFront", 2, 5);

                        }

                    }
                    else if (currentDirection.Y > 0)
                    {
                        _IdleEarl.Load(Content, "Earl_Backwards", 7, 7);

                        if (keyboardState.IsKeyDown(Keys.N))
                        {
                            Speed = 50f;
                            _IdleEarl.Load(Content, "_Earl_Sneaking_Backward", 4, 5);
                        }
                        else
                        {
                            Speed = 100f;
                        }
                        if (!_PresentOneVisible && keyboardState.IsKeyDown(Keys.NumPad1))
                        {
                            Speed = 250f;
                            _IdleEarl.Load(Content, "Earl_RocketSkatesBack", 2, 5);

                        }

                    }
                    else
                    {
                        _IdleEarl.Load(Content, "Idle_Earl", frames, framesPerSec);
                         
                    }


                }
                lastDirection = currentDirection; // Updating the last direction, reading a single frame. 


                if (!_inElevatorCutscene)
                {
                    _IdleEarl.updateFrame(elapsed);
                }


                // -----------------------------------------------------
                // Decide what position the demon should react to
                // -----------------------------------------------------
                Vector2 demonTargetPos = characterPos;

                // If Earl is dead, float him upward AND give the demon
                // a fake far-away target so it returns to Patrol naturally.
                if (_earlDead)
                {
                    // Ghost floating effect
                    characterPos.Y -= 20f * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    // Demon sees a fake very far player location (so it won't chase)
                    demonTargetPos = new Vector2(10000, 10000);
                }

                if (_demon != null)
                {
                    _demon.Update(gameTime, demonTargetPos, Content);
                }

                // If Earl is dead, stop all gameplay logic now
                if (_earlDead)
                {
                    return;
                }


                // -----------------------------------------------------
                // EARL HIT BY DEMON (Only if alive & not already hurt)
                // -----------------------------------------------------
                if (_demon != null && _spriteRectangle.Intersects(_demon.Hitbox) && !_earlHurt && !_earlDead)
                {
                    _earlHurt = true;
                    _earlHurtTimer = 0f;

                    youchInstance.Play();

                    // Reduce Earl's health
                    _earlHealth--;

                    // ----- EARL DIES AFTER 3 HITS -----
                    if (_earlHealth <= 0)
                    {
                        _earlDead = true;

                        // Load ghost sprite (1 frame unless yours has more)
                        _IdleEarl.Load(Content, "Ghost_Earl", 4, 1);

                        // Immediately force demon back to Patrol
                        _demon.CurrentState = EnemyDemon.State.Patrol;

                        return; // no knockback, no youch, nothing else
                    }

                    // ---------- DIRECTIONAL YOUCH (alive only) ----------
                    if (lastDirection.X > 0)
                        _IdleEarl.Load(Content, "Youch_Right_Earl", 1, 1);
                    else if (lastDirection.X < 0)
                        _IdleEarl.Load(Content, "Youch_Left_Earl", 1, 1);
                    else if (lastDirection.Y > 0)
                        _IdleEarl.Load(Content, "Youch_Back_Earl", 1, 1);
                    else
                        _IdleEarl.Load(Content, "Youch_Front_Earl", 1, 1);

                    // ---------- KNOCKBACK (alive only) ----------
                    Vector2 knockback = characterPos - new Vector2(
                        _demon.Hitbox.Center.X,
                        _demon.Hitbox.Center.Y
                    );

                    if (knockback != Vector2.Zero)
                        knockback.Normalize();

                    characterPos += knockback * 50f; // Pushes Earl back after getting hit.
                }


                // -----------------------------------------------------
                // YOUCH TIMER (Correct logic)
                // -----------------------------------------------------
                if (_earlHurt && !_earlDead)
                {
                    _earlHurtTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (_earlHurtTimer >= _earlHurtDuration)
                    {
                        _earlHurt = false;
                        _IdleEarl.Load(Content, "Idle_Earl", 3, 2);
                    }
                }

                // ----------------- ENTER ELEVATOR -----------------
                if (_levelState == LevelState.Gameplay)
                {
                    if (_spriteRectangle.Intersects(_elevatorRect) && keyboardState.IsKeyDown(Keys.L))
                    {
                        _levelState = LevelState.EnterElevator;
                        _elevatorEnter.Reset();

                        // Disable ALL movement & hurt logic
                        _inElevatorCutscene = true;
                        _earlHurt = false;
                        Speed = 0;
                    }
                }

                // ----------------- ELEVATOR CUTSCENE SEQUENCE -----------------
               
                switch (_levelState)
                {
                    // -------------------------------------------------
                    // 1. ENTER ELEVATOR ANIMATION
                    // -------------------------------------------------
                    case LevelState.EnterElevator:

                        _elevatorEnter.updateFrame(elapsed);

                        if (EnterAnimationFinished())
                        // we fix this below
                        {
                            _levelState = LevelState.InsideElevator;
                            _elevatorInside.Reset();
                            _insideElevatorTimer = 0f;
                        }
                        break;

                    // -------------------------------------------------
                    // 2. Inside elevator idle frame for short pause
                    // -------------------------------------------------
                    case LevelState.InsideElevator:

                        _insideElevatorTimer += elapsed;
                        if (_insideElevatorTimer >= _insideElevatorDelay)
                        {
                            _levelState = LevelState.GoingUp;
                            _elevatorGoingUp.Reset();
                        }
                        break;

                    // -------------------------------------------------
                    // 3. Elevator Going Up Animation
                    // -------------------------------------------------
                    case LevelState.GoingUp:

                        _elevatorGoingUp.updateFrame(elapsed);

                        // Move Earl upward visually
                        characterPos.Y -= 40f * elapsed;

                        if (GoingUpAnimationFinished())

                        {
                            _levelState = LevelState.ExitElevator;
                            _elevatorExit.Reset();
                        }
                        break;

                    // -------------------------------------------------
                    // 4. Elevator Exit animation
                    // -------------------------------------------------
                    case LevelState.ExitElevator:

                        _elevatorExit.updateFrame(elapsed);

                        if (ExitAnimationFinished())

                        {
                            LoadNextLevel();
                            _levelState = LevelState.Gameplay;
                            _inElevatorCutscene = false;

                        }
                        break;
                }


                // Update camera to follow player
                _camera.Follow(characterPos);
              

            } 
            
            if (keyboardState.IsKeyDown(Keys.P)) { // Checks if you press P 
                if (!_CheckPause) // Makes sure that the pause input reads only for a single frame
                {
                    _CheckPause = true;
                    _GamePause = !_GamePause; // Meaning _GamePause is set to true. 
                    MediaPlayer.Stop();



                }
            }
            if (keyboardState.IsKeyUp(Keys.P))
            {
                _CheckPause = false; // Releasing the key, _CheckPause goes back into false, unpasuing the game. 
                
                Console.WriteLine("Game is unpaused!");
            }


        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(transformMatrix: _camera.Transform);
            

            // Draw level background FULLSCREEN so level color changes are visible
            // Example: 3× size map
            int scale = 1;

            _spriteBatch.Draw(
                LevelManager.LevelBackground,
                new Rectangle(
                    0,
                    0,
                    LevelManager.LevelBackground.Width * scale,
                    LevelManager.LevelBackground.Height * scale),
                Color.White
            );


            // 4. DRAW PRESENTS, MENU, ETC.
            if (_PresentOneVisible)
                _spriteBatch.Draw(_JetPackPresent, _PresentRectangeOne, Color.White);

            if (_PresentTwoVisible)
                _spriteBatch.Draw(_PresentTwo, _PresentRectangeTwo, Color.White);

            if (_isMenuOpenSpriteVisible)
                _spriteBatch.Draw(_MenuOpenTexture, new Rectangle(250, 90, 300, 100), Color.White);

            if (_isMenuClosedSpriteVisible)
                _spriteBatch.Draw(_MenuClosedTexture, new Rectangle(250, 90, 300, 100), Color.White);

            // ---- Draw Cake (ONLY If In Level 2) ----
            if (LevelManager.CurrentLevel == 2 && _cakeVisible)
            {
                _spriteBatch.Draw(_cakeTexture, _cakeRect, Color.White);
            }

            // 2. DRAW ELEVATOR & EARL
            switch (_levelState)
            {
                case LevelState.Gameplay:
                    _elevatorIdle.DrawFrame(_spriteBatch, _elevatorRect.Location.ToVector2());
                    _IdleEarl.DrawFrame(_spriteBatch, characterPos);
                    break;

                case LevelState.EnterElevator:
                    _elevatorEnter.DrawFrame(_spriteBatch, _elevatorRect.Location.ToVector2());
                    break;

                case LevelState.InsideElevator:
                    _elevatorInside.DrawFrame(_spriteBatch, _elevatorRect.Location.ToVector2());
                    break;

                case LevelState.GoingUp:
                    _elevatorGoingUp.DrawFrame(_spriteBatch, _elevatorRect.Location.ToVector2());
                    break;

                case LevelState.ExitElevator:
                    _elevatorExit.DrawFrame(_spriteBatch, _elevatorRect.Location.ToVector2());
                    break;
            }

            // 3. DRAW DEMON (only in gameplay)
            if(_levelState == LevelState.Gameplay && _demon != null)
{
                _demon.Draw(_spriteBatch);
            }


           


            _spriteBatch.End();

            // ---------------- UI DRAW (NO CAMERA TRANSFORM) ----------------
            _spriteBatch.Begin();
            _spriteBatch.Draw(_RectangleImage, _BlacksquareRect, Color.White);
            _spriteBatch.Draw(_earlBarTexture, _earlBarRect, Color.White);
            _spriteBatch.Draw(_currentLevelTexture, _numberRect, Color.White);
            _spriteBatch.Draw(_weinerTexture, _textRect, Color.White);
         

            _spriteBatch.End();


            base.Draw(gameTime);
        }

    }
}
