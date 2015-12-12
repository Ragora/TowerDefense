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
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public SpriteFont Arial;

        public static Point Resolution = new Point(800, 600);

        public Texture2D CursorTexture;
        public Texture2D SmallTexture;
        public Texture2D MediumTexture;
        public Texture2D LargeTexture;
        public Texture2D BlockerTexture;

        public Texture2D MapTexture;

        public List<ATower> Towers;

        public bool IsObjectPlacable;

        public GUI.PlayGUI PlayGUI;

        public CollisionManager.RayInfo Ray;

        public enum SelectedObject
        {
            None,
            Small,
            Medium,
            Large,
            Blocker,
        }

        public SelectedObject Selected;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = Resolution.X;
            graphics.PreferredBackBufferHeight = Resolution.Y;

            Ray = new CollisionManager.RayInfo();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        private void OnCursorClicked(Vector2 cursorPosition)
        {
            ATower newTower = null;

            if (!IsObjectPlacable)
                return;

            switch (Selected)
            {
                case SelectedObject.Small:
                    {
                        newTower = new STower(this)
                        {
                            Position = GUI.GUIManager.CursorPosition,
                        };

                        break;
                    }
                case SelectedObject.Medium:
                    {
                        newTower = new MTower(this)
                        {
                            Position = GUI.GUIManager.CursorPosition,
                        };
                        break;
                    }
                case SelectedObject.Large:
                    {
                        newTower = new LTower(this)
                        {
                            Position = GUI.GUIManager.CursorPosition,
                        };
                        break;
                    }
                case SelectedObject.Blocker:
                    {
                        newTower = new BTower(this)
                        {
                            Position = GUI.GUIManager.CursorPosition,
                        };
                        break;
                    }
                case SelectedObject.None:
                    {
                        Point cursorPoint = new Point((int)GUI.GUIManager.CursorPosition.X, (int)GUI.GUIManager.CursorPosition.Y);

                        ATower selectedTower = null;
                        foreach (ATower tower in Towers)
                            if (tower.Rectangle.Contains(cursorPoint))
                            {
                                selectedTower = tower;
                                break;
                            }

                        if (selectedTower == null)
                            return;

                        if (selectedTower is STower)
                        {
                            GUI.GUIManager.CursorTexture = SmallTexture;
                            Selected = SelectedObject.Small;
                        }
                        else if (selectedTower is MTower)
                        {
                            GUI.GUIManager.CursorTexture = MediumTexture;
                            Selected = SelectedObject.Medium;
                        }
                        else if (selectedTower is LTower)
                        {
                            GUI.GUIManager.CursorTexture = LargeTexture;
                            Selected = SelectedObject.Large;
                        }
                        else if (selectedTower is BTower)
                        {
                            GUI.GUIManager.CursorTexture = BlockerTexture;
                            Selected = SelectedObject.Blocker;
                        }

                        Towers.Remove(selectedTower);
                        GUI.GUIManager.CursorColor = Color.Green;
                        return;
                    }
            }

            if (newTower != null)
            {
                newTower.Initialize();
                Towers.Add(newTower);
            }

            Selected = SelectedObject.None;
            GUI.GUIManager.CursorTexture = this.CursorTexture;
            GUI.GUIManager.CursorColor = Color.White;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            DrawHelpers.Create(this);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Towers = new List<ATower>();

            this.Arial = this.Content.Load<SpriteFont>("Fonts/Arial");
            this.CursorTexture = this.Content.Load<Texture2D>("Images/cursor");
            this.SmallTexture = this.Content.Load<Texture2D>("Images/small");
            this.MediumTexture = this.Content.Load<Texture2D>("Images/medium");
            this.LargeTexture = this.Content.Load<Texture2D>("Images/large");
            this.BlockerTexture = this.Content.Load<Texture2D>("Images/blocker");
            this.MapTexture = this.Content.Load<Texture2D>("Images/map");

            CollisionManager.Create(this);

            InputManager.Create();
            InputManager.UseKeyboard = false;
            InputManager.UseController = true;

            GUI.GUIManager.BindControllerListeners();

            GUI.GUIManager.Create(this);
            GUI.GUIManager.OnCursorDown = this.OnCursorClicked;
            GUI.GUIManager.CursorTexture = this.CursorTexture;

            PlayGUI = new GUI.PlayGUI(this);
            GUI.GUIManager.AddGUI(PlayGUI, "play");
            GUI.GUIManager.Initialize();

            GUI.GUIManager.SetGUI("play");

            AIManager.Create();
            AIManager.PlotAIPaths();

            AIManager.EnablePathDebug = true;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            GUI.GUIManager.Update(gameTime);
            InputManager.Update(gameTime);

            Point cursorPosition = new Point((int)GUI.GUIManager.CursorPosition.X, (int)GUI.GUIManager.CursorPosition.Y);
            foreach (ATower tower in Towers)
                if (tower.Rectangle.Contains(cursorPosition))
                    tower.Color = Color.Red;
                else
                    tower.Color = Color.White;

            if (Selected != SelectedObject.None)
            {
                IsObjectPlacable = GUI.GUIManager.CursorPosition.Y > 80;

                GUI.GUIManager.CursorColor = IsObjectPlacable ? Color.Green : Color.Red;
            }
            else
            {
                IsObjectPlacable = true;
                GUI.GUIManager.CursorColor = Color.White;
            }

            Ray = CollisionManager.Raycast(GUI.GUIManager.CursorPosition, 0, 5, false);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(MapTexture, new Vector2(0, 0), Color.White);

            foreach (ATower tower in Towers)
                tower.Draw(spriteBatch);

            GUI.GUIManager.Draw(spriteBatch);

            spriteBatch.Draw(LargeTexture, Ray.Position, Color.Green);

            AIManager.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
