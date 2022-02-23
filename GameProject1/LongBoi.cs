using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameProject1.Collisions;
namespace GameProject1
{
    public class LongBoi
    {
        private Random rand = new Random();

        private Texture2D texture;

        private Vector2 position;

        private RectangleCollision hb;

        public RectangleCollision HB => hb;

        private int speed = 1;

        public bool active = false;

        public Color Color { get; set; } = Color.White;

        private Player p;

        public LongBoi(Player player)
        {
            this.position = new Vector2(800,0);
            this.hb = new RectangleCollision(new Vector2(800, 0), 32, 500);
            this.p = player;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("LongBoi");
        }

        public void Update(GameTime gameTime, Viewport viewport, bool mode)
        {
            if (active)
            {
                position.X -= 10 * speed;
                hb.X -= 10 * speed;
            }

            if (position.X <= 0)
            {
                resetLong(viewport, mode);
            }
        }

        public void resetLong(Viewport viewport, bool mode)
        {
            Vector2 rsPos = new Vector2(viewport.Width + rand.Next(100, 201), 0);
            position = rsPos;
            hb.X = rsPos.X;
            hb.Y = rsPos.Y;
            if(!mode)p.score++;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White);
            //spriteBatch.Draw(texture, new Vector2(hb.X, hb.Y), null, Color.Blue);
        }
    }
}
