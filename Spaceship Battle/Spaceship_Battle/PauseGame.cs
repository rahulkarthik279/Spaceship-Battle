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
<<<<<<< Updated upstream
    class PauseGame: Button
=======
    class PauseGame
>>>>>>> Stashed changes
    {
        Texture2D spritesheet;
        Texture2D spritesheet2;
        ContentManager content;
        MouseState oldM = Mouse.GetState();
<<<<<<< Updated upstream
        public PauseGame(IServiceProvider service): base(new Rectangle(0,0,30,30), new Rectangle[] { new Rectangle(500, 0, 500, 780), new Rectangle(0, 0, 500, 780) }, "hello")
=======
        Button pauseOrPlay;
        Button save;
        Button saveAndExit;
        bool isPaused = false;
        SpriteFont font;
        public bool exit = false;
        public PauseGame(IServiceProvider service)
>>>>>>> Stashed changes
        {
            content = new ContentManager(service, "Content");
            font = content.Load<SpriteFont>("menufont");
            spritesheet = content.Load<Texture2D>("playpause");
            spritesheet2 = content.Load<Texture2D>("Sci-Fi-buttons-cover");
            pauseOrPlay = new Button(new Rectangle(0, 0, 30, 30), new Rectangle[] { new Rectangle(500, 0, 500, 780), new Rectangle(0, 0, 500, 780) }, "");
            save = new Button(new Rectangle(325, 100, 150, 50), new Rectangle[] { new Rectangle(440, 148, 128, 55), new Rectangle(440, 80, 128, 55) }, "Save");
            saveAndExit = new Button(new Rectangle(325, 200, 150, 50), new Rectangle[] { new Rectangle(125, 145, 145, 55), new Rectangle(125, 75, 145, 55) }, "Save and Exit");
        }
        public void draw(SpriteBatch sb, GameTime gt)
        {
            sb.Draw(spritesheet, pauseOrPlay.drect, pauseOrPlay.srect, Color.White);
            if (isPaused)
            {
                sb.Draw(spritesheet2, save.drect, save.srect, Color.White);
                sb.Draw(spritesheet2, saveAndExit.drect, saveAndExit.srect, Color.White);
                float startstringX = save.drect.X + save.drect.Width / 2.0f - save.insideText.Length * 4.5f;
                sb.DrawString(font, save.insideText, new Vector2(startstringX, save.drect.Y + 20), Color.White);
                float startstringX2 = saveAndExit.drect.X + saveAndExit.drect.Width / 2.0f - saveAndExit.insideText.Length * 4.5f;
                sb.DrawString(font, saveAndExit.insideText, new Vector2(startstringX2, saveAndExit.drect.Y + 20), Color.White);
            }
        }
<<<<<<< Updated upstream
=======

>>>>>>> Stashed changes
        public void setActive(bool input)
        {
            if (input)
            {
<<<<<<< Updated upstream
                if(srect == sourcearray[0])
=======
                if (pauseOrPlay.srect == pauseOrPlay.sourcearray[0])
>>>>>>> Stashed changes
                {
                    pauseOrPlay.srect = pauseOrPlay.sourcearray[1];
                    isPaused = true;
                }
                else
                {
                    pauseOrPlay.srect = pauseOrPlay.sourcearray[0];
                    isPaused = false;
                }
            }
        }
        public int update(GameTime gt)
        {
            MouseState m = Mouse.GetState();
            Point mousepos = new Point(m.X, m.Y);
            if (isPaused)
            {
                if (save.drect.Contains(mousepos))
                {
                    save.setActive(true);
                    if (m.LeftButton == ButtonState.Pressed && oldM.LeftButton == ButtonState.Released)
                    {
                        oldM = m;
                        setActive(true);
                        isPaused = false;
                        return 1;
                    }
                }
                else
                {
                    save.setActive(false);
                }
                if (saveAndExit.drect.Contains(mousepos))
                {
                    saveAndExit.setActive(true);
                    if(m.LeftButton == ButtonState.Pressed)
                    {
                        exit = true;
                    }
                }
                else
                {
                    saveAndExit.setActive(false);
                }
            }
            if (pauseOrPlay.drect.Contains(mousepos) && (m.LeftButton == ButtonState.Pressed & oldM.LeftButton == ButtonState.Released))
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
