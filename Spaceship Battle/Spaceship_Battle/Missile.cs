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
    class PlayerMissile:GravityBody
    {
        public static List<PlayerMissile> list;
        
        public float angle; //rotation in radians with respect to +x axis
        public static Texture2D pic;
        public Boolean exploded;
        int explosionspeed;
        public static Rectangle srect = new Rectangle(0, 0, 200, 200);
        public Vector2 origin;
        static Random rn = new Random();
        public static Texture2D rocketpic, explosionpic;
        public int xT;
        public int yT;

        public PlayerMissile(int xtarget, int ytarget, int screenwidth, Rectangle r): base(1,0,0,r)
        {
            //stuff for exploding
            exploded = false;
            explosionspeed = 1;
            origin = new Vector2(srect.Width / 2, srect.Height / 2);
            xT = xtarget;
            yT = ytarget;
            Console.WriteLine(xtarget + ", " + ytarget);

            //calculate velocity + setup angle
            //random y velocity, then calculate x velocity to reach target
            
            //setup stuff for drawing
            pic = rocketpic;
        }

        public PlayerMissile(Level l, int startX, int startY, int endX, int endY, int time) : 
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
            }
            else
            {
                if (Math.Abs(pos.Y - yT) < 5)
                {
                    startexploding();
                }
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

        public Boolean circleintersects(Rectangle input)
        {
            Boolean output = false;
            int radius = rect.Width / 2;
            double distance1 = Math.Sqrt(Math.Pow(input.X - x, 2) + Math.Pow(input.Y - y, 2));
            double distance2 = Math.Sqrt(Math.Pow(input.X + input.Width - x, 2) + Math.Pow(input.Y + input.Height - y, 2));
            output = distance1 <= radius || distance2 <= radius;
            if (output) { startexploding(); }
            return output;
        }

        public void startexploding()
        {
            if (!exploded)
            {
                pic = explosionpic;
                exploded = true;
                rect.Width = 10;
                rect.Height = rect.Width;
                velocity.X = 0;
                velocity.Y = 0;
            }
        }

        public void Draw(SpriteBatch sb, GameTime g)
        {
            sb.Draw(pic, rect, srect, Color.White, angle, origin, SpriteEffects.None, 0);
        }
    }
}