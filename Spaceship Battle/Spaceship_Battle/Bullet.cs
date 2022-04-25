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
        public bool isDestroyed;
        public bool isFired;
        public static Texture2D text;
        public bool isPlayers;
        int maxTime, time;
        Color color;
        public Level Level
        {
            get { return level; }
        }
        Level level;
        public Bullet(Level l, int startX, int startY, int endX, int endY, int time, Color c) : 
            base(0, (double)(endX - startX) / time, (double)(endY - startY) / time, new Rectangle(startX, startY, 20, 20))
        {
            level = l;
            isDestroyed = false;
            isFired = false;
            maxTime = time;
            color = c;
        }
        public Bullet(Level l, int startX, int startY, float angle, float speed, Color c, bool isPlayer) : 
            base(0, 0,0, new Rectangle(startX-20, startY-20, 20, 20))
        {
            level = l;
            
            velocity.X = (float)Math.Cos(angle) * speed;
            velocity.Y = (float)Math.Sin(angle) * speed;
            if (isPlayer)
            {
                velocity.X += Level.player.velocity.X;
                velocity.Y += Level.player.velocity.Y;
            }
            color = c;
        }
        public bool intersectsBullet(Bullet b)
        {
            if (rect.Intersects(b.rect) && !b.isDestroyed && !isDestroyed)
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
        public bool intersectsTurret(Turret t)
        {
            if(rect.Intersects(t.rect) && Turret.health > 0){
                if (!isDestroyed)
                {
                    Turret.health -= 30;
                }
                isDestroyed = true;
                return true;
            }
            return false;
        }
        public new void update()
        {

            if(!isDestroyed && isFired)
            {
                base.update();
                time++;
            }
            //if(time == maxTime)
            //{
            //    isDestroyed = true;
            //}
        }

        public void draw(SpriteBatch sb, GameTime gt)
        {
            sb.Draw(text, rect, color);        
        }
    }
}
