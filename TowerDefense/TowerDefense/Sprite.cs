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
using System.Timers;

namespace TowerDefense
{
    /// <summary>
    /// A class representing a basic sprite.
    /// </summary>
    public class Sprite : IDrawable
    {
        public SpriteEffects Effects { get; set; }

        /// <summary>
        /// The position to be drawn at.
        /// </summary>
        public Vector2 Position { get; set; }
        
        /// <summary>
        /// Whether or not we're currently being drawn.
        /// </summary>
        public bool Drawn { get; set; }

        /// <summary>
        /// The scale factor.
        /// </summary>
        public float Scale { get; set; }

        /// <summary>
        /// The color to draw with.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// The draw origin. This is essentially the draw offset relative to the top left
        /// corner of the image.
        /// </summary>
        public Vector2 Origin { get; set; }

        /// <summary>
        /// The rotation to draw the sprite at.
        /// </summary>
        public float Theta { get; set; }

        /// <summary>
        /// A read-only property that returns a Rectangle representing the collision bounds of this sprite.
        /// </summary>
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, InternalTexture.Width, InternalTexture.Height);
            }
        }

        /// <summary>
        /// The game instance this sprite is associated with.
        /// </summary>
        protected Game InternalGame;

        /// <summary>
        /// The texture to be drawn.
        /// </summary>
        protected Texture2D InternalTexture;

        /// <summary>
        /// The path to the texture to load during initialization.
        /// </summary>
        protected String InternalTexturePath;

        /// <summary>
        /// A constructor accepting a game instance and a texture path.
        /// </summary>
        /// <param name="game">
        /// The game instance this sprite is associated with.
        /// </param>
        /// <param name="texturePath">
        /// The path to the texture to be drawn.
        /// </param>
        public Sprite(Game game, String texturePath)
        {
            Drawn = true;
            InternalTexturePath = texturePath;
            InternalGame = game;

            Scale = 1.0f;
            Color = Color.White;
            Origin = new Vector2(0, 0);
        }
        
        /// <summary>
        /// Initializes the sprite by loading the requested texture.
        /// </summary>
        public virtual void Initialize()
        {
            InternalTexture = InternalGame.Content.Load<Texture2D>(InternalTexturePath);
        }

        /// <summary>
        /// Draws the sprite to the frame buffer.
        /// </summary>
        /// <param name="batch">
        /// The sprite batch to draw to.
        /// </param>
        public virtual void Draw(SpriteBatch batch)
        {
            if (!Drawn)
                return;

            batch.Draw(InternalTexture, Position, null, Color, Theta, new Vector2(0, 0), Scale, this.Effects, 0);
        }
    }
}
