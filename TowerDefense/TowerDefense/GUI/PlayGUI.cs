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

namespace TowerDefense.GUI
{
    /// <summary>
    /// The PlayGUI is the GUI that is drawn when the player is currently playing the game --
    /// in control of the player object.
    /// </summary>
    public class PlayGUI : GUI
    {
        /// <summary>
        /// The GUI to be drawn when the player is playing the game.
        /// </summary>
        /// <param name="game">
        /// The game instance to associate this new PlayGUI with.
        /// </param>
        public PlayGUI(Game game) : base(game)
        {
            // Add the buttons
            Elements.Button smallButton = new Elements.Button(game, "fonts/Arial", "", "Images/small", "Images/small")
            {
                Position = new Vector2(0, 20),
                Responder = this.OnSmallPressed,
            };

            this.AddElement(smallButton);

            Elements.Button mediumButton = new Elements.Button(game, "fonts/Arial", "", "Images/medium", "Images/medium")
            {
                Position = new Vector2(100, 20),
                Responder = this.OnMediumPressed,
            };

            this.AddElement(mediumButton);

            Elements.Button largeButton = new Elements.Button(game, "fonts/Arial", "", "Images/large", "Images/large")
            {
                Position = new Vector2(200, 20),
                Responder = this.OnLargePressed,
            };

            this.AddElement(largeButton);

            Elements.Button blockerButton = new Elements.Button(game, "fonts/Arial", "", "Images/blocker", "Images/blocker")
            {
                Position = new Vector2(300, 20),
                Responder = this.OnBlockerPressed,
            };

            this.AddElement(blockerButton);
        }

        private void OnSmallPressed()
        {
            if (InternalGame.Selected != Game.SelectedObject.None)
            {
                InternalGame.Selected = Game.SelectedObject.None;
                GUIManager.CursorTexture = InternalGame.CursorTexture;
                return;
            }

            InternalGame.Selected = Game.SelectedObject.Small;
            GUIManager.CursorTexture = InternalGame.SmallTexture;
            GUIManager.CursorColor = Color.Green;
        }

        private void OnMediumPressed()
        {
            if (InternalGame.Selected != Game.SelectedObject.None)
            {
                InternalGame.Selected = Game.SelectedObject.None;
                GUIManager.CursorTexture = InternalGame.CursorTexture;
                return;
            }

            InternalGame.Selected = Game.SelectedObject.Medium;
            GUIManager.CursorTexture = InternalGame.MediumTexture;
            GUIManager.CursorColor = Color.Green;
        }

        private void OnLargePressed()
        {
            if (InternalGame.Selected != Game.SelectedObject.None)
            {
                InternalGame.Selected = Game.SelectedObject.None;
                GUIManager.CursorTexture = InternalGame.CursorTexture;
                return;
            }

            InternalGame.Selected = Game.SelectedObject.Large;
            GUIManager.CursorTexture = InternalGame.LargeTexture;
            GUIManager.CursorColor = Color.Green;
        }

        private void OnBlockerPressed()
        {
            if (InternalGame.Selected != Game.SelectedObject.None)
            {
                InternalGame.Selected = Game.SelectedObject.None;
                GUIManager.CursorTexture = InternalGame.CursorTexture;
                return;
            }

            InternalGame.Selected = Game.SelectedObject.Blocker;
            GUIManager.CursorTexture = InternalGame.BlockerTexture;
            GUIManager.CursorColor = Color.Green;
        }
    }
}
