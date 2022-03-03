//All code in ParticleSystem is used from the Particle System Exapmple assignemt
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GameProject1
{
    public class ParticleSystem : DrawableGameComponent
    {
        public const int AlphaBlendDrawOrder = 100;
        public const int AdditiveBlendDrawOrder = 200;

        protected static SpriteBatch spriteBatch;
        protected static ContentManager contentManager;

        Particle[] particles;
        Queue<int> freeParticles;
        Texture2D texture;
        Vector2 origin;

        protected BlendState blendState = BlendState.AlphaBlend;
        protected string textureFilename;
        protected int minNumParticles;
        protected int maxNumParticles;

        public int FreeParticleCount => freeParticles.Count;

        /// <summary>
        /// Constructs a new instance of a particle system
        /// </summary>
        /// <param name="game"></param>
        public ParticleSystem(Game game, int maxParticles) : base(game)
        {
            // Create our particles
            particles = new Particle[maxParticles];
            freeParticles = new Queue<int>(maxParticles);
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].Initialize(Vector2.Zero);
                freeParticles.Enqueue(i);
            }
            // Run the InitializeConstants hook
            InitializeConstants();
        }
        /// <summary>
        /// Used to do the initial configuration of the particle engine.  The 
        /// protected constants `textureFilename`, `minNumParticles`, and `maxNumParticles`
        /// should be set in the override.
        /// </summary>
        protected virtual void InitializeConstants() { }
        /// <summary>
        /// InitializeParticle randomizes some properties for a particle, then
        /// calls initialize on it. It can be overridden by subclasses if they 
        /// want to modify the way particles are created.
        /// </summary>
        /// <param name="p">the particle to initialize</param>
        /// <param name="where">the position on the screen that the particle should be
        /// </param>
        protected virtual void InitializeParticle(ref Particle p, Vector2 where)
        {
            // Initialize the particle with default values
            p.Initialize(where);
        }
        /// <summary>
        /// Updates the individual particles.  Can be overridden in derived classes
        /// </summary>
        /// <param name="particle">The particle to update</param>
        /// <param name="dt">The elapsed time</param>
        protected virtual void UpdateParticle(ref Particle particle, float dt)
        {
            // Update particle's linear motion values
            particle.Velocity += particle.Acceleration * dt;
            particle.Position += particle.Velocity * dt;

            // Update the particle's angular motion values
            particle.AngularVelocity += particle.AngularAcceleration * dt;
            particle.Rotation += particle.AngularVelocity * dt;

            // Update the time the particle has been alive 
            particle.TimeSinceStart += dt;
        }

        /// <summary>
        /// Override the base class LoadContent to load the texture. once it's
        /// loaded, calculate the origin.
        /// </summary>
        /// <throws>A InvalidOperationException if the texture filename is not provided</throws>
        protected override void LoadContent()
        {
            // create the shared static ContentManager and SpriteBatch,
            // if this hasn't already been done by another particle engine
            if (contentManager == null) contentManager = new ContentManager(Game.Services, "Content");
            if (spriteBatch == null) spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            // make sure sub classes properly set textureFilename.
            if (string.IsNullOrEmpty(textureFilename))
            {
                string message = "textureFilename wasn't set properly, so the " +
                    "particle system doesn't know what texture to load. Make " +
                    "sure your particle system's InitializeConstants function " +
                    "properly sets textureFilename.";
                throw new InvalidOperationException(message);
            }
            // load the texture....
            texture = contentManager.Load<Texture2D>(textureFilename);

            // ... and calculate the center. this'll be used in the draw call, we
            // always want to rotate and scale around this point.
            origin.X = texture.Width / 2;
            origin.Y = texture.Height / 2;

            base.LoadContent();
        }

        /// <summary>
        /// Overriden from DrawableGameComponent, Update will update all of the active
        /// particles.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            // calculate dt, the change in the since the last frame. the particle
            // updates will use this value.
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // go through all of the particles...
            for (int i = 0; i < particles.Length; i++)
            {

                if (particles[i].Active)
                {
                    // ... and if they're active, update them.
                    UpdateParticle(ref particles[i], dt);
                    // if that update finishes them, put them onto the free particles
                    // queue.
                    if (!particles[i].Active)
                    {
                        freeParticles.Enqueue(i);
                    }
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Overriden from DrawableGameComponent, Draw will use the static 
        /// SpriteBatch to render all of the active particles.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // tell sprite batch to begin, using the spriteBlendMode specified in
            // initializeConstants
            spriteBatch.Begin(blendState: blendState);

            foreach (Particle p in particles)
            {
                // skip inactive particles
                if (!p.Active)
                    continue;

                spriteBatch.Draw(texture, p.Position, null, p.Color,
                    p.Rotation, origin, p.Scale, SpriteEffects.None, 0.0f);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// AddParticles's job is to add an effect somewhere on the screen. If there 
        /// aren't enough particles in the freeParticles queue, it will use as many as 
        /// it can. This means that if there not enough particles available, calling
        /// AddParticles will have no effect.
        /// </summary>
        /// <param name="where">where the particle effect should be created</param>
        protected void AddParticles(Vector2 where)
        {
            // the number of particles we want for this effect is a random number
            // somewhere between the two constants specified by the subclasses.
            int numParticles =
                RandomHelper.Next(minNumParticles, maxNumParticles);

            // create that many particles, if you can.
            for (int i = 0; i < numParticles && freeParticles.Count > 0; i++)
            {
                // grab a particle from the freeParticles queue, and Initialize it.
                int index = freeParticles.Dequeue();
                InitializeParticle(ref particles[index], where);
            }
        }

        /// <summary>
        /// AddParticles's job is to add an effect somewhere on the screen. If there 
        /// aren't enough particles in the freeParticles queue, it will use as many as 
        /// it can. This means that if there not enough particles available, calling
        /// AddParticles will have no effect.
        /// </summary>
        /// <param name="where">where the particle effect should be created</param>
        protected void AddParticles(Rectangle where)
        {
            // the number of particles we want for this effect is a random number
            // somewhere between the two constants specified by the subclasses.
            int numParticles =
                RandomHelper.Next(minNumParticles, maxNumParticles);

            // create that many particles, if you can.
            for (int i = 0; i < numParticles && freeParticles.Count > 0; i++)
            {
                // grab a particle from the freeParticles queue, and Initialize it.
                int index = freeParticles.Dequeue();
                InitializeParticle(ref particles[index], RandomHelper.RandomPosition(where));
            }
        }

    }
}
