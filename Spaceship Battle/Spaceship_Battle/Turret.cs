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
    class Turret : GravityBody
    {
        public static Texture2D text;
        public static Level level;
        public static List<Turret> list;
        public int rotation;
        public int health;
        public Rectangle drect;

        public Turret(Rectangle r) : base(0, 0, 0, new Rectangle())
        {
            rect = r;
            drect = new Rectangle(rect.X + rect.Width / 2, rect.Y + rect.Height / 2, rect.Width, rect.Height);
            pos.X = level.world.Width / 2 - rect.Width / 2;
            pos.Y = level.world.Height / 2 - rect.Height / 2;
            health = 200;
        }

        public static void loadcontent(ContentManager c, Level l) {
            text = c.Load<Texture2D>("turret");
            level = l;
            list = new List<Turret>(1);
            list.Add(new Turret(new Rectangle(0, 0, 100, 100)));
        }

        public static void initialize() {
            for (int i = 0; i < list.Count; i++) {
                list[i].health = 200;
            }
        }

        public new void update()
        {
            if (health > 0)
            {
                base.update();
                rotation += 2;


                if (level.timer % 6 == 0)
                {
                    Bullet.list.Add(new Bullet(level, rect.X + rect.Width / 2 - level.getoffset(0), rect.Y + rect.Height / 2 - level.getoffset(1), MathHelper.ToRadians(rotation - 90), 15, false));
                }

                if (Level.player.rect.Intersects(rect))
                {
                    Level.player.velocity.X = 0;
                    Level.player.velocity.Y = 0;
                }

            }
        }

        public static void updateAll() {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].update();
            }
        }

        public void draw(SpriteBatch sb)
        {

            if (health > 0)
            {
                Rectangle drect = new Rectangle(rect.X + rect.Width / 2, rect.Y + rect.Height / 2, rect.Width, rect.Height);
                sb.Draw(text, drect, new Rectangle(725, 75, 85, 85), Color.White, MathHelper.ToRadians(rotation), new Vector2(85 / 2, 85 / 2), SpriteEffects.None, 0);
            }

        }

        public static void drawAll(SpriteBatch sb) {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].draw(sb);
            }
        }
    }
}