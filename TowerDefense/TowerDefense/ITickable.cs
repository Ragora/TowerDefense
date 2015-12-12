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
    /// An interface type for objects can receive update pulses.
    /// </summary>
    interface ITickable
    {
        /// <summary>
        /// A boolean controlling whether or not this ITickable is being updated.
        /// </summary>
        bool Updated { set; get; }

        /// <summary>
        /// The update method that all ITickable objects must implement.
        /// </summary>
        /// <param name="time">
        /// The GameTime passed in by the game's main Update method.
        /// </param>
        void Update(GameTime time);
    }
}
