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
        public int mass;
        public Vector2 pos, velocity;
        public static double gravitationalconstant=.01;

        public GravityBody(int mass, Vector2 pos, Vector2 v) {
            this.mass = mass;
            this.pos = pos;
            this.velocity = v;
        }

        public GravityBody(int mass, double x, double y, double vx, double vy)
        {
            this.mass = mass;
            pos = new Vector2((float) x, (float) y);
            pos = new Vector2((float)vx, (float)vy);
        } 

        public GravityBody(int mass, float x, float y, float vx, float vy) {
            this.mass = mass;
            pos = new Vector2(x, y);
            pos = new Vector2(vx, vy);
        }

        public GravityBody(int mass, int x, int y, int vx, int vy)
        {
            this.mass = mass;
            pos = new Vector2(x, y);
            pos = new Vector2(vx, vy);
        }


        public void update(GravityBody other)
        {
            //ignore trivial masses
            if (mass > 1)
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
