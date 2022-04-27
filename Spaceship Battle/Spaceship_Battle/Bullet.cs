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
        public static int damage = 20;

        public bool isDestroyed;
        public bool isPlayers;
        public Level Level
        {
            get { return level; }
        }
        Level level;

        public Bullet(Level l, int startX, int startY, int endX, int endY, int time) : 
            base(1, (double)(endX - startX) / time, (double)(endY - startY) / time, new Rectangle(startX, startY, 20, 20))
        {
            level = l;
            isDestroyed = false;
            isPlayers = false;
        }

        public Bullet(Level l, int startX, int startY, float angle, float speed, bool fromPlayer) : 
            base(1, 0,0, new Rectangle(startX-20, startY-20, 20, 20))
        {
            level = l;
            isPlayers = fromPlayer;
            velocity.X = Level.player.velocity.X + (float)Math.Cos(angle) * speed;
            velocity.Y = Level.player.velocity.Y + (float)Math.Sin(angle) * speed;
        }

        public static void loadcontent(ContentManager content) {
            Bullet.text = content.Load<Texture2D>("redRectForBorg");
            list = new List<Bullet>();
        }

        public static void updateAll()
        {
            //first loop for intersections
            for (int i = 0; i < list.Count; i++)
            {
                list[i].update(i);
            }

            //second loop to remove all destroyed bullets
            for (int i = list.Count - 1; i >= 0; i--) {
                if (list[i].isDestroyed) {
                    list.RemoveAt(i);
                }
            }
        }

        public void update(int index)
        {
            if (!isDestroyed)
            {
                base.update();
                for (int j = index + 1; j < list.Count; j++)
                {
                    //intersect with other bullets
                    if (!list[j].isDestroyed && list[j].isPlayers != isPlayers)
                    {
                        if (rect.Intersects(list[j].rect))
                        {
                            isDestroyed = true;
                            list[j].isDestroyed = true;
                        }
                    }
                }
                
                if (isPlayers)
                {
                    //intersect with enemies
                    for (int j = 0; j < Enemy.list.Count; j++)
                    {
                        if (intersectsEnemy(Enemy.list[j])){
                            isDestroyed = true;
                        }
                    }
                    //intersect with turrets
                    for (int j = 0; j < Turret.list.Count; j++)
                    {
                        if (intersectsTurret(Turret.list[j]))
                        {
                            isDestroyed = true;
                        }
                    }
                }
                else {
                    //intersect with players
                    if (intersectsPlayer(Level.player)) {
                        isDestroyed = true;
                    }
                }
            }

        }

        public static void drawAll(SpriteBatch sb)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].draw(sb);
            }
        }

        public void draw(SpriteBatch sb)
        {
            sb.Draw(text, rect, Color.White);
        }

        //public bool intersectsBullet(Bullet b)
        //{
        //    if (rect.Intersects(b.rect) && !b.isDestroyed)
        //    {
        //        isDestroyed = true;
        //        b.isDestroyed = true;
        //        return true;
        //    }
        //    return false;
        //}

        public bool intersectsTurret(Turret t)
        {
            if (rect.Intersects(t.rect) && t.health > 0)
            {
                if (!isDestroyed)
                {
                    t.health -= damage;
                }
                isDestroyed = true;
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
                    p.health -= damage;
                }
                isDestroyed = true;
                
                return true;
            }
            return false;
        }
    }
}
