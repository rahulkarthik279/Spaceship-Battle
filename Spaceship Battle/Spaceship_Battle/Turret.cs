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
        public List<Bullet> list;
        public Level level;
        public int rotation;
        int numActive;
        public static int health = 150;
        public Turret(Level l) : base(0, 0, 0, new Rectangle(0, 0, 100, 100))
        {
            level = l;
            pos.X = level.world.Width / 2 - rect.Width / 2;
            pos.Y = level.world.Height / 2 - rect.Height / 2;
            list = new List<Bullet>();
        }
        public void update()
        {
            base.update();
            rotation += 30;
            list.Add(new Bullet(level, rect.X + rect.Width / 2 - level.getoffset(0), rect.Y + rect.Height / 2 - level.getoffset(1), MathHelper.ToRadians(rotation), 15, Color.Red, false));
            list[numActive].isFired = true;
            numActive++;
            for(int i = 0; i < list.Count; i++)
            {
                list[i].update();
                list[i].intersectsPlayer(level.player);
            }
        }
        public void intersectsPlayerBullets(List<Bullet> list2)
        {
            for(int i = 0; i < list2.Count; i++)
            {
                for(int j = 0; j < list.Count; j++)
                {
                    list2[i].intersectsBullet(list[j]);
                }
            }
        }
        public void draw(SpriteBatch sb, GameTime gt)
        {
            sb.Draw(text, rect, null, Color.White, MathHelper.ToRadians(rotation), new Vector2(text.Width / 2, text.Height / 2), new SpriteEffects(), 0);
            for(int i = 0; i < list.Count; i++)
            {
                if(list[i].isFired && !list[i].isDestroyed)
                {
                    list[i].draw(sb, gt);
                }
            }
        }
    }
}
