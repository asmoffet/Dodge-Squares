using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
/// <summary>
/// referencing code from Nathan Bean's Collision Tutorial
/// </summary>
namespace GameProject1.Collisions
{
    public class CollisionHelper
    {

        /// <summary>
        /// detects collision between two bounding rectangles
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>true for collison false otherwise</returns>
        public static bool Collides(RectangleCollision a, RectangleCollision b)
        {
            return !(a.Right < b.Left || a.Left > b.Right
                        || a.Top > b.Bottom || a.Bottom < b.Top);
        }
    }
}
