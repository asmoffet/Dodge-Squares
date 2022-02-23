using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using GameProject1.Collisions;

namespace GameProject1
{
    public class GoalSquare
    {
        private Random rand = new Random();

        private Texture2D texture;

        private SoundEffect sfx;

        private Vector2 position;

        private RectangleCollision hb;

        public RectangleCollision HB => hb;


        public bool active = false;

        private Player p;

        public GoalSquare(Vector2 vector, Player player)
        {
            this.position = vector;
            this.hb = new RectangleCollision(vector, 32, 32);
            this.p = player;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("squareMan");
            sfx = content.Load<SoundEffect>("Pickup_Coin");
        }
        public void Update(GameTime gameTime, Viewport viewport)
        {
            
        }
        public void goalStart(Viewport viewport)
        {
            Vector2 rsPos = new Vector2(rand.Next(0, viewport.Width - 32), rand.Next(0, viewport.Height - 32));
            position = rsPos;
            hb.X = rsPos.X;
            hb.Y = rsPos.Y;
        }

        public void resetGoal(Viewport viewport)
        {
            Vector2 rsPos = new Vector2(rand.Next(0, viewport.Width - 32), rand.Next(0, viewport.Height - 32));
            position = rsPos;
            hb.X = rsPos.X;
            hb.Y = rsPos.Y;
            sfx.Play();
            p.score += 5;
        }

        public void exitScreen()
        {
            Vector2 rsPos = new Vector2(500, 500);
            position = rsPos;
            hb.X = rsPos.X;
            hb.Y = rsPos.Y;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.Green, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            //spriteBatch.Draw(texture, new Vector2(hb.X, hb.Y), null, Color.Blue, 0, Vector2.Zero, scl, SpriteEffects.None, 0);
        }

    }
}
