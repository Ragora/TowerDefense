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
    public class DrawHelpers
    {
        private static Texture2D WhiteTexture;

        public static void Create(Game game)
        {
            WhiteTexture = game.Content.Load<Texture2D>("Images/white");
        }

        public static void DrawLine(SpriteBatch batch, Vector2 start, Vector2 end, Color color, int thickness = 3)
        {
            // First, we need a delta & a distance
            Vector2 delta = start - end;
            float distance = delta.Length();

            // Now we need a normalized delta for the angle
            delta.Normalize();
            float theta = (float)(Math.Atan2(delta.Y, delta.X));
            theta += MathHelper.Pi;

            // Build the rectangle & draw
            Rectangle drawnRectangle = new Rectangle((int)start.X, (int)start.Y, (int)distance, thickness);
            batch.Draw(WhiteTexture, drawnRectangle, null, color, theta, new Vector2(0, 0), SpriteEffects.None, 0);
        }

        public static void DrawPoint(SpriteBatch batch, Vector2 position, Color color, int thickness = 30)
        {
            Rectangle drawnRectangle = new Rectangle((int)position.X, (int)position.Y, thickness, thickness);

            batch.Draw(WhiteTexture, drawnRectangle, null, color, 0, new Vector2((float)WhiteTexture.Width / 2, (float)WhiteTexture.Height / 2), 
                       SpriteEffects.None, 0);
        }

        private DrawHelpers()
        {

        }
    }
}
