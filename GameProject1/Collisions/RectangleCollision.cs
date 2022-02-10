using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

/// <summary>
/// Referencing code from the Collision's Tutorial by Nathan Bean
/// </summary>

namespace GameProject1.Collisions
{
    public struct RectangleCollision
    {

        public float X;

        public float Y;

        public float Width;

        public float Height;


        public float Left => X;
        public float Right => X + Width;
        public float Top => Y;
        public float Bottom => Y + Height;

        public RectangleCollision(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public RectangleCollision(Vector2 position, float width, float height)
        {
            X = position.X;
            Y = position.Y;
            Height = height;
            Width = width;
        }

        public bool CollidesWith(RectangleCollision other)
        {
            return CollisionHelper.Collides(this, other);
        }

    }
}
