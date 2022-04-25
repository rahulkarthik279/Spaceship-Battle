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
    class Planet : GravityBody
    {
        public static List<Planet> list;
        public static Texture2D[] pics;
        public Planet moon;
        Texture2D pic;
        public int type;
        //type determine the kind of planet
        //1 - big orange planet
        //2 - smaller blue planet
        //3 - moon

        public Planet(int type, float x, float y) : base(0,0,0, new Rectangle())
        {
            pic = pics[type - 1];
            rect = new Rectangle((int)x, (int)y, pic.Bounds.Width/8, pic.Bounds.Height/8);
            pos = new Vector2(x, y);
            typeswitch(type);
            moon = null;
            this.type = type;
        }

        public Planet(int type, int x, int y, bool addMoon) : base(0, 0, 0, new Rectangle())
        {
            pic = pics[type - 1];
            rect = new Rectangle((int)x, (int)y, pic.Bounds.Width / 8, pic.Bounds.Height / 8);
            pos = new Vector2(x, y);
            typeswitch(type);
            this.type = type;
            if (addMoon)
            {
                moon = new Planet(3,pos.X, pos.Y-50);
            }
            else {
                moon = null;
            }
        }

        public void typeswitch(int type) {
            switch (type)
            {
                case 1:
                    mass = 40000;
                    break;
                case 2:
                    mass = 5000;
                    break;
                case 3:
                    mass = 1000;
                    velocity = new Vector2(2, 0);
                    break;
            }
        }



        public static void LoadContent(ContentManager content)
        {
            pics = new Texture2D[] { content.Load<Texture2D>("planet"), content.Load<Texture2D>("planet2"), content.Load<Texture2D>("moon") };
            list = new List<Planet>();
        }

        public static void updateAll() {
            for (int j = 0; j < list.Count; j++)
            {
                list[j].update();
            }
        }

        public new void update()
        {
            base.update();
            if (moon!=null) {
                this.physicsstuff(moon);
                moon.update();
            }

            //enemies
            for (int i = 0; i < Enemy.list.Count(); i++)
            {
                physicsstuff(Enemy.list[i]);
            }

            //bullets
            for (int i = 0; i < Bullet.list.Count(); i++)
            {
                physicsstuff(Bullet.list[i]);
            }

            //fireballs
            for (int i = 0; i < Fireball.list.Count(); i++)
            {
                physicsstuff(Fireball.list[i]);
            }
            //player
            physicsstuff(Level.player);
        }

        public void draw(SpriteBatch sb)
        {
            base.draw(sb, false, pic);
            if (moon!=null)
            {
                moon.draw(sb);
            }
        }

        public static void drawAll(SpriteBatch sb) {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].draw(sb);
            }
        }

        public new void physicsstuff(GravityBody other)
        {
            //ignore 0 masses
            if (this.mass > 0 && other.mass > 0)
            {
                double dx = other.pos.X - pos.X - rect.Width / 2;
                double dy = other.pos.Y - pos.Y - rect.Height / 2;
                double distancesquared = Math.Pow(dx, 2) + Math.Pow(dy, 2);
                if (distancesquared < 20)
                {
                    return;
                }
                double dv = gravitationalconstant * mass / distancesquared;
                double distance = Math.Sqrt(distancesquared);
                other.velocity.X -= (float)(dv * dx / distance);
                other.velocity.Y -= (float)(dv * dy / distance);
            }
            if (moon != null && other != moon)
            {
                moon.physicsstuff(other);
            }
        }


        
    }
}
