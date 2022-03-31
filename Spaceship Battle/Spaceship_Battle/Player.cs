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
    class Player : GravityBody
    {
        public Texture2D text;
        public int health;
        public bool isAlive;
        public Gun gun;
        KeyboardState oldKb = Keyboard.GetState();
        MouseState oldMouse = Mouse.GetState();
        public float rotation;
        public Level Level
        {
            get { return level; }
        }
        Level level;
        public Player(Level l, Rectangle r): base(10,0,0, r)
        {
            level = l;
            rect = r;
            health = 10000000;
            isAlive = true;
            gun = new Gun(level, 15, new Rectangle(r.X + 15, r.Y + r.Height, 30, 10));
            for (int i = 0; i < gun.bullets.Count; i++)
            {
                gun.bullets[i].isPlayers = true;
            }
        }
        public void update(GameTime gt)
        {

            if (health > 0)
            {
                getInput(gt);
                gun.update(rect);
            }

           //set camera position don't touch this jank code
           //in X direction
            if (velocity.X > 0)
            {
                if (rect.X > level.width-100)
                {
                    if (-1*level.getoffset(0) > level.world.Width - level.width-1)
                    {
                        if (pos.X > level.world.Width)
                        {
                            velocity.X = 0;
                            pos.X = level.world.Width;
                        }
                    }
                    else
                    {
                        Level.world.X = (int)pos.X * -1 + level.width - 100;
                    }
                }
            }
            else
            {
                if (rect.X < 100)
                {
                    if (level.getoffset(0) >-1)
                    {
                        if (pos.X < 1)
                        {
                            velocity.X = 0;
                            pos.X = 0;
                        }
                    }
                    else
                    {
                        Level.world.X = (int)pos.X * -1 + 100;
                    }
                }
            }
            //in repeat in y direction
            if (velocity.Y > 0)
            {
                if (rect.Y > level.height-100)
                {
                    if (-1 * level.getoffset(1) > level.world.Height - level.height-1)
                    {
                        if (pos.Y > level.world.Height)
                        {
                            velocity.Y = 0;
                            pos.Y = level.world.Height;
                        }
                    }
                    else
                    {
                        Level.world.Y = (int)pos.Y * -1 + level.height - 100;
                    }
                }
            }
            else
            {
                if (rect.Y < 100)
                {
                    if (level.getoffset(1) > -1)
                    {
                        if (pos.Y < 1)
                        {
                            velocity.Y = 0;
                            pos.Y = 0;
                        }
                    }
                    else
                    {
                        Level.world.Y = (int)pos.Y * -1 + 100;
                    }
                }
            }
            // end of camera stuff this crap is cancer

            base.update();
        }
        private void getInput(GameTime gt)
        {
            GamePadState gamePad = GamePad.GetState(PlayerIndex.One);
            KeyboardState kb = Keyboard.GetState();
            MouseState newMouse = Mouse.GetState();
            //distance between mouse and gun
            double diffX, diffY;
            diffX = newMouse.X - (gun.rect.X + gun.rect.Width/2);
            diffY = newMouse.Y - (gun.rect.Y + gun.rect.Height/2);
            //Console.WriteLine(newMouse.X + ", " + newMouse.Y);
            rotation = (float)Math.Atan2(diffY,diffX);
            if(kb.IsKeyDown(Keys.Right))
            {
                if (base.velocity.X < 5)
                {
                    base.velocity.X += .1f;
                }
                
            }
            if (kb.IsKeyDown(Keys.Left))
            {
                if (velocity.X > -5) {
                    velocity.X -= .1f;
                }
            }
            if (kb.IsKeyDown(Keys.Up) || gamePad.ThumbSticks.Left.Y > 0)
            {
                if(velocity.Y>-5)
                {
                    velocity.Y -= .1f;
                }
                
            }
            if (kb.IsKeyDown(Keys.Down) || gamePad.ThumbSticks.Left.Y < 0)
            {
                if (velocity.Y < 5)
                {
                    velocity.Y += .1f;
                }
            }
            if((kb.IsKeyDown(Keys.Space) && !oldKb.IsKeyDown(Keys.Space))|| gamePad.IsButtonDown(Buttons.A) || (newMouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton != ButtonState.Pressed))
            {
                gun.fire(true);
            }
            oldMouse = newMouse;
            oldKb = kb;
        }

        public void draw(SpriteBatch sb, GameTime gt)
        {
            sb.Draw(text, rect, null, Color.White, (float)Math.PI/2+rotation, new Vector2(text.Bounds.Width/2, text.Bounds.Height/2), SpriteEffects.None, 0);
        }
    }
}
