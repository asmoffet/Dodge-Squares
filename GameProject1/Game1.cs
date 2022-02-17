using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace GameProject1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Player player;
        private PainSquare[] painSquares;
        private LongBoi longBoi;
        private DeathLazer[] dl;
        private SpriteFont arial;

        private bool gameStarted = false;
        private int square = 0;        
        private double nxtsqr = 10;
        private double nxtBoi = 30;
        private int lzr = 0;
        private double nxtLzr = 50;
        private int hghscr = 0;

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
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            foreach (var pain in painSquares) pain.LoadContent(Content);
            longBoi.LoadContent(Content);
            foreach(var death in dl) death.LoadContent(Content);
            player.LoadContent(Content);
            arial = Content.Load<SpriteFont>("arial");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && !gameStarted)
            {
                gameStarted = true;
                painSquares[square].active = true;
                square++;
            }

            Viewport viewport = _graphics.GraphicsDevice.Viewport;
            player.Update(gameTime, viewport);
            
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
                
                pain.Update(gameTime, viewport);
                
                if (pain.active && pain.HB.CollidesWith(player.HB))
                {
                    if (player.score > hghscr)
                    {
                        hghscr = player.score;
                    }
                    foreach (var p in painSquares)
                    {
                        p.active = false;
                        p.resetPain(viewport);
                    }
                    foreach(var death in dl)
                    {
                        death.active = false;
                        death.laserCoolDown();
                    }
                    longBoi.active = false;
                    longBoi.resetLong(viewport);
                    gameStarted = false;
                    player.playerReset();
                    player.score = 0;
                    nxtsqr = 10;
                    square = 0;
                    nxtLzr = 50;
                    lzr = 0;
                }
                
            }


            longBoi.Update(gameTime, viewport);

            if (longBoi.active && longBoi.HB.CollidesWith(player.HB))
            {
                if (player.score > hghscr)
                {
                    hghscr = player.score;
                }
                foreach (var p in painSquares)
                {
                    p.active = false;
                    p.resetPain(viewport);
                }
                foreach (var death in dl)
                {
                    death.active = false;
                    death.laserCoolDown();
                }
                longBoi.active = false;
                longBoi.resetLong(viewport);
                gameStarted = false;
                player.playerReset();
                player.score = 0;
                nxtsqr = 10;
                square = 0;
                nxtLzr = 50;
                lzr = 0;
            }



            foreach (var death in dl)
            {
                death.Update(gameTime, viewport);
                if (death.active && death.HB.CollidesWith(player.HB))
                {

                    if (player.score > hghscr)
                    {
                        hghscr = player.score;
                    }

                    foreach (var p in painSquares)
                    {
                        p.active = false;
                        p.resetPain(viewport);
                    }
                    foreach (var death2 in dl)
                    {
                        death2.active = false;
                        death2.laserCoolDown();
                    }
                    longBoi.active = false;
                    longBoi.resetLong(viewport);
                    gameStarted = false;
                    player.playerReset();
                    player.score = 0;
                    nxtsqr = 10;
                    square = 0;
                    nxtLzr = 50;
                    lzr = 0;
                }
            }
            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            Viewport viewport = _graphics.GraphicsDevice.Viewport;
            _spriteBatch.Begin();
            if (!gameStarted)
            {
                _spriteBatch.DrawString(arial, "Press Space to start/dash.", new Vector2((viewport.Width / 2) - 110, (viewport.Height / 2) - 45), Color.White, 0, Vector2.Zero, .25f, SpriteEffects.None, 0);
                _spriteBatch.DrawString(arial, "Press H for a dash marker.", new Vector2((viewport.Width / 2) - 110, (viewport.Height / 2) - 60), Color.White, 0, Vector2.Zero, .25f, SpriteEffects.None, 0);
            }
            _spriteBatch.DrawString(arial, player.score.ToString(), new Vector2(viewport.Width / 2, viewport.Height / 2), Color.White, 0, new Vector2(25,25), 1f, SpriteEffects.None, 0);
            _spriteBatch.DrawString(arial, "HIGH SCORE: " + hghscr.ToString(), new Vector2(5,5), Color.White, 0, Vector2.Zero, .3f, SpriteEffects.None, 0);
            player.Draw(gameTime, _spriteBatch);
            longBoi.Draw(gameTime, _spriteBatch);
            foreach (var pain in painSquares) pain.Draw(gameTime, _spriteBatch);
            foreach(var death in dl)death.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
