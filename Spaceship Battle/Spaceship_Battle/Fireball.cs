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
    class Fireball : GravityBody
    {
        public bool isDestroyed;
        public static Texture2D text;
        public static List<Fireball> list;
        //public bool isPlayers;
        //int maxTime, time;
        int numTimesHit;
        public static bool isActivated;

        public Level Level
        {
            get { return level; }
        }
        Level level;
        //public Fireball(Level l, int startX, int startY, int endX, int endY, int time) :
        //    base(0, (double)(endX - startX) / time, (double)(endY - startY) / time, new Rectangle(startX, startY, 20, 20))
        //{
        //    level = l;
        //    isDestroyed = false;
        //    isFired = false;
        //    maxTime = time;
        //}
        public Fireball(Level l, int startX, int startY, float angle, float speed) :
            base(0, 0, 0, new Rectangle(startX - 20, startY - 20, 20, 20))
        {
            level = l;
            velocity.X = Level.player.velocity.X + (float)Math.Cos(angle) * speed;
            velocity.Y = Level.player.velocity.Y + (float)Math.Sin(angle) * speed;
        }
        public bool intersectsBullet(Bullet b)
        {
            if (rect.Intersects(b.rect) && !b.isDestroyed)
            {
                b.isDestroyed = true;
                numTimesHit++;
                if (numTimesHit == 2)
                {
                    isDestroyed = true;
                }
                return true;
            }
            return false;
        }
        public bool intersectsEnemy(Enemy e)
        {
            if (rect.Intersects(e.rect) && e.health > 0)
            {
                e.health = 0;
                isDestroyed = true;
                Console.WriteLine(e.health);
                return true;
            }
            return false;
        }
        //public bool intersectsPlayer(Player p)
        //{
        //    if (rect.Intersects(p.rect) && p.health > 0 && !Level.player.isInvincible)
        //    {
        //        if (!isDestroyed)
        //        {
        //            p.health -= 20;
        //        }
        //        isDestroyed = true;

        //        return true;
        //    }
        //    return false;
        //}

        public static void loadcontent(ContentManager content) {
            Fireball.text = content.Load<Texture2D>("save");
            list = new List<Fireball>();
            isActivated = false;
        }

        public new void update()
        {
            if (!isDestroyed)
            {
                base.update();
                for (int j = 0; j < Enemy.list.Count; j++)
                {
                    intersectsEnemy(Enemy.list[j]);
                }
            }
        }

        public static void updateAll() {
            //first loop for intersections
            for (int i = 0; i < list.Count; i++)
            {
                list[i].update();
            }

            //second loop to remove all destroyed bullets
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i].isDestroyed)
                {
                    list.RemoveAt(i);
                }
            }
        }
        public void draw(SpriteBatch sb)
        {
            if (!isDestroyed)
            {
                sb.Draw(text, rect, Color.Red);
            }
        }

        public void drawAll(SpriteBatch sb) {
            for (int i = 0; i < list.Count; i++) {
                list[i].draw(sb);
            }
        }
    }
}