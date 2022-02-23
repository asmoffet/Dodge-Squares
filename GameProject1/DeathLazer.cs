using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameProject1.Collisions;

namespace GameProject1
{
    public class DeathLazer
    {
        private Random rand = new Random();

        private Texture2D texture1;

        private Texture2D texture2;

        private Vector2 position;

        private RectangleCollision hb;

        private float scl = 3f;

        private int phase;

        private int timer = 0;

        public RectangleCollision HB => hb;

        public bool active = false;

        public Color Color { get; set; } = Color.White;

        private Player p;

        public DeathLazer(Player player)
        {
            this.p = player;
            this.phase = 0;
        }

        public void LoadContent(ContentManager content)
        {
            texture1 = content.Load<Texture2D>("squareMan");
            texture2 = content.Load<Texture2D>("LongBoi");
        }

        public void Update(GameTime gameTime, Viewport viewport, bool mode)
        {
            if (active && phase == 0)
            {
                setLaser(viewport);
                phase++;
            }

            if (phase == 1 && active)
            {
                if( timer <= 150 && timer > 75)
                {
                    Color = Color.Orange;
                }
                if( timer <= 75 && timer > 0)
                {
                    Color = Color.Green; 
                }
                if(timer == 0)
                {
                    timer = 11;
                    phase++;
                }
                timer--;
            }

            if( phase == 2 && active)
            {
                if(timer == 0)
                {
                    timer = 21;
                    p.score++;
                    phase++;
                }
                timer--;
            }

            if(phase == 3 && active)
            {
                if(timer == 20)
                {
                    laserGO();
                }
                if(timer == 0)
                {
                    timer = 76;
                    if(!mode)p.score++;
                    phase++;
                }
                timer--;
            }
            if(phase == 4 && active)
            {
                if (timer == 75)
                {
                    laserGO();
                }
                if (timer == 0)
                {
                    phase = 0;
                }
                timer--;
            }
        }

        private void setLaser(Viewport viewport)
        {
            Vector2 rsPos = new Vector2(viewport.Width - 16, rand.Next(0, viewport.Height - 32));
            position = rsPos;
            hb.X = 800;
            hb.Y = 800;
            hb.Height = 32 * (int)scl;
            hb.Width = 500 * (int)scl;
            timer = 225;
            Color = Color.Red;
        }


        private void laserGO()
        {
            position.X = position.X + 16;
            hb.X = 0;
            hb.Y = position.Y;
            hb.Height = 32 * (int)scl;
            hb.Width = 800;
        }

        public void laserCoolDown()
        {
            position = new Vector2(800, 800);
            hb.X = position.X;
            hb.Y = position.Y;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(phase == 0 || phase == 1)
            {
                spriteBatch.Draw(texture1, position, null, Color, 1.5708f, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
            if(phase == 2 || phase == 3)
            {
                spriteBatch.Draw(texture2, position, null, Color.White, 1.5708f, Vector2.Zero, scl, SpriteEffects.None, 0);
            }
            
        }
    }
}
