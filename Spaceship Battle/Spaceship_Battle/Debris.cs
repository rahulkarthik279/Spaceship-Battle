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
    class Debris : GravityBody
    {
        public static Texture2D text;
        public static List<Debris> list;
        public static Level level;
        public static int maxCapacity = 10;
        public static Random rand;
        public Debris(int posX, int posY, int width, int height) : base(0, 0, 0, new Rectangle(0, 0, 0,0))
        {
            pos.X = posX;
            pos.Y = posY;
            rect.Width = width;
            rect.Height = height;
        }

        public static void updateAll()
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].update();
                list[i].intersects(Level.player);
            }
        }

        public static void LoadContent(ContentManager content, Level l)
        {
            text = content.Load<Texture2D>("debris");
            rand = new Random();
            list = new List<Debris>();
            level = l;
            //for(int i = 0; i < maxCapacity; i++)
            //{
            //    list.Add(new Debris(rand.Next(0, level.world.Width), rand.Next(0, level.world.Height)));
            //}
        }

        public bool intersects(Player p)
        {
            if (rect.Intersects(p.rect) && !p.isInvincible)
            {
                Player.health -= 0.2;
                return true;
            }
            return false;
        }

        public static void drawAll(SpriteBatch sb)
        {
            for (int i = 0; i < list.Count; i++)
            {
                sb.Draw(text, list[i].rect, Color.White);
            }
        }
    }
}