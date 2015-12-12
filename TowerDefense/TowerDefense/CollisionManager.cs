using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TowerDefense
{
    public class CollisionManager
    {
        private static Game InternalGame;
        private static Texture2D InternalCollisionTexture;

        public static List<Color> CollisionColors;

        public static Texture2D CollisionTexture
        {
            get
            {
                return InternalCollisionTexture;
            }
        }

        public static void Create(Game game)
        {
            InternalGame = game;
            InternalCollisionTexture = InternalGame.Content.Load<Texture2D>("Images/map");

            CollisionColors = new List<Color>();
            CollisionColors.Add(Color.Black);
        }

        public static CollisionInfo CalculateCollision(Rectangle rectangle, bool invert = false)
        {
            CollisionInfo result = new CollisionInfo();

            rectangle.X = rectangle.X < 0 ? 0 : rectangle.X;
            rectangle.Y = rectangle.Y < 0 ? 0 : rectangle.Y;
            rectangle.X = InternalCollisionTexture.Width - rectangle.X < rectangle.Width ? InternalCollisionTexture.Width - rectangle.Width : rectangle.X;
            rectangle.Y = InternalCollisionTexture.Height - rectangle.Y < rectangle.Height ? InternalCollisionTexture.Height - rectangle.Height : rectangle.Y;

            int pixelCount = rectangle.Width * rectangle.Height;
            Color[] pixels = new Color[pixelCount];

            InternalCollisionTexture.GetData(0, rectangle, pixels, 0, pixelCount);

            foreach (Color pixel in pixels)
                if (CollisionColors.Contains(pixel))
                {
                    result.Color = pixel;
                    result.Collided = !invert;

                    return result;
                }

            result.Collided = invert;
            return result;
        }

        public struct CollisionInfo
        {
            public bool Collided;
            public Color Color;
        }

        public struct RayInfo
        {
            public bool Collided;
            public Vector2 Position;
            public Vector2 Normal;
            public Color Color;
        }

        public static RayInfo Raycast(Vector2 start, Vector2 end, bool inverse = false, float step = 0.02f)
        {
            // Initialize the result struct
            RayInfo result = new RayInfo();
            result.Collided = false;
            result.Position = start;

            //  What is the difference between where we are now and there?
            Vector2 delta = end - start;

            // What is the distance traveled from here to there?
            float traveledDistance = (float)Math.Sqrt(Math.Pow(end.X - start.X, 2) + Math.Pow(end.Y - start.Y, 2));

            // We take a normal of the delta and use that to produce our stepping
            delta.Normalize();
            result.Normal = delta;
            delta *= step;

            Rectangle currentRectangle = new Rectangle(0, 0, 1, 1);
            int iterationCount = (int)Math.Ceiling(traveledDistance / delta.Length());
            for (int iteration = 0; iteration < iterationCount; iteration++)
            {
                result.Position += delta * iteration;
                currentRectangle.X = (int)result.Position.X;
                currentRectangle.Y = (int)result.Position.Y;

                CollisionInfo collision = CalculateCollision(currentRectangle, inverse);
                if (collision.Collided)
                {
                    result.Collided = collision.Collided;
                    result.Color = collision.Color;

                    return result;
                }
            }

            return result;
        }

        public static RayInfo Raycast(Vector2 start, float theta, float distance, bool inverse = false, float step = 0.02f)
        {
            // Which way are we going? We negate the Y component of the direction because XNA uses a coordinate system in which 0,0
            // is the top left corner of the screen, therefore +Y is down.           
            Vector2 direction = new Vector2((float)Math.Cos(theta), -(float)Math.Sin(theta));

            // Where are we going to end up?
            Vector2 end = start + (direction * distance);

            return Raycast(start, end, inverse, step);
        }

        private CollisionManager()
        {

        }
    }
}
