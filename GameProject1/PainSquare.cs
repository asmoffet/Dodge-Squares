using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameProject1.Collisions;
namespace GameProject1
{
    public class PainSquare
    {
        private Random rand = new Random();

        private Texture2D texture;

        private Vector2 position;

        private RectangleCollision hb;

        private float scl = 1f;

        public RectangleCollision HB => hb;

        private int speed = 1;

        public bool active = false;

        public Color Color { get; set; } = Color.White;

        private Player p;

        public PainSquare(Vector2 vector, Player player) 
        {
            this.position = vector;
            this.hb = new RectangleCollision(vector * (int)scl, 32 * (int)scl, 32 * (int)scl);
            this.p = player;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("squareMan");
        }
        public void Update(GameTime gameTime, Viewport viewport, bool mode)
        {
            if (active)
            {
                position.X -= 10 * speed;
                hb.X -= 10 * speed;
            }

            if(position.X <= 0)
            {
                resetPain(viewport, mode);
            }
        }

        public void resetPain(Viewport viewport, bool mode)
        {
            Vector2 rsPos = new Vector2(viewport.Width + rand.Next(125, 226), rand.Next(0, viewport.Height));
            scl = (float)rand.Next(1, 4);
            position = rsPos;
            hb.X = rsPos.X;
            hb.Y = rsPos.Y;
            hb.Height = 32 * (int)scl;
            hb.Width = 32 * (int)scl;
            if(!mode)p.score++;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0, Vector2.Zero, scl, SpriteEffects.None, 0);
            //spriteBatch.Draw(texture, new Vector2(hb.X, hb.Y), null, Color.Blue, 0, Vector2.Zero, scl, SpriteEffects.None, 0);
        }

    }
}
