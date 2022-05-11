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
    class Missile:GravityBody
    {
        public const int continuousdamage = 4;

        public static List<Missile> list;
        public static Texture2D rocketpic, explosionpic;
        public float angle; //rotation in radians with respect to +x axis
        public Boolean exploded;
        int explosionspeed;
        public static Rectangle srect = new Rectangle(0, 0, 200, 200);
        public Vector2 origin;
        static Random rn = new Random();
        public int xT;
        public int yT;


        //public Missile(int xtarget, int ytarget, int screenwidth, Rectangle r) : base(1, 0, 0, r)
        //{
        //    stuff for exploding
        //    exploded = false;
        //    explosionspeed = 1;
        //    origin = new Vector2(srect.Width / 2, srect.Height / 2);
        //    xT = xtarget;
        //    yT = ytarget;
        //    Console.WriteLine(xtarget + ", " + ytarget);
        //}

        public Missile(int startX, int startY, int endX, int endY, int time) : 
            base(1, (double)(endX - startX) / time, (double)(endY - startY) / time, new Rectangle(startX, startY, 20, 20))
        {
            exploded = false;
            explosionspeed = 1;
            origin = new Vector2(srect.Width / 2, srect.Height / 2);
            xT = endX;
            yT = endY;
            
            angle = (float)(Math.Atan2(velocity.Y, velocity.X));
            exploded = false;
            explosionspeed = 1;
        }

        public Missile(int startX, int startY, float angle, float speed) :
            base(1, 0, 0, new Rectangle(startX - 20, startY - 20, 20, 20))
        {
            velocity.X = Level.player.velocity.X + (float)Math.Cos(angle) * speed;
            velocity.Y = Level.player.velocity.Y + (float)Math.Sin(angle) * speed;
        }

        public static void loadcontent(ContentManager content) {
            list = new List<Missile>();
            rocketpic = content.Load<Texture2D>("missile");
            explosionpic = content.Load<Texture2D>("redexplosion");
        }

        //update method returns whether the main method should delete the object
        public new Boolean update()
        {
            base.update();
            if (exploded)
            {
                if (rect.Width > 50 && explosionspeed > 0)
                {
                    explosionspeed *= -1;
                }
                rect.Width += explosionspeed;
                rect.Height = rect.Width;
                if (rect.Width <= 0)
                    return true;

                //intersect with enemies
                for (int j = 0; j < Enemy.list.Count; j++)
                {
                    if (circleintersects(Enemy.list[j]))
                    {
                        Enemy.list[j].health -= continuousdamage;
                    }
                }
                //intersect with turrets
                for (int j = 0; j < Turret.list.Count; j++)
                {
                    if (circleintersects(Turret.list[j]))
                    {
                        Turret.list[j].health -= continuousdamage;
                    }
                }
                //intersect bullets
                for (int j = 0; j < Bullet.list.Count; j++)
                {
                    if (circleintersects(Bullet.list[j]))
                    {
                        Bullet.list[j].isDestroyed = true;
                    }
                }

                //intersect Player
                if (rect.Intersects(Level.player.rect) && Level.player.health > 0)
                {
                    if (Level.player.isInvincible == false)
                    {
                        Level.player.health -= continuousdamage;
                    }
                }


            }
            else
            {
                if (Math.Abs(pos.Y - yT) < 5)
                {
                    startexploding();
                }

                //intersect with enemies
                for (int j = 0; j < Enemy.list.Count; j++)
                {
                    if (intersectsEnemy(Enemy.list[j]))
                    {
                        Enemy.list[j].health = 0;
                        startexploding();
                    }
                }
                //intersect with turrets
                for (int j = 0; j < Turret.list.Count; j++)
                {
                    if (intersectsTurret(Turret.list[j]))
                    {
                        startexploding();
                    }
                }
                //intersect bullets
                for (int j = 0; j < Bullet.list.Count; j++)
                {
                    if (intersectsBullet(Bullet.list[j]))
                    {
                        startexploding();
                    }
                }

            }
            return false;
        }

        public static void updateAll() {
            for (int i = list.Count - 1; i >= 0; i--) {
                if (list[i].update()) {
                    list.RemoveAt(i);
                }
            }
        }

        public bool intersectsTurret(Turret t)
        {
            if (rect.Intersects(t.rect) && t.health > 0)
            {
                return true;
            }
            return false;
        }

        public bool intersectsEnemy(Enemy e)
        {
            if (rect.Intersects(e.rect) && e.health > 0)
            {
                return true;
            }
            return false;
        }

        public bool intersectsBullet(Bullet b) {
            if (rect.Intersects(b.rect))
            {
                b.isDestroyed = true;
                return true;
            }
            return false;
        }

        //check if object intersects a rectangle
        public Boolean intersects(Rectangle input)
        {
            Boolean output = false;
            int radius = rect.Width / 2;
            //double distance = Math.Sqrt(Math.Pow(input.X - origin.X, 2) + Math.Pow(input.Y - origin.Y, 2));
            output = input.Contains(rect.X + radius, rect.Y) || input.Contains(rect.X - radius, rect.Y) || input.Contains(rect.X, rect.Y + radius) || input.Contains(rect.X, rect.Y - radius);
            if (output) { startexploding(); }
            return output;
        }

        public Boolean[] intersects(Rectangle[] input)
        {
            Boolean[] output = new Boolean[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                output[i] = intersects(input[i]);
            }
            return output;
        }

        public Boolean circleintersects(GravityBody input)
        {
            Boolean output = false;
            int radius = rect.Width / 2;
            double distance1 = Math.Sqrt(Math.Pow(input.pos.X - pos.X, 2) + Math.Pow(input.pos.Y - pos.Y, 2));
            double distance2 = Math.Sqrt(Math.Pow(input.pos.X + input.rect.Width - pos.X, 2) + Math.Pow(input.pos.Y + input.rect.Height - pos.Y, 2));
            output = distance1 <= radius || distance2 <= radius;
            if (output) { startexploding(); }
            return output;
        }

        public void startexploding()
        {
            if (!exploded)
            {
                exploded = true;
                rect.Width = 10;
                rect.Height = rect.Width;
                velocity.X = 0;
                velocity.Y = 0;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            if (exploded)
            {
                sb.Draw(explosionpic, rect, srect, Color.White, angle, origin, SpriteEffects.None, 0);
            }
            else {
                sb.Draw(rocketpic, rect, srect, Color.White, angle, origin, SpriteEffects.None, 0);
            }
        }

        public static void drawAll(SpriteBatch sb) {
            for (int i = 0; i < list.Count; i++) {
                list[i].Draw(sb);
            }
        }
    }
}