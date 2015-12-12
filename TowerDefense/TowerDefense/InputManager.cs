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
    /// A static manager used to handle the player's input in the game.
    /// </summary>
    public class InputManager
    {
        /// <summary>
        /// A delegate that is used for responding to binary inputs.
        /// </summary>
        /// <param name="pressed">
        /// A boolean representing whether or not the input is active.
        /// </param>
        public delegate void BinaryResponder(bool pressed);

        /// <summary>
        /// A delegate that is used for responding to analog inputs.
        /// </summary>
        /// <param name="amount">
        /// The amount the input currently outputting.
        /// </param>
        public delegate void AnalogResponder(float amount);

        /// <summary>
        /// A delegate that is used for responding to analog inputs with two channels.
        /// </summary>
        /// <param name="amountOne">
        /// The amount the first channel currently outputting.
        /// </param>
        /// <param name="amountTwo">
        /// The amount the second channel currently outputting.
        /// </param>
        public delegate void DualAnalogListener(float amountOne, float amountTwo);

        /// <summary>
        /// A mapping of keys to their respective binary responders.
        /// </summary>
        private static SortedDictionary<Keys, BinaryResponder> KeyMap;

        /// <summary>
        /// A mapping of buttons to their respective binary responders.
        /// </summary>
        private static SortedDictionary<GamePadButtons, BinaryResponder> ButtonMap;

        /// <summary>
        /// The dual analog responder to be called for the left control stick when the controller
        /// subsystem is active.
        /// </summary>
        public static DualAnalogListener LeftStickListener;

        /// <summary>
        /// The analog responder to be called for the left trigger when the controller
        /// subsystem is active.
        /// </summary>
        public static AnalogResponder LeftTriggerListener;

        /// <summary>
        /// The dual analog responder to be called for the right control stick when the controller
        /// subsystem is active.
        /// </summary>
        public static DualAnalogListener RightStickListener;

        /// <summary>
        /// The analog responder to be called for the right trigger when the controller
        /// subsystem is active.
        /// </summary>
        public static AnalogResponder RightTriggerListener;
        
        /// <summary>
        /// The binary responder to be called when the directional pad down is pressed on the
        /// controller and the controller subsystem is active.
        /// </summary>
        public static BinaryResponder DPadDownListener;

        /// <summary>
        /// The binary responder to be called when the directional pad up is pressed on the
        /// controller and the controller subsystem is active.
        /// </summary>
        public static BinaryResponder DPadUpListener;

        /// <summary>
        /// The binary responder to be called when the directional pad down is pressed on the
        /// controller and the controller subsystem is active.
        /// </summary>
        public static BinaryResponder DPadLeftListener;

        /// <summary>
        /// The binary responder to be called when the directional pad up is pressed on the
        /// controller and the controller subsystem is active.
        /// </summary>
        public static BinaryResponder DPadRightListener;

        /// <summary>
        /// The binary responder to be called when the A button is pressed on the
        /// controller and the controller subsystem is active.
        /// </summary>
        public static BinaryResponder AButtonListener;

        /// <summary>
        /// The binary responder to be called when the start button is pressed on the
        /// controller and the controller subsystem is active.
        /// </summary>
        public static BinaryResponder StartButtonListener;

        /// <summary>
        /// The binary responder to be called when the right shoulder button is pressed on the
        /// controller and the controller subsystem is active.
        /// </summary>
        public static BinaryResponder RightShoulderListener;

        /// <summary>
        /// Whether or not the keyboard subsystem should be active.
        /// </summary>
        public static bool UseKeyboard;

        /// <summary>
        /// Whether or not the controller subsystem should be active.
        /// </summary>
        public static bool UseController;

        /// <summary>
        /// The last known state of the keyboard.
        /// </summary>
        private static KeyboardState LastKeyboardState;

        /// <summary>
        /// The last known state of the controller.
        /// </summary>
        private static GamePadState LastControllerState;

        /// <summary>
        /// Creates the input system and initializes it. This should only ever be called once.
        /// </summary>
        public static void Create()
        {
            KeyMap = new SortedDictionary<Keys, BinaryResponder>();

            LastKeyboardState = Keyboard.GetState();
            LastControllerState = GamePad.GetState(PlayerIndex.One);
        }

        public static Vector2 GetMousePosition()
        {
            MouseState mouse = Mouse.GetState();

            return new Vector2(mouse.X, mouse.Y);
        }

        /// <summary>
        /// Registers a binary responder to be called when a given keyboard key is
        /// pressed and the keyboard subsystem is active.
        /// </summary>
        /// <param name="key"></param>
        /// The key to respond to.
        /// <param name="entry">
        /// The binary responder to bind to the given key.
        /// </param>
        public static void SetKeyResponder(Keys key, BinaryResponder entry)
        {
            KeyMap[key] = entry;

            if (entry == null)
                KeyMap.Remove(key);
        }

        /// <summary>
        /// Registers a binary responder to be called when a given controller button is
        /// pressed and the controller subsystem is active.
        /// </summary>
        /// <param name="button"></param>
        /// The button to respond to.
        /// <param name="entry">
        /// The binary responder to bind to the given button.
        /// </param>
        public static void SetButtonResponder(GamePadButtons button, BinaryResponder entry)
        {
            ButtonMap[button] = entry;

            if (entry == null)
                ButtonMap.Remove(button);
        }

        /// <summary>
        /// Updates the input system.
        /// </summary>
        /// <param name="time">
        /// The GameTime passed in by the game's main Update method.
        /// </param>
        public static void Update(GameTime time)
        {
            // If we're using the keyboard, perform the necessary processing
            if (UseKeyboard)
            {
                KeyboardState currentKeyboardState = Keyboard.GetState();

                // Prevent the collection modification error...
                SortedDictionary<Keys, BinaryResponder> temporaryMap = new SortedDictionary<Keys, BinaryResponder>(KeyMap);

                // Blow through the pressed responders
                foreach (Keys key in temporaryMap.Keys)
                {
                    if (currentKeyboardState.IsKeyDown(key) && LastKeyboardState.IsKeyUp(key))
                        temporaryMap[key](true);
                    else if (currentKeyboardState.IsKeyUp(key) && LastKeyboardState.IsKeyDown(key))
                        temporaryMap[key](false);
                }

                LastKeyboardState = currentKeyboardState;
            }

            // If we're using the controller, perform the necessary processing.
            if (UseController)
            {
                GamePadState currentGamepadState = GamePad.GetState(PlayerIndex.One);

                // Check the analog listeners
                if (LeftStickListener != null)
                    LeftStickListener(currentGamepadState.ThumbSticks.Left.X, currentGamepadState.ThumbSticks.Left.Y);

                if (LeftTriggerListener != null)
                    LeftTriggerListener(currentGamepadState.Triggers.Left);

                if (RightStickListener != null)
                    RightStickListener(currentGamepadState.ThumbSticks.Right.X, currentGamepadState.ThumbSticks.Right.Y);

                if (RightTriggerListener != null)
                    RightTriggerListener(currentGamepadState.Triggers.Right);

                // Unfortunately, there appears to be no query for the 360 controller like there is on the keyboard, so we have explicit listeners for the buttons
                if (currentGamepadState.DPad.Down == ButtonState.Pressed && LastControllerState.DPad.Down != ButtonState.Pressed && DPadDownListener != null)
                    DPadDownListener(true);
                else if (currentGamepadState.DPad.Down != ButtonState.Pressed && LastControllerState.DPad.Down == ButtonState.Pressed && DPadDownListener != null)
                    DPadDownListener(false);

                if (currentGamepadState.DPad.Up == ButtonState.Pressed && LastControllerState.DPad.Up != ButtonState.Pressed && DPadUpListener != null)
                    DPadUpListener(true);
                else if (currentGamepadState.DPad.Up != ButtonState.Pressed && LastControllerState.DPad.Up == ButtonState.Pressed && DPadUpListener != null)
                    DPadUpListener(false);

                if (currentGamepadState.DPad.Left == ButtonState.Pressed && LastControllerState.DPad.Left != ButtonState.Pressed && DPadLeftListener != null)
                    DPadLeftListener(true);
                else if (currentGamepadState.DPad.Left != ButtonState.Pressed && LastControllerState.DPad.Left == ButtonState.Pressed && DPadLeftListener != null)
                    DPadLeftListener(false);

                if (currentGamepadState.DPad.Right == ButtonState.Pressed && LastControllerState.DPad.Right != ButtonState.Pressed && DPadRightListener != null)
                    DPadRightListener(true);
                else if (currentGamepadState.DPad.Right != ButtonState.Pressed && LastControllerState.DPad.Right == ButtonState.Pressed && DPadRightListener != null)
                    DPadRightListener(false);

                if (currentGamepadState.Buttons.A == ButtonState.Pressed && LastControllerState.Buttons.A != ButtonState.Pressed && AButtonListener != null)
                    AButtonListener(true);
                else if (currentGamepadState.Buttons.A != ButtonState.Pressed && LastControllerState.Buttons.A == ButtonState.Pressed && AButtonListener != null)
                    AButtonListener(false);

                if (currentGamepadState.Buttons.Start == ButtonState.Pressed && LastControllerState.Buttons.Start != ButtonState.Pressed && StartButtonListener != null)
                    StartButtonListener(true);
                else if (currentGamepadState.Buttons.Start != ButtonState.Pressed && LastControllerState.Buttons.Start == ButtonState.Pressed && StartButtonListener != null)
                    StartButtonListener(false);

                if (currentGamepadState.Buttons.RightShoulder == ButtonState.Pressed && LastControllerState.Buttons.RightShoulder != ButtonState.Pressed && RightShoulderListener != null)
                    RightShoulderListener(true);
                else if (currentGamepadState.Buttons.RightShoulder != ButtonState.Pressed && LastControllerState.Buttons.RightShoulder == ButtonState.Pressed && RightShoulderListener != null)
                    RightShoulderListener(false);

                LastControllerState = currentGamepadState;
            }
        }
    }
}
