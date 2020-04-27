using OpenTK;
using System;
using System.Collections.Generic;

namespace OpenGL_Game.Objects
{
    public class WallBoundaries
    {
        public Vector3 point1;
        public Vector3 point2;
        public WallBoundaries(Vector3 firstPoint, Vector3 secondPoint)
        {
            // Set points
            if (firstPoint.X > secondPoint.X || firstPoint.Z > secondPoint.Z)
            {
                point1 = firstPoint;
                point2 = secondPoint;
            }
            else
            {
                point1 = secondPoint;
                point2 = firstPoint;
            }
        }

        public static List<WallBoundaries> SetMapWalls()
        {
            List<WallBoundaries> bounds = new List<WallBoundaries>();

            // Outer Walls
            bounds.Add(new WallBoundaries(new Vector3(-29.4f, 1.0f, -29.4f), new Vector3(29.4f, 1.0f, -29.0f))); // Top 
            bounds.Add(new WallBoundaries(new Vector3(29.0f, 1.0f, -29.4f), new Vector3(29.0f, 1.0f, 29.4f)));   // Right
            bounds.Add(new WallBoundaries(new Vector3(29.4f, 1.0f, 29.4f), new Vector3(-29.4f, 1.0f, 29.0f))); // Bot
            bounds.Add(new WallBoundaries(new Vector3(-29.0f, 1.0f, 29.4f), new Vector3(-29.0f, 1.0f, -29.4f))); // Left

            // Outer Corridor Walls
            bounds.Add(new WallBoundaries(new Vector3(-14.5f, 1.0f, -24.0f), new Vector3(14.5f, 1.0f, -24.0f))); // Top
            bounds.Add(new WallBoundaries(new Vector3(-14.5f, 1.0f, 24.0f), new Vector3(14.5f, 1.0f, 24.0f))); // Top
            bounds.Add(new WallBoundaries(new Vector3(24.0f, 1.0f, -14.5f), new Vector3(24.0f, 1.0f, 14.5f))); // Right
            bounds.Add(new WallBoundaries(new Vector3(-24.0f, 1.0f, -14.5f), new Vector3(-24.0f, 1.0f, 14.5f))); // Left

            // Inner Corridor Walls
            bounds.Add(new WallBoundaries(new Vector3(-14.5f, 1.0f, -20.0f), new Vector3(-2.5f, 1.0f, -20.0f))); // Top 1
            bounds.Add(new WallBoundaries(new Vector3(14.5f, 1.0f, -20.0f), new Vector3(2.5f, 1.0f, -20.0f))); // Top 2
            bounds.Add(new WallBoundaries(new Vector3(-14.5f, 1.0f, 20.0f), new Vector3(-2.5f, 1.0f, 20.0f))); // Bot 1
            bounds.Add(new WallBoundaries(new Vector3(14.5f, 1.0f, 20.0f), new Vector3(2.5f, 1.0f, 20.0f))); // Bot 2
            bounds.Add(new WallBoundaries(new Vector3(20.0f, 1.0f, -14.5f), new Vector3(20.0f, 1.0f, -2.5f))); // Right 1
            bounds.Add(new WallBoundaries(new Vector3(20.0f, 1.0f, 2.5f), new Vector3(20.0f, 1.0f, 14.5f))); // Right 2
            bounds.Add(new WallBoundaries(new Vector3(-20.0f, 1.0f, -14.5f), new Vector3(-20.0f, 1.0f, -2.5f))); // Left 1
            bounds.Add(new WallBoundaries(new Vector3(-20.0f, 1.0f, 2.5f), new Vector3(-20.0f, 1.0f, 14.5f))); // Left 2

            // Cross Corridors
            bounds.Add(new WallBoundaries(new Vector3(-2.0f, 1.0f, -19.5f), new Vector3(-2.0f, 1.0f, -7.5f))); // Top L
            bounds.Add(new WallBoundaries(new Vector3(2.0f, 1.0f, -19.5f), new Vector3(2.0f, 1.0f, -7.5f))); // Top R
            bounds.Add(new WallBoundaries(new Vector3(-2.0f, 1.0f, 7.5f), new Vector3(-2.0f, 1.0f, 19.5f))); // Bot L
            bounds.Add(new WallBoundaries(new Vector3(2.0f, 1.0f, 7.5f), new Vector3(2.0f, 1.0f, 19.5f))); // Bot R
            bounds.Add(new WallBoundaries(new Vector3(7.5f, 1.0f, -2.0f), new Vector3(19.5f, 1.0f, -2.0f)));
            bounds.Add(new WallBoundaries(new Vector3(7.5f, 1.0f, 2.0f), new Vector3(19.5f, 1.0f, 2.0f)));
            bounds.Add(new WallBoundaries(new Vector3(-7.5f, 1.0f, -2.0f), new Vector3(-19.5f, 1.0f, -2.0f)));
            bounds.Add(new WallBoundaries(new Vector3(-7.5f, 1.0f, 2.0f), new Vector3(-19.5f, 1.0f, 2.0f)));

            // Center room
            bounds.Add(new WallBoundaries(new Vector3(2.5f, 1.0f, -7.0f), new Vector3(7.5f, 1.0f, -7.0f)));
            bounds.Add(new WallBoundaries(new Vector3(-2.5f, 1.0f, -7.0f), new Vector3(-7.5f, 1.0f, -7.0f)));
            bounds.Add(new WallBoundaries(new Vector3(2.5f, 1.0f, 7.0f), new Vector3(7.5f, 1.0f, 7.0f)));
            bounds.Add(new WallBoundaries(new Vector3(-2.5f, 1.0f, 7.0f), new Vector3(-7.5f, 1.0f, 7.0f)));

            bounds.Add(new WallBoundaries(new Vector3(7.0f, 1.0f, -7.0f), new Vector3(7.0f, 1.0f, -2.5f)));
            bounds.Add(new WallBoundaries(new Vector3(7.0f, 1.0f, 7.0f), new Vector3(7.0f, 1.0f, 2.5f)));
            bounds.Add(new WallBoundaries(new Vector3(-7.0f, 1.0f, -7.0f), new Vector3(-7.0f, 1.0f, -2.5f)));
            bounds.Add(new WallBoundaries(new Vector3(-7.0f, 1.0f, 7.0f), new Vector3(-7.0f, 1.0f, 2.5f)));
            // Spawn Room
            bounds.Add(new WallBoundaries(new Vector3(-15f, 1.0f, -29.4f), new Vector3(-15f, 1.0f, -24.4f)));
            bounds.Add(new WallBoundaries(new Vector3(-15f, 1.0f, -19.4f), new Vector3(-15f, 1.0f, -14.4f)));
            bounds.Add(new WallBoundaries(new Vector3(-15f, 1.0f, -15f), new Vector3(-19.5f, 1.0f, -15f)));
            bounds.Add(new WallBoundaries(new Vector3(-24.5f, 1.0f, -15f), new Vector3(-29.4f, 1.0f, -15f)));
            // Room 1
            bounds.Add(new WallBoundaries(new Vector3(15f, 1.0f, -29.4f), new Vector3(15f, 1.0f, -24.4f)));
            bounds.Add(new WallBoundaries(new Vector3(15f, 1.0f, -19.4f), new Vector3(15f, 1.0f, -14.4f)));
            bounds.Add(new WallBoundaries(new Vector3(15f, 1.0f, -15f), new Vector3(19.5f, 1.0f, -15f)));
            bounds.Add(new WallBoundaries(new Vector3(24.5f, 1.0f, -15f), new Vector3(29.4f, 1.0f, -15f)));
            // Room 2
            bounds.Add(new WallBoundaries(new Vector3(15f, 1.0f, 29.4f), new Vector3(15f, 1.0f, 24.4f)));
            bounds.Add(new WallBoundaries(new Vector3(15f, 1.0f, 19.4f), new Vector3(15f, 1.0f, 14.4f)));
            bounds.Add(new WallBoundaries(new Vector3(15f, 1.0f, 15f), new Vector3(19.5f, 1.0f, 15f)));
            bounds.Add(new WallBoundaries(new Vector3(24.5f, 1.0f, 15f), new Vector3(29.4f, 1.0f, 15f)));
            // Room 3
            bounds.Add(new WallBoundaries(new Vector3(-15f, 1.0f, 29.4f), new Vector3(-15f, 1.0f, 24.4f)));
            bounds.Add(new WallBoundaries(new Vector3(-15f, 1.0f, 19.4f), new Vector3(-15f, 1.0f, 14.4f)));
            bounds.Add(new WallBoundaries(new Vector3(-15f, 1.0f, 15f), new Vector3(-19.5f, 1.0f, 15f)));
            bounds.Add(new WallBoundaries(new Vector3(-24.5f, 1.0f, 15f), new Vector3(-29.4f, 1.0f, 15f)));

            return bounds;
        }
    }
}
