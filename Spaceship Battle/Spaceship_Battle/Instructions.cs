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
        public static List<string> instructions;
        public static Button backButton;
        public Level level;
        public static void LoadContent()
        {
            instructions = new List<string>();
            backButton = new Button(new Rectangle(0, 0, 180, 70), new Rectangle[] { new Rectangle(125, 145, 145, 55), new Rectangle(125, 75, 145, 55) }, "Back to Menu");
            instructions.Add("Arrow keys/gamepad arrows to move.");
            instructions.Add("Hit flashing color boxes for powerups.");
            instructions.Add("Click the mouse to fire a bullet at the specified location.");
            instructions.Add("It takes two bullet hits to kill an enemy.");
        }
        public static void draw(SpriteBatch sb, GameTime gt)
        {
            int x = 100;
            int y = 90;
            for(int i = 0; i < instructions.Count; i++)
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
