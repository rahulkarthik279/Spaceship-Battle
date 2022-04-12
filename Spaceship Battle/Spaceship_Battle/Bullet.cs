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
    class Bullet : GravityBody
    {
        public static Texture2D text;
        public static List<Bullet> list;

        public bool isDestroyed;
        public bool isPlayers;
        public Level Level
        {
            get { return level; }
        }
        Level level;

        public Bullet(Level l, int startX, int startY, int endX, int endY, int time) : 
            base(0, (double)(endX - startX) / time, (double)(endY - startY) / time, new Rectangle(startX, startY, 20, 20))
        {
            level = l;
            isDestroyed = false;
            maxTime = time;
        }

        public Bullet(Level l, int startX, int startY, float angle, float speed) : 
            base(0, 0,0, new Rectangle(startX-20, startY-20, 20, 20))
        {
            level = l;
            velocity.X = Level.player.velocity.X + (float)Math.Cos(angle) * speed;
            velocity.Y = Level.player.velocity.Y + (float)Math.Sin(angle) * speed;
        }


        public bool intersectsBullet(Bullet b)
        {
            if (rect.Intersects(b.rect) && !b.isDestroyed)
            {
                isDestroyed = true;
                b.isDestroyed = true;
                return true;
            }
            return false;
        }
        public bool intersectsEnemy(Enemy e)
        {
            if (rect.Intersects(e.rect) && e.health > 0)
            {
                if (!isDestroyed)
                {
                    e.health = 0;
                }
                isDestroyed = true;
                Console.WriteLine(e.health);
                return true;
            }
            return false;
        }
        public bool intersectsPlayer(Player p)
        {

            if (rect.Intersects(p.rect) && p.health > 0)
            {
                if (!isDestroyed&&p.isInvincible==false)
                {
                    p.health -= 20;
                }
                isDestroyed = true;
                
                return true;
            }
            return false;
        }


        public static void updateAll()
        {
            for (int i = 0; i < list.Count; i++) {
                list[i].update();
            }
        }

        public new void update()
        {
            if(!isDestroyed)
            {
                base.update();
            }


        }

        public static void drawAll(SpriteBatch sb) {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].draw(sb);
            }
        }

        public void draw(SpriteBatch sb)
        {
            sb.Draw(text, rect, Color.White);        
        }
    }
}
