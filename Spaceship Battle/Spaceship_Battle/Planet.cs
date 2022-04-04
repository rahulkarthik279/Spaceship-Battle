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
        public static Texture2D[] pics;
        public static int gravityfactor;
        Planet moon;
        public bool hasMoon;
        Texture2D pic;
        public Rectangle drect;
        //type determine the kind of planet
        //1 - big orange planet
        //2 - smaller blue planet
        //3 - moon

        public Planet(int type, float x, float y) : base(0,x,y,0,0)
        {
            pic = pics[type - 1];
            drect = new Rectangle((int)x, (int)y,pic.Bounds.Width/8, pic.Bounds.Height/8);
            switch (type) {
                case 1:
                    base.mass = 500;
                    break;
                case 2:
                    base.mass = 100;
                    break;
                case 3:
                    base.mass = 10;
                    base.velocity = new Vector2(5, 0);
                    break;
            }
            hasMoon = false;
            moon = null;
        }

        public Planet(int type, int x, int y, bool addMoon) : base(0, x, y, 0, 0)
        {
            pic = pics[type - 1];
            drect = new Rectangle((int)x, (int)y, pic.Bounds.Width / 8, pic.Bounds.Height / 8);
            switch (type)
            {
                case 1:
                    base.mass = 5000;
                    break;
                case 2:
                    base.mass = 1000;
                    break;
                case 3:
                    base.mass = 100;
                    base.velocity = new Vector2(5, 0);
                    break;
            }
            hasMoon = addMoon;
            if (addMoon)
            {
                moon = new Planet(3,pos.X, pos.Y-200);
            }
            else {
                moon = null;
            }
        }

        public static void LoadContent(IServiceProvider service, int w, int h)
        {
            ContentManager content = new ContentManager(service);
            pics = new Texture2D[3] { content.Load<Texture2D>("planet"), content.Load<Texture2D>("planet2"), content.Load<Texture2D>("moon") };
        }

        public void update(GameTime gt)
        {
            if (mass > 100) {
                update(moon);
            }
        }

        public void draw(GameTime gt, SpriteBatch sb)
        {
            drect.X = (int)pos.X + drect.Width / 2;
            drect.Y = (int)pos.Y + drect.Height / 2;
            sb.Draw(pic, drect, Color.White);
        }
    }
}
