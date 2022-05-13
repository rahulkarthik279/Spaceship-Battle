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
        public static List<Enemy> list;
        public static int numEnemies;

        public int health;
        static Random rand;
        public Gun gun;
        int seconds;
        int timeForFiring;

        public Level Level
        {
            get { return level; }
        }
        static Level level;

        public Enemy(Rectangle r): base(1,0,0,r)
        {
            gun = new Gun(level, rand.Next(10, 16), new Rectangle(r.X + 10, r.Y + r.Height, 30, 10));
            rect = r;
            health = rand.Next(1, 4) * 100;
            velocity.X = -1* rand.Next(1, 4);
            velocity.Y = rand.Next(1, 3);
            timeForFiring = rand.Next(120, 480);
        }

        public static void loadcontent(ContentManager content, int numenemies, Level l) {
            list = new List<Enemy>();
            text = content.Load<Texture2D>("airplane (2)");
            numEnemies = numenemies;
            level = l;
            rand = new Random();
        }

        public static void updateAll()
        {
            for (int i = 0; i <list.Count; i++)
            {
               list[i].update();
            }
            if (level.timer % 20 == 0 && list.Count < Enemy.numEnemies)
            {
                list.Add(new Enemy(new Rectangle(rand.Next((int)Level.player.pos.X, level.world.Width), rand.Next((int)Level.player.pos.Y - 500, level.world.Height), 60, 30)));
            }
            for (int i = list.Count - 1; i >= 0; i--) {
                if (list[i].health <= 0)
                {
                    list.RemoveAt(i);
                }
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

                //player 
                if (rect.Intersects(Level.player.rect)) {
                    health = 0;
                    Level.player.health -= 30;
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

        public static void drawAll(SpriteBatch sb) {
            for (int i = 0; i <list.Count; i++)
            {
               list[i].draw(sb);
            }
        }
    }
}
