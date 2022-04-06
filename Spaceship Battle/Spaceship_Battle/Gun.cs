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
    class Gun
    {
        public int capacity;
        public List<Bullet> bullets;
        public Rectangle rect;
        public static Texture2D text;
        public int numActive = 0;
        Random rand = new Random();
        public Level Level
        {
            get { return level; }
        }
        Level level;
        public Gun(Level l, int c)
        {
            capacity = c;
            bullets = new List<Bullet>();
        }
        public Gun(Level l, int c, Rectangle r)
        {
            level = l;
            capacity = 9999;//c;
            rect = r;
            bullets = new List<Bullet>();
            //for (int i = 0; i < capacity; i++)
            //{
            //    bullets.Add(new Bullet(level, rect.X + rect.Width, rect.Y + rect.Height / 2, 400, 400, 120));
            //}
        }
        
        public void fire(bool isPlayers)
        {
            if (numActive < capacity)
            {
                if (!isPlayers)
                {
                    bullets.Add(new Bullet(level, rect.X + rect.Width/2 - level.getoffset(0), rect.Y - level.getoffset(1), Level.player.rect.X, Level.player.rect.Y, rand.Next(60, 180)));
                }
                else
                {
                    bullets.Add(new Bullet(level, rect.X + rect.Width/2 - level.getoffset(0), rect.Y - level.getoffset(1), Level.player.rotation, rand.Next(8, 12)));
                }
                bullets[numActive].isFired = true;
                numActive++;
            }
            
        }
        public void update(Rectangle playerrect)
        {
            for(int i = 0; i < bullets.Count; i++)
            {
                bullets[i].update();
            }
            rect.X = playerrect.X + 15;
            rect.Y = playerrect.Y + playerrect.Height;
        }

        public void draw(SpriteBatch sb, GameTime gt, bool isPlayers)
        {
            if (isPlayers)
            {
                sb.Draw(text, rect, null, Color.White, Level.player.rotation, new Vector2(text.Width / 2, text.Height / 2), SpriteEffects.None, 0);
            }
            else
            {
                sb.Draw(text, rect, Color.White);
            }
            for(int i = 0; i < bullets.Count; i++)
            {
                if(bullets[i].isFired && !bullets[i].isDestroyed)
                {
                    bullets[i].draw(sb,gt);
                }
            }
        }
    }
}
