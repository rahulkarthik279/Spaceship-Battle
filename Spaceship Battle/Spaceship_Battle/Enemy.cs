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
        public static List<Enemy> enemies;
        public static int numEnemies;

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
            velocity.X = -1* rand.Next(1, 4);
            velocity.Y = rand.Next(1, 3);
            timeForFiring = rand.Next(120, 480);
        }

        public static void loadcontent(ContentManager content, int numenemies) {
            enemies = new List<Enemy>();
            text = content.Load<Texture2D>("airplane");
            numEnemies = numenemies;
        }

        public static void updateAll()
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].update();
            }
        }
        public new void update()
        {
            if(health > 0 && Level.player.health > 0)
            {
                //positioning
                base.update();

                //constrain location to world
                if (pos.X < 1)
                {
                    velocity.X = 1f;
                    pos.X = 0;
                }else if (pos.X > level.world.Width)
                {
                    velocity.X = -1f;
                    pos.X = level.world.Width;
                }
                if (pos.Y < 1)
                {
                    velocity.Y = 1f;
                    pos.Y = 0;
                }
                else if (pos.Y > level.world.Height)
                {
                    velocity.Y = -1f;
                    pos.Y = level.world.Height;
                }



                if (seconds == timeForFiring)
                {
                    gun.fire(false);
                    timeForFiring = rand.Next(seconds + 120, seconds + 480);
                }
                seconds++;
            }
            gun.update(rect);
        }

        public void draw(SpriteBatch sb)
        {
            if(rect != null && health > 0 && Level.player.health > 0)
            {
                sb.Draw(text, rect, Color.White);
            }
        }

        public void drawAll(SpriteBatch sb) {
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].draw(sb);
            }
        }
    }
}
