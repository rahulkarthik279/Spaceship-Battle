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
        int numActive;
        public static int health = 150;

        public Turret(Rectangle r) : base(0, 0, 0, new Rectangle())
        {
            rect = r;
            pos.X = level.world.Width / 2 - rect.Width / 2;
            pos.Y = level.world.Height / 2 - rect.Height / 2;
        }

        public static void loadcontent(ContentManager c, Level l) {
            text = c.Load<Texture2D>("turrent");
            level = l;
            list = new List<Turret>(1);
            list.Add(new Turret(new Rectangle(0, 0, 100, 100)));
        }

        public new void update()
        {
            base.update();
            rotation += 30;
            Bullet.list.Add(new Bullet(level, rect.X + rect.Width / 2 - level.getoffset(0), rect.Y + rect.Height / 2 - level.getoffset(1), MathHelper.ToRadians(rotation), 15, false));
            numActive++;
        }

        public static void updateAll() {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].update();
            }
        }

        public void draw(SpriteBatch sb, GameTime gt)
        {
            sb.Draw(text, rect, null, Color.White, MathHelper.ToRadians(rotation), new Vector2(text.Width / 2, text.Height / 2), new SpriteEffects(), 0);
        }

    }
}