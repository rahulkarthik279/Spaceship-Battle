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
    class StartMenu
    {
        Button[] buttons;
        public static Texture2D spritesheet;
        ContentManager content;
        public static SpriteFont font, bigfont;
        public static int[] buttonsize = new int[] { 250, 60 };
        Rectangle[] bluebutton = new Rectangle[] { new Rectangle(125, 145, 145, 55), new Rectangle(125, 75, 145, 55) };
        Rectangle[] yellowbutton = new Rectangle[] { new Rectangle(440, 148, 128, 55), new Rectangle(440, 80, 128, 55) };
        GamePadState oldPad = GamePad.GetState(PlayerIndex.One);
        public StartMenu(IServiceProvider service,int w, int h) {
            content = new ContentManager(service, "Content");
            spritesheet = content.Load<Texture2D>("Sci-Fi-buttons-cover");
            font = content.Load <SpriteFont>("menufont");
            bigfont = content.Load<SpriteFont>("menufontbig");
            buttons = new Button[3];
            buttons[0] = new Button(new Rectangle((w-buttonsize[0])/2, 250, buttonsize[0], buttonsize[1]), bluebutton, "Continue Single Player");
            buttons[1] = new Button(new Rectangle((w - buttonsize[0]) / 2, 350, buttonsize[0], buttonsize[1]), bluebutton, "Restart Single Player");
            buttons[2] = new Button(new Rectangle((w - buttonsize[0]) / 2, 450, buttonsize[0], buttonsize[1]), yellowbutton, "Instructions");
        }

        public int update(GameTime gt)
        {
            MouseState m = Mouse.GetState();
            Point mousepos = new Point(m.X, m.Y);
            GamePadState newPad = GamePad.GetState(PlayerIndex.One);
            if(newPad.Buttons.Start == ButtonState.Pressed && oldPad.Buttons.Start == ButtonState.Released)
            {
                return 1;
            }
            if(newPad.Buttons.LeftShoulder == ButtonState.Pressed && oldPad.Buttons.LeftShoulder == ButtonState.Released)
            {
                return 2;
            }
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].drect.Contains(mousepos))
                {
                    buttons[i].setActive(true);
                    if (m.LeftButton==ButtonState.Pressed) {
                        if(i == 0 || i == 1)
                        {
                            return 1;
                        }
                        else
                        {
                            return 2;
                        }
                    }
                }
                else {
                    buttons[i].setActive(false);
                }
            }
            return 0;
        }


        public void draw(GameTime gt, SpriteBatch sb) {
            for (int i = 0; i < buttons.Length; i++)
            {
                Rectangle drect = buttons[i].drect;
                sb.Draw(spritesheet, drect, buttons[i].srect, Color.White);
                float startstringX = drect.X + drect.Width / 2.0f - buttons[i].insideText.Length*4.5f;
                sb.DrawString(font, buttons[i].insideText, new Vector2(startstringX,buttons[i].drect.Y+20), Color.White);
            }
            sb.DrawString(bigfont, "Spaceship Battle", new Vector2(120,60),Color.Yellow);


        }

    }
}
