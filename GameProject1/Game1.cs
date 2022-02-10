using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProject1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Player player;
        private PainSquare[] painSquare;
        private SpriteFont arial;

        private bool gameStarted = false;
        private int square = 0;
        private int nxtsqr = 10;
        private int hghscr = 0;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            player = new Player();
            painSquare = new PainSquare[] 
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
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            foreach (var pain in painSquare) pain.LoadContent(Content);
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
                painSquare[square].active = true;
                square++;
            }



            Viewport viewport = _graphics.GraphicsDevice.Viewport;
            player.Update(gameTime, viewport);
            
            if(player.score > nxtsqr && square < painSquare.Length)
            {
                painSquare[square].active = true;
                nxtsqr += 10 * square;
                square++;
            }

            foreach (var pain in painSquare)
            {
                
                pain.Update(gameTime, viewport);

                if (pain.active && pain.HB.CollidesWith(player.HB))
                {
                    if (player.score > hghscr)
                    {
                        hghscr = player.score;
                    }
                    foreach (var p in painSquare)
                    {
                        p.active = false;
                        p.resetPain(viewport);
                    }
                    gameStarted = false;
                    player.playerReset();
                    player.score = 0;
                    nxtsqr = 10;
                    square = 0;
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
            }
            _spriteBatch.DrawString(arial, player.score.ToString(), new Vector2(viewport.Width / 2, viewport.Height / 2), Color.White, 0, new Vector2(25,25), 1f, SpriteEffects.None, 0);
            _spriteBatch.DrawString(arial, "HIGH SCORE: " + hghscr.ToString(), new Vector2(5,5), Color.White, 0, Vector2.Zero, .3f, SpriteEffects.None, 0);
            player.Draw(gameTime, _spriteBatch);
            foreach (var pain in painSquare) pain.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
