using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Threading;

namespace GameProject1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private KeyboardState currentKeyboadState;
        private KeyboardState priorKeyboadState;

        private Player player;
        private PainSquare[] painSquares;
        private LongBoi longBoi;
        private DeathLazer[] dl;
        private GoalSquare[] gs;
        private SpriteFont arial;
        private Song backgroundMusic;

        private bool gameStarted = false;
        private bool goalMode = false;
        private int square = 0;        
        private double nxtsqr = 10;
        private double nxtBoi = 50;
        private int lzr = 0;
        private double nxtLzr = 30;
        private int hghscr = 0;
        private float magnatude = 1f;
        private float offset = -50;
        private bool dead = false;
        private bool dying = false;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            this.Window.Title = "Dodge Square";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            player = new Player();
            painSquares = new PainSquare[] 
            {
                new PainSquare(new Vector2(500, 200), player),
                new PainSquare(new Vector2(500, 500), player),
                new PainSquare(new Vector2(500, 500), player),
                new PainSquare(new Vector2(500, 500), player),
                new PainSquare(new Vector2(500, 500), player),
                new PainSquare(new Vector2(500, 500), player),
                new PainSquare(new Vector2(500, 500), player),
                new PainSquare(new Vector2(500, 500), player),
                new PainSquare(new Vector2(500, 500), player),
                new PainSquare(new Vector2(500, 500), player),
                new PainSquare(new Vector2(500, 500), player) 
            };
            longBoi = new LongBoi(player);
            dl = new DeathLazer[] 
            { 
                new DeathLazer(player),
                new DeathLazer(player),
                new DeathLazer(player)
            };

            gs = new GoalSquare[] { new GoalSquare(new Vector2(500, 500), player) };

            Components.Add(player.Initialize(this));
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            foreach (var pain in painSquares) pain.LoadContent(Content);
            longBoi.LoadContent(Content);
            foreach(var death in dl) death.LoadContent(Content);
            foreach (var goal in gs) goal.LoadContent(Content);
            player.LoadContent(Content);
            arial = Content.Load<SpriteFont>("arial");
            backgroundMusic = Content.Load<Song>("bgmusic");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundMusic);
        }

        protected override void Update(GameTime gameTime)
        {

            Viewport viewport = _graphics.GraphicsDevice.Viewport;
            priorKeyboadState = currentKeyboadState;
            currentKeyboadState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.G) && !gameStarted && priorKeyboadState != currentKeyboadState)
            {
                goalMode = !goalMode;
            }

            // TODO: Add your update logic here
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && !gameStarted && priorKeyboadState != currentKeyboadState)
            {
                gameStarted = true;
                painSquares[square].active = true;
                square++;
                if (goalMode)
                {
                    gs[0].goalStart(viewport);
                }
            }

            player.Update(gameTime, viewport, dying);
            
            if(player.score > nxtsqr && square < painSquares.Length)
            {
                painSquares[square].active = true;
                nxtsqr += 20;
                square++;
            }

            if(player.score > nxtBoi )
            {
                longBoi.active = true;
            }

            if(player.score > nxtLzr && lzr < dl.Length)
            {
                dl[lzr].active = true;
                nxtLzr += 30;
                lzr++;
            }

            foreach (var pain in painSquares)
            {
                
                pain.Update(gameTime, viewport, goalMode);
                
                if (pain.active && pain.HB.CollidesWith(player.HB))
                {
                    if (player.score > hghscr)
                    {
                        hghscr = player.score;
                    }
                    foreach (var p in painSquares)
                    {
                        p.active = false;
                        p.resetPain(viewport, goalMode);
                    }
                    foreach(var death in dl)
                    {
                        death.active = false;
                        death.laserCoolDown();
                    }
                    foreach (var goal in gs) goal.exitScreen();
                    longBoi.active = false;
                    longBoi.resetLong(viewport, goalMode);
                    gameStarted = false;
                    dead = true;
                    player.playerReset();
                    player.score = 0;
                    nxtsqr = 10;
                    square = 0;
                    nxtLzr = 30;
                    lzr = 0;
                }
                
            }


            longBoi.Update(gameTime, viewport, goalMode);

            if (longBoi.active && longBoi.HB.CollidesWith(player.HB))
            {
                if (player.score > hghscr)
                {
                    hghscr = player.score;
                }
                foreach (var p in painSquares)
                {
                    p.active = false;
                    p.resetPain(viewport, goalMode);
                }
                foreach (var death in dl)
                {
                    death.active = false;
                    death.laserCoolDown();
                }
                foreach (var goal in gs) goal.exitScreen();
                longBoi.active = false;
                longBoi.resetLong(viewport, goalMode);
                gameStarted = false;
                dead = true;
                player.playerReset();
                player.score = 0;
                nxtsqr = 10;
                square = 0;
                nxtLzr = 30;
                lzr = 0;
            }



            foreach (var death in dl)
            {
                death.Update(gameTime, viewport, goalMode);
                if (death.active && death.HB.CollidesWith(player.HB))
                {

                    if (player.score > hghscr)
                    {
                        hghscr = player.score;
                    }

                    foreach (var p in painSquares)
                    {
                        p.active = false;
                        p.resetPain(viewport, goalMode);
                    }
                    foreach (var death2 in dl)
                    {
                        death2.active = false;
                        death2.laserCoolDown();
                    }
                    foreach (var goal in gs) goal.exitScreen();
                    longBoi.active = false;
                    longBoi.resetLong(viewport, goalMode);
                    gameStarted = false;
                    dead = true;
                    player.playerReset();
                    player.score = 0;
                    nxtsqr = 10;
                    square = 0;
                    nxtLzr = 30;
                    lzr = 0;
                }
            }
            
            foreach(var goal in gs)
            {
                if (goal.HB.CollidesWith(player.HB))
                {
                    goal.resetGoal(viewport);
                }
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {



            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            Viewport viewport = _graphics.GraphicsDevice.Viewport;

            Matrix zoom;

            if (dying)
            {
                zoom = Matrix.CreateTranslation(new Vector3(offset, offset, 0)) * Matrix.CreateScale(magnatude);
            }
            else
            {
                zoom = Matrix.CreateScale(magnatude);
            }
            if (dead)
            {
                magnatude = 2;
                offset = -50;
                dead = false;
                dying = true;
            }
            else if(magnatude > 1f)
            {
                magnatude -= .05f;
                offset -= 1.25f;
                if (magnatude < 1f)
                {
                    magnatude = 1f;
                }

            }
            else
            {
                dying = false;
            }

            Matrix pcLoc = Matrix.CreateTranslation(player.Position.X * .333f, player.Position.Y * .333f, 0);

            _spriteBatch.Begin(transformMatrix : zoom);
            if (!gameStarted)
            {
                _spriteBatch.DrawString(arial, "Press Space to start/dash.", new Vector2((viewport.Width / 2) - 110, (viewport.Height / 2) - 45), Color.White, 0, Vector2.Zero, .25f, SpriteEffects.None, 0);
                _spriteBatch.DrawString(arial, "Press H for a dash marker.", new Vector2((viewport.Width / 2) - 110, (viewport.Height / 2) - 65), Color.White, 0, Vector2.Zero, .25f, SpriteEffects.None, 0);
                _spriteBatch.DrawString(arial, "Press G to play goal mode!.", new Vector2((viewport.Width / 2) - 110, (viewport.Height / 2) - 85), Color.White, 0, Vector2.Zero, .25f, SpriteEffects.None, 0);
            }
            _spriteBatch.DrawString(arial, player.score.ToString(), new Vector2(viewport.Width / 2, viewport.Height / 2), Color.White, 0, new Vector2(25,25), 1f, SpriteEffects.None, 0);
            _spriteBatch.DrawString(arial, "HIGH SCORE: " + hghscr.ToString(), new Vector2(5,5), Color.White, 0, Vector2.Zero, .3f, SpriteEffects.None, 0);
            if(goalMode) _spriteBatch.DrawString(arial, "Mode == Goal", new Vector2(viewport.Width - 150, 5), Color.White, 0, Vector2.Zero, .3f, SpriteEffects.None, 0);
            else _spriteBatch.DrawString(arial, "Mode == Dodge", new Vector2(viewport.Width - 150, 5), Color.White, 0, Vector2.Zero, .3f, SpriteEffects.None, 0);
            player.Draw(gameTime, _spriteBatch);
            longBoi.Draw(gameTime, _spriteBatch);
            foreach (var pain in painSquares) pain.Draw(gameTime, _spriteBatch);
            foreach(var death in dl)death.Draw(gameTime, _spriteBatch);
            foreach (var goal in gs) goal.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();

            

            base.Draw(gameTime);
        }
    }
}
