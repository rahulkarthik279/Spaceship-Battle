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
    abstract class GravityBody
    {
        public static double gravitationalconstant = .01;
        public static int offsetX=0, offsetY=0;
        public static int w, h;
        public int mass;
        public Vector2 pos, velocity;
        public Rectangle rect;

        //Rectangle input in constructor gets the starting position of the object. The position is absolute relative to the level, not the position on screen. 
        public GravityBody(int mass, Vector2 v, Rectangle r) {
            this.mass = mass;
            this.pos = new Vector2(r.X, r.Y);
            this.velocity = v;
            this.rect = r;
            rect.X = (int)pos.X + offsetX;
            rect.Y = (int)pos.Y + offsetY;
        }

        public GravityBody(int mass, double vx, double vy, Rectangle r)
        {
            this.mass = mass;
            pos = new Vector2(r.X, r.Y);
            velocity = new Vector2((float)vx, (float)vy);
            this.rect = r;
            rect.X = (int)pos.X + offsetX;
            rect.Y = (int)pos.Y + offsetY;
        } 

        public GravityBody(int mass, float vx, float vy, Rectangle r) {
            this.mass = mass;
            pos = new Vector2(r.X, r.Y);
            velocity = new Vector2(vx, vy);
            this.rect = r;
            rect.X = (int)pos.X + offsetX;
            rect.Y = (int)pos.Y + offsetY;
        }

        public GravityBody(int mass, int vx, int vy, Rectangle r)
        {
            this.mass = mass;
            pos = new Vector2(r.X, r.Y);
            velocity = new Vector2(vx, vy);
            this.rect = r;
            rect.X = (int)pos.X + offsetX;
            rect.Y = (int)pos.Y + offsetY;
        }


        //updates the position and the rectangle to draw it
        public void update() {
            pos.X += velocity.X;
            pos.Y += velocity.Y;
            rect.X = (int)pos.X + offsetX;
            rect.Y = (int)pos.Y + offsetY;
        }

        //do physics calculation for a single object
        public void physicsstuff(GravityBody other)
        {
            //ignore 0 masses
            if (mass > 0 && other.mass > 0)
            {
                double dx = other.pos.X - pos.X - rect.Width / 2;
                double dy = other.pos.Y - pos.Y - rect.Height / 2;
                double distancesquared = Math.Pow(dx, 2) + Math.Pow(dy, 2);
                if (distancesquared < 1)
                {
                    return;
                }
                double dv = gravitationalconstant * mass / distancesquared;
                double distance = Math.Sqrt(distancesquared);
                other.velocity.X -= (float)(dv * dx / distance);
                other.velocity.Y -= (float)(dv * dy / distance);
            }
        }

        public Boolean withinLevel() {
            if (pos.X+rect.Width<0 || pos.Y+rect.Height < 0) {
                return false;
            }
            if (pos.X >w || pos.Y >h) {
                return false;
            }

            return true;
        }

        public void draw(SpriteBatch sb, Boolean centered, Texture2D pic) {
            sb.Draw(pic, rect, Color.White);
        }

        public void draw(SpriteBatch sb, Boolean centered, Texture2D pic, float rotation)
        {
            if (centered)
            {
                Rectangle drect = new Rectangle(rect.X - rect.Width / 2, rect.Y - rect.Height / 2, rect.Width, rect.Height);
                sb.Draw(pic, drect, null, Color.White, rotation, new Vector2(pic.Width / 2, pic.Height / 2), SpriteEffects.None, 0);
            }
            else
            {
                sb.Draw(pic, rect, null, Color.White, rotation, new Vector2(0, 0), SpriteEffects.None, 0);
            }
        }

        public void resetcamera() {
            offsetX = 0;
            offsetY = 0;
        }
    }
}
