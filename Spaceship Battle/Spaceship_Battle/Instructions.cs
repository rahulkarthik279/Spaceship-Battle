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

namespace Spaceship_Battle
{
    class Instructions
    {
        static List<string> instructions;
        public static Button backButton;
        //static GamePadState oldPad = GamePad.GetState(PlayerIndex.One);
        public static void LoadContent()
        {
            instructions = new List<string>();
            backButton = new Button(new Rectangle(0, 0, 180, 70), new Rectangle[] { new Rectangle(125, 145, 145, 55), new Rectangle(125, 75, 145, 55) }, "Back to Menu");
            instructions.Add("CONTROLS FOR MOUSE AND KEYBOARD:");
            instructions.Add("Arrow keys to move.");
            instructions.Add("Click the left button of the mouse to fire a bullet and the space bar to fire a fireball.");
            instructions.Add("Click the pause button to pause the game.");
            instructions.Add("Move the mouse around to face in a certain direction.");
            instructions.Add("");
            instructions.Add("CONTROLS FOR GAMEPAD CONTROLLER:");
            instructions.Add("Left joystick to move.");
            instructions.Add("Press A to fire a bullet/missile and B to fire a fireball.");
            instructions.Add("Press the Back button to pause/resume the game.");
            instructions.Add("If you want to exit, press back to pause then R1 to save and exit.");
            instructions.Add("Move the left joystick around to face in a certain direction.");
            instructions.Add("");
            instructions.Add("GENERAL RULES:");
            instructions.Add("Hit flashing color boxes for powerups.");
            instructions.Add("Click the mouse to fire a bullet at the specified location.");
            instructions.Add("It takes two bullet hits to kill an enemy.");
            instructions.Add("It takes one hit from a fireball to kill an enemy.");
            instructions.Add("As you beat each level, you gain an extra weapon (fireball, then missile).");
            instructions.Add("And finally, BEWARE OF GRAVITY!!!!");

        }
        public static void draw(SpriteBatch sb, GameTime gt)
        {
            int x = 50;
            int y = 90;
            for (int i = 0; i < instructions.Count; i++)
            {
                sb.DrawString(StartMenu.font, instructions[i], new Vector2(x, y), Color.White);
                y += 25;
            }
            sb.Draw(StartMenu.spritesheet, backButton.drect, backButton.srect, Color.White);
            float startstringX = backButton.drect.X + backButton.drect.Width / 2.0f - backButton.insideText.Length * 4.5f;
            sb.DrawString(StartMenu.font, backButton.insideText, new Vector2(startstringX, backButton.drect.Y + 20), Color.White);
        }
        public static int update()
        {
            MouseState m = Mouse.GetState();
            Point mousepos = new Point(m.X, m.Y);
            GamePadState newPad = GamePad.GetState(PlayerIndex.One);
            if (newPad.Buttons.Back == ButtonState.Pressed)//&& oldPad.Buttons.Back == ButtonState.Released)
            {
                //oldPad = newPad;
                return -2;
            }
            if (backButton.drect.Contains(mousepos))
            {
                backButton.setActive(true);
                if (m.LeftButton == ButtonState.Pressed)
                {
                    return -2;
                }
            }
            else
            {
                backButton.setActive(false);
            }
            return 0;

        }
    }
}