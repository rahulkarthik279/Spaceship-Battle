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
    class Level
    {
        int numLevel;
        int timerBetweenLevels = 300;


        public Rectangle world;

        Texture2D worldText;
        Background bk;

        public static Player player;

        Rectangle unfilledHealthBar;
        Rectangle healthBar;
        Texture2D unfilledText;
        Texture2D healthBarText;
        int numPowerups;

        public int timer = 0;
        Random rand = new Random();
        public int width, height;
        SpriteFont f1;

        public Level(IServiceProvider sp, GraphicsDevice g, int w, int h) {
            ContentManager content = new ContentManager(sp, "Content");
            width = w;
            height = h;
            bk = new Background(g, @"Content\smallmountain.jpg");
            world = new Rectangle(0, 0, bk.w, bk.h);
            GravityBody.w = bk.w;
            GravityBody.h = bk.h;
            f1 = content.Load<SpriteFont>("nextlevelfont");
            
            content = new ContentManager(sp, "Content");
            worldText = content.Load<Texture2D>("space");
            
            Gun.text = content.Load<Texture2D>("spaceship_rifle");

            Planet.LoadContent(content);
            Meteor.LoadContent(content , world.Width, world.Height);
            Enemy.loadcontent(content, 12, this); // second number represent numEnmies
            Fireball.loadcontent(content);
            Bullet.loadcontent(content);
            Powerup.loadcontent(content, this);
            
            numLevel = 1;
            
            //powerups
            
            numPowerups = 10;
            


            player = new Player(this, new Rectangle(120, h / 2, 60, 30));
            player.text = content.Load<Texture2D>("playerspaceship");
            //player = new Player(this, new Rectangle(30, h / 2, 60, 30));
            //player.LoadContent();
            
            unfilledHealthBar = new Rectangle(30, 30, 100, 20);
            healthBar = unfilledHealthBar;
            unfilledText = content.Load<Texture2D>("box (1)");
            healthBarText = content.Load<Texture2D>("whiterectangle");
        }

        public static void LoadContent(IServiceProvider sp, int w, int h) {
            
        }

        public void update(GameTime gt)
        {

            //gravity stuff
            player.update(gt);
            GravityBody.offsetX = world.X;
            GravityBody.offsetY = world.Y;

            Meteor.updateAll();
            Planet.updateAll();
            Enemy.updateAll();
            Fireball.updateAll();
            Powerup.updateAll();
            Bullet.updateAll();

            if (player.health <= 0)
            {
                newLevel(false);
            }

            //finish level move on 
            if (player.pos.X + player.rect.Width >= world.Width)
            {
                if (timerBetweenLevels == 300)
                {
                    numLevel++;
                }
                if (timerBetweenLevels > 0)
                {
                    timerBetweenLevels--;
                    if (timerBetweenLevels == 0)
                    {
                        newLevel(true);
                    }
                }
            }
            healthBar.Width = player.health;



            timer++;
        }

        public void draw(SpriteBatch sb, GameTime gt)
        {
            //draw background
            //sb.Draw(worldText, world, Color.White);
            if (timerBetweenLevels == 300)
            {
                bk.draw(sb, GravityBody.offsetX, GravityBody.offsetY);


                Planet.drawAll(sb);
                Meteor.drawAll(sb);
                Powerup.drawAll(sb);
                Enemy.drawAll(sb);
                Bullet.drawAll(sb);
                Fireball.drawAll(sb);

                //player
                if (player.health > 0)
                {
                    player.draw(sb, gt);
                    player.gun.draw(sb, gt, true);
                }

                //healthbars
                sb.Draw(unfilledText, unfilledHealthBar, Color.White);
                sb.Draw(healthBarText, healthBar, Color.White);
            }
            else
            {
                sb.DrawString(f1, "Level " + (numLevel) + " coming up in", new Vector2(width / 2 - 20, 50), Color.White);
                sb.DrawString(f1, (timerBetweenLevels / 60 + 1) + "", new Vector2(width / 2, 140), Color.Blue);
            }
        }


        public int getoffset(int a) {
            // if a==0 return x offset, any other number gives y offset
            if (a == 0)
            {
                return world.X;
            }
            return world.Y;
        }

        public void newLevel(bool nextLevel)
        {
            if (nextLevel)
            {
                //world.Width += 1000;
                if (numLevel == 2)
                {
                    Fireball.isActivated = true;
                }
                player.gun.capacity += 20;
                Enemy.numEnemies += 5;
                numPowerups += 5;
            }
            timerBetweenLevels = 300;
            player.pos.X = 120;
            player.pos.Y = height / 2;
            world.X = 0; world.Y = 0;
            GravityBody.offsetX = 0;
            GravityBody.offsetY = 0;
            player.velocity.X = 0;
            player.velocity.Y = 0;
            player.health = 100;
            player.gun.numActive = 0;

            Enemy.list.Clear();

            Powerup.initialize(numPowerups);
        }

    }
}

