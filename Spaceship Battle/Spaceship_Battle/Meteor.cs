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
    class Meteor:GravityBody
    {
        public static Texture2D text;
        public static List<Meteor> meteors;
        public static Random rn;
        public float rotation;

        public Meteor(): base(500, 0,0, new Rectangle(0,0,72,50))
        {
            while (velocity.X == 0)
            {
                velocity.X = rn.Next(-10, 10);
            }
            while (velocity.Y == 0)
            {
                velocity.Y = rn.Next(-10, 10);
            }
                pos.X = rn.Next(w / 4, 3 * w / 4);
            
            if (velocity.Y > 0)
            {
                pos.Y = 0;
            }
            else{
                pos.Y = h;
            }
            rotation = (float) Math.Atan2(velocity.Y, velocity.X) + 3.2f;

            this.update();
        }

        public static void updateAll() {
            for(int i=meteors.Count-1; i>=0; i--)
            {
                if (meteors[i].update()) {
                    meteors.RemoveAt(i);
                }
            }

            if (rn.Next(60) == 1)
            {
                meteors.Add(new Meteor());
            } 
        }

        public new Boolean update() {
            base.update();
            

            if (rect.Intersects(Level.player.rect)) {
                Level.player.health -= 10;
                return true;
            }
            for (int i = 0; i < Enemy.list.Count; i++) {
                if (rect.Intersects(Enemy.list[i].rect))
                {
                    Enemy.list[i].health = 0;
                    return true;
                }
            }
            for (int i = 0; i < Bullet.list.Count; i++)
            {
                if (rect.Intersects(Bullet.list[i].rect))
                {
                    Bullet.list[i].isDestroyed = true;
                    return true;
                }
            }

            if (withinLevel())
            {
                return false;
            }

            return true;
        }
        
        public static void LoadContent(ContentManager content, int width, int height) {
            text = content.Load<Texture2D>("meteor");
            meteors = new List<Meteor>();
            rn = new Random();
            w = width;
            h = height;
        }

        public static void drawAll(SpriteBatch sb)
        {
            for (int i = 0; i < meteors.Count; i++)
            {
                //sb.Draw(text, meteors[i].rect, Color.White);
                meteors[i].draw(sb, true, text, meteors[i].rotation);
            }
        }


    }
}
