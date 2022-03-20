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
    class GravityBody
    {
        public static double gravityfactor = .1;
        public static int offsetX=0, offsetY=0;
        public int mass;
        public Vector2 pos, velocity;
        public static double gravitationalconstant=.01;
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
            if (mass > 0)
            {
                double dx = other.pos.X - pos.X;
                double dy = other.pos.Y - pos.Y;
                double distancesquared = Math.Pow(dx, 2) + Math.Pow(dy, 2);
                double dv = gravitationalconstant * mass / distancesquared;
                double distance = Math.Sqrt(distancesquared);
                other.velocity.X -= (float)(dv * dx / distance);
                other.velocity.Y -= (float)(dv * dy / distance);
            }
        }
    }
}
