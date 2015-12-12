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
    /// <summary>
    /// An interface type for all drawable objects.
    /// </summary>
    interface IDrawable
    {
        /// <summary>
        /// The initialize method that should be used to create and load resources
        /// required for the drawing of the IDrawable object, such as loading textures.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Draws the IDrawable to the frame buffer.
        /// </summary>
        /// <param name="batch">
        /// The sprite batch to draw to.
        /// </param>
        void Draw(SpriteBatch batch);

        /// <summary>
        /// The position to be drawn at.
        /// </summary>
        Vector2 Position { get; set; }

        /// <summary>
        /// A boolean controlling whether or not this IDrawable is being drawn.
        /// </summary>
        bool Drawn { get; set; }

        /// <summary>
        /// The scale factor.
        /// </summary>
        float Scale { get; set; }

        /// <summary>
        /// The color to be drawn with.
        /// </summary>
        Color Color { get; set; }

        /// <summary>
        /// The draw origin of this IDrawable. It is essentially a draw offset relative to the top left
        /// corner of the image.
        /// </summary>
        Vector2 Origin { get; set; }

        /// <summary>
        /// The rotation to be drawn at.
        /// </summary>
        float Theta { get; set; }

        /// <summary>
        /// This property should return the collision bounds of the IDrawable
        /// represented via a Rectangle.
        /// </summary>
        Rectangle Rectangle { get; }

        SpriteEffects Effects { get; set; }
    }
}
