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
    class Enemy:GravityBody
    {
        public static Texture2D text;
        public int health;
        Random rand;
        public Gun gun;
        int seconds;
        int timeForFiring;
        public Level Level
        {
            get { return level; }
        }
        Level level;
        public Enemy(Level l, Rectangle r): base(1,0,0,r)
        {
            level = l;
            rand = new Random();
            gun = new Gun(level, rand.Next(10, 16), new Rectangle(r.X + 10, r.Y + r.Height, 30, 10));
            rect = r;
            health = rand.Next(1, 4) * 100;
            velocity.X = rand.Next(1, 4);
            velocity.Y = rand.Next(1, 3);
            for(int i = 0; i < gun.bullets.Count; i++)
            {
                gun.bullets[i].isPlayers = false;
                //gun.bullets[i].LoadContent();
            }
            timeForFiring = rand.Next(120, 480);
        }
        
        public new void update()
        {
            if(health > 0 && (rect.X + rect.Width > 0) && Level.player.health > 0)
            {
                pos.X -= velocity.X;
                if(rand.NextDouble() < 0.50)
                {
                    if(rect.Y > 0)
                    {
                        pos.Y += velocity.Y;
                    }
                    
                }
                else
                {
                    if(rect.Y + rect.Height < 400)
                    {
                        pos.Y -= velocity.Y;
                    }
                }


                
                if (seconds == timeForFiring)
                {
                    gun.fire(false);
                    timeForFiring = rand.Next(seconds + 120, seconds + 480);
                }
                seconds++;
            }
            gun.update(rect);

            rect.X = (int)pos.X + level.getoffset(0);
            rect.Y = (int)pos.Y + level.getoffset(1);
        }
        public void draw(SpriteBatch sb, GameTime gt)
        {
            if(rect != null && health > 0 && Level.player.health > 0)
            {
                sb.Draw(text, rect, Color.White);
            }
        }
    }
}
