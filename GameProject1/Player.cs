using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameProject1.Collisions;

namespace GameProject1
{
    public class Player
    {
        private KeyboardState keyboardState;

        private Texture2D texture;

        private Vector2 position = new Vector2(200, 200);

        private RectangleCollision hb = new RectangleCollision(new Vector2(200, 200), 32, 32);

        public RectangleCollision HB => hb;

        private bool blink = true;

        private int timer = 0;

        private Color color = Color.White;

        public int score = 0;

        public RectangleCollision HitBox => hb;
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("squareMan");
        }

        public void Update(GameTime gameTime, Viewport viewport)
        {
            keyboardState = Keyboard.GetState();
            if ((keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))) { position += new Vector2(0, -5); }
            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S)) { position += new Vector2(0, 5); }
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A)) { position += new Vector2(-5, 0); }
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D)) { position += new Vector2(5, 0); }
            if (keyboardState.IsKeyDown(Keys.Space)) { Blink(); }
            CheckBounds(viewport);
            hb.X = position.X;
            hb.Y = position.Y;
            if(timer > 0)
            {
                timer--;
                if (timer < 15)
                {
                    color = Color.Orange;
                }
                if(timer == 0)
                {
                    color = Color.White;
                    blink = true;
                }
            }
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, color);
            //spriteBatch.Draw(texture, new Vector2(hb.X, hb.Y), null, Color.Gold);
        }

        private void Blink()
        {
            if (blink) position += new Vector2(125, 0);
            blink = false;
            color = Color.Red;
            timer = 30;
        }

        private void CheckBounds(Viewport viewport)
        {
            if (position.Y < 0)
            {
                position.Y = 0;

            }
            if(position.Y > viewport.Height -32)
            {
                position.Y = viewport.Height - 32;
            }
            if(position.X < 0)
            {
                position.X = 0;
            }
            if(position.X > viewport.Width - 32)
            {
                position.X = viewport.Width - 32;
            }
        }

        public void playerReset()
        {
            position = new Vector2(200, 200);
        }
    }
}
