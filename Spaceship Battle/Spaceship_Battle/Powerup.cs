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
    class Powerup : GravityBody
    {
        static Level level;
        static Random rand;
        static List<Powerup> list;

        public int timer;
        public static Texture2D pic;
        public bool pickedup;
        public string messageFromPowerUp;
        public static SpriteFont f1;



        public Powerup(Rectangle r) : base(0, 0, 0, r)
        {
            messageFromPowerUp = "";
            pickedup = false;
            timer = 60;
        }



        public new void update()
        {
            base.update();
            if (pickedup) {
                timer--;
            }else {
                if (Level.player.rect.Intersects(rect))
                {
                    intersected();
                }
            }
        }

        public static void updateAll() {
            for (int i = list.Count-1; i >=0; i--) {
                if (list[i].timer < 0)
                {
                    list.RemoveAt(i);
                }
                else {
                    list[i].update();
                }
            }
        }

        public static void initialize(int numPowerups) {
            list.Clear();
            for (int i = 0; i < numPowerups; i++)
            {
                list.Add(new Powerup(new Rectangle(rand.Next(500, level.world.Width), rand.Next(10, level.world.Height), 20, 20)));
            }
        }

        public static void loadcontent(ContentManager content, Level l) {
            pic = content.Load<Texture2D>("white");
            f1 = content.Load<SpriteFont>("SpriteFont1");
            level = l;
            rand = new Random();
            list = new List<Powerup>();
        }

        public void intersected() {
            pickedup = true;
            messageFromPowerUp = chooseRandomPowerUp();
        }

        public string chooseRandomPowerUp()
        {
            int picker = rand.Next(3);
            switch (picker)
            {
                case 0:
                    return addHealth();
                case 1:
                    return addBullets();
                default:
                    return invincibility();
            }
        }

        private string addHealth()
        {
            int addedHealth = rand.Next(1, 4) * 5;
            Level.player.health += addedHealth;
            if (Level.player.health > 100)
            {
                Level.player.health = 100;
            }
            return "Added " + addedHealth + " health.";
        }
        private string addBullets()
        {
            int addedBullets = rand.Next(1, 3) * 10;
            Level.player.gun.capacity += addedBullets;
            return "Added " + addedBullets + " bullets.";
        }
        private string invincibility()
        {
            Level.player.setInvisible();
            return "Invincibility for 10 seconds.";
        }


        public void draw(SpriteBatch sb)
        {
            if(pickedup){
                sb.DrawString(f1, messageFromPowerUp, new Vector2(350, 10), Color.White);
                return;
            }

            Color[] colors = { Color.Blue, Color.Green, Color.Orange };
            sb.Draw(pic, rect, colors[rand.Next(0, colors.Length)]);
        }

        public static void drawAll(SpriteBatch sb) {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].draw(sb);
            }
        }
    }
}