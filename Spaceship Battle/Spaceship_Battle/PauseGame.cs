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
    class PauseGame : Button
    {
        Texture2D spritesheet;
        ContentManager content;
        MouseState oldM = Mouse.GetState();

        public PauseGame(IServiceProvider service) : base(new Rectangle(0, 0, 30, 30), new Rectangle[] { new Rectangle(500, 0, 500, 780), new Rectangle(0, 0, 500, 780) }, "hello")
        {
            content = new ContentManager(service, "Content");
            spritesheet = content.Load<Texture2D>("playpause");
        }

        public void draw(SpriteBatch sb, GameTime gt)
        {
            sb.Draw(spritesheet, base.drect, base.srect, Color.White);
        }

        public new void setActive(bool input)
        {
            if (input)
            {
                if (srect == sourcearray[0])
                {
                    srect = sourcearray[1];
                }
                else
                {
                    srect = sourcearray[0];
                }
            }
        }

        public int update(GameTime gt)
        {
            MouseState m = Mouse.GetState();
            Point mousepos = new Point(m.X, m.Y);
            if (drect.Contains(mousepos) && (m.LeftButton == ButtonState.Pressed & oldM.LeftButton == ButtonState.Released))
            {
                setActive(true);
                oldM = m;
                return 1;
            }
            oldM = m;
            return 0;

        }
    }
}