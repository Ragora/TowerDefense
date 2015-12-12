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
    public class AIManager
    {
        public static bool EnablePathDebug = false;

        private static List<AINode> TopNodes;

        public class AINode
        {
            public Vector2 Position;
            public List<AINode> NextNodes;

            public AINode(Vector2 position)
            {
                NextNodes = new List<AINode>();
                Position = position;
            }
        };

        private static Color[] PointColors =
        {
            Color.Red,
            Color.Green,
            Color.Blue,
        };

        private static Color[] LineColors =
        {
            Color.Blue,
            Color.Red,
            Color.Green,
        };

        public static void Create()
        {
            TopNodes = new List<AINode>();
        }

        public static void PlotAIPaths()
        {
            Console.WriteLine("AIManager:: Plotting AI Paths Stage 1 ...");

            TopNodes.Clear();

            // First we locate spawn pixels.
            Rectangle rectangle = new Rectangle(0, 0, CollisionManager.CollisionTexture.Width, CollisionManager.CollisionTexture.Height);
            int pixelCount = CollisionManager.CollisionTexture.Width * CollisionManager.CollisionTexture.Height;
            Color[] pixels = new Color[pixelCount];
            CollisionManager.CollisionTexture.GetData(0, rectangle, pixels, 0, pixelCount);

            Color searchPixel = new Color(0, 255, 0);
            for (int pixel = 0; pixel < pixelCount; pixel++)
            {
                if (pixels[pixel] == searchPixel)
                {
                    Point spawnPoint = new Point(pixel % CollisionManager.CollisionTexture.Width, 
                                                 pixel / CollisionManager.CollisionTexture.Height);

                    AINode node = new AINode(new Vector2(spawnPoint.X, spawnPoint.Y));
                    TopNodes.Add(node);
                    Console.WriteLine("AIManager:: Found spawn point at {0},{1}", spawnPoint.X, spawnPoint.Y);
                }
            }

            if (TopNodes.Count == 0)
                Console.WriteLine("AIManager:: Failed to find a viable spawn point!");
            else
                PlotAIPaths_StageTwo();
        }

        private static void PlotAIPaths_StageTwo()
        {
            Color endColor = new Color(0, 0, 255);
            CollisionManager.CollisionColors.Add(endColor);

            Console.WriteLine("AIManager:: Plotting AI Paths Stage 2 ...");

            foreach (AINode topNode in TopNodes)
            {
                AINode currentNode = topNode;

                // We assume downward start directions for now
                float theta = (3 * MathHelper.Pi) / 2;

                for (int iteration = 0; iteration < 4; iteration++)
                {
                    CollisionManager.RayInfo ray = CollisionManager.Raycast(currentNode.Position, theta, 200);

                    if (ray.Collided)
                    {
                        // We travel a short distance backwards on our normal to prevent future raycasts from being doomed
                        Vector2 reverseDistance = ray.Normal * 10;

                        AINode newNode = new AINode(ray.Position - reverseDistance);
                        currentNode.NextNodes.Add(newNode);
                        currentNode = newNode;

                        // Now we two other casts to find a viable turn. For now, we take the direction with the largest distance
                        for (float currentOffset = MathHelper.PiOver2; currentOffset > 0; currentOffset -= 0.05f)
                        {
                            float negativeTheta = theta - currentOffset;
                            float positiveTheta = theta + currentOffset;

                            CollisionManager.RayInfo rayNegative = CollisionManager.Raycast(currentNode.Position, negativeTheta, 200);
                            CollisionManager.RayInfo rayPositive = CollisionManager.Raycast(currentNode.Position, positiveTheta, 200);

                            float distanceNegative = Vector2.Distance(currentNode.Position, rayNegative.Position);
                            float distancePositive = Vector2.Distance(currentNode.Position, rayPositive.Position);

                            bool pathNegative = distanceNegative > distancePositive ? true : false;

                            if (pathNegative && distanceNegative > 20)
                            {
                                reverseDistance = rayNegative.Normal * 10;

                                AINode turnNode = new AINode(rayNegative.Position - reverseDistance);
                                currentNode.NextNodes.Add(turnNode);
                                currentNode = newNode;

                                theta = (float)Math.Atan2(-rayNegative.Normal.Y, rayNegative.Normal.X);
                                break;
                            }
                            else if (distancePositive > 20)
                            {
                                reverseDistance = rayPositive.Normal * 10;

                                AINode turnNode = new AINode(rayPositive.Position - reverseDistance);
                                currentNode.NextNodes.Add(turnNode);
                                currentNode = newNode;

                                theta = (float)Math.Atan2(-rayPositive.Normal.Y, rayPositive.Normal.X);
                                break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("AIManager:: End of AI Graph?");
                        break;
                    }
                }
            }
        }

        private static void RecurseNode(AINode node, ref List<AINode> nodes)
        {
            nodes.Add(node);

            foreach (AINode nextNode in node.NextNodes)
                RecurseNode(nextNode, ref nodes);
        }

        public static void Draw(SpriteBatch batch)
        {
            if (EnablePathDebug)
            {
                int currentColor = 0;

                foreach (AINode node in TopNodes)
                {
                    List<AINode> nodes = new List<AINode>();
                    RecurseNode(node, ref nodes);

                    for (int iteration = 0; iteration < nodes.Count; iteration++)
                    {
                        DrawHelpers.DrawPoint(batch, nodes[iteration].Position, PointColors[currentColor]);
                        if (iteration != nodes.Count - 1)
                            DrawHelpers.DrawLine(batch, nodes[iteration].Position, nodes[iteration + 1].Position, LineColors[currentColor]);
                    }

                    ++currentColor;
                    currentColor %= LineColors.Length;
                }
            }
        }

        private AIManager()
        {

        }
    }
}
