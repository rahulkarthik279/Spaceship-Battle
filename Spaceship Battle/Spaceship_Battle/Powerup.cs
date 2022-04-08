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
        Level level;
        Random rand = new Random();
        public int timer;
        public static Texture2D pic;
        public bool pickedup;
        public string messageFromPowerUp;
        public static SpriteFont f1;

        public Powerup(Rectangle r, Level l) : base(0, 0, 0, r)
        {
            level = l;
            messageFromPowerUp = "";
            pickedup = false;
            timer = 60;
        }
        public new void update()
        {
            base.update();
            if (pickedup) {
                timer--;
            }
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
            level.player.health += addedHealth;
            if (level.player.health > 100)
            {
                level.player.health = 100;
            }
            return "Added " + addedHealth + " health.";
        }
        private string addBullets()
        {
            int addedBullets = rand.Next(1, 3) * 10;
            level.player.gun.capacity += addedBullets;
            return "Added " + addedBullets + " bullets.";
        }
        private string invincibility()
        {
            level.player.setInvisible();
            return "Invincibility for 10 seconds.";
        }
        public void draw(SpriteBatch sb, GameTime gt)
        {
            if(pickedup){
                sb.DrawString(f1, messageFromPowerUp, new Vector2(350, 10), Color.White);
                return;
            }

            Color[] colors = { Color.Blue, Color.Green, Color.Orange };
            sb.Draw(pic, rect, colors[rand.Next(0, colors.Length)]);
        }
    }
}