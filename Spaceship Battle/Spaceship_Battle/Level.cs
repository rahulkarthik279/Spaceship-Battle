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
        Planet[] planets;
        int numLevel;
        int timerBetweenLevels = 300;


        public Rectangle world;
        Texture2D worldText;
        Background bk;

        public Player player;

        List<Enemy> enemies;
        int numEnemies; //Max number of enemies spawned

        Rectangle unfilledHealthBar;
        Rectangle healthBar;
        Texture2D unfilledText;
        Texture2D healthBarText;

        List<Powerup> powerups;
        int numPowerups;

        int timer = 0;
        Random rand = new Random();
        public int width, height;

        public Level(IServiceProvider sp, GraphicsDevice g, int w, int h) {
            ContentManager content = new ContentManager(sp, "Content");
            width = w;
            height = h;
            bk = new Background(g, @"Content\smallmountain.jpg");
            world = new Rectangle(0, 0, bk.w, bk.h);
            enemies = new List<Enemy>();
            content = new ContentManager(sp, "Content");
            worldText = content.Load<Texture2D>("space");
            Enemy.text = content.Load<Texture2D>("airplane");
            Bullet.text = content.Load<Texture2D>("redRectForBorg");
            Gun.text = content.Load<Texture2D>("spaceship_rifle");
            Planet.pics = new Texture2D[] { content.Load<Texture2D>("planet"), content.Load<Texture2D>("planet2"), content.Load<Texture2D>("moon") };
            Fireball.text = content.Load<Texture2D>("save");
            
            //powerups
            Powerup.pic = content.Load<Texture2D>("redRectForBorg");
            Powerup.f1 = content.Load<SpriteFont>("SpriteFont1");
            powerups = new List<Powerup>();
            numPowerups = 10;
            for (int i = 0; i < numPowerups; i++)
            {
                powerups.Add(new Powerup(new Rectangle(rand.Next(500, world.Width), rand.Next(10, world.Height), 20, 20), this));
            }


            player = new Player(this, new Rectangle(120, h / 2, 60, 30));
            player.text = content.Load<Texture2D>("playerspaceship");
            //player = new Player(this, new Rectangle(30, h / 2, 60, 30));
            //player.LoadContent();
            planets = new Planet[] { new Planet(1, 600, 200, true) };
            numEnemies = rand.Next(5, 11);
            unfilledHealthBar = new Rectangle(30, 30, 100, 20);
            healthBar = unfilledHealthBar;
            unfilledText = content.Load<Texture2D>("box (1)");
            healthBarText = content.Load<Texture2D>("whiterectangle");
        }

        public static void LoadContent(IServiceProvider sp, int w, int h) {
            Planet.LoadContent(sp, w, h);
            
        }

        public void update(GameTime gt)
        {
            //basic nonmoving image update
            for (int i = 0; i < planets.Length; i++) {
                planets[i].update();
            }

            //Start of intersection code
            List<Bullet> bullets = player.gun.bullets;
            for (int i = 0; i < enemies.Count; i++)
            {
                if (player.rect.Intersects(enemies[i].rect) && !player.isInvincible)
                {
                    if (enemies[i].health != 0)
                    {
                        player.health -= 30;
                    }
                    enemies[i].health = 0;

                }
                for (int x = 0; x < player.fireballs.Count; x++)
                {
                    player.fireballs[x].intersectsEnemy(enemies[i]);
                }
                List<Bullet> enemyBullets = enemies[i].gun.bullets;
                for (int j = 0; j < bullets.Count; j++)
                {
                    bullets[j].intersectsEnemy(enemies[i]);
                    for (int k = 0; k < enemyBullets.Count; k++)
                    {
                        bullets[j].intersectsBullet(enemyBullets[k]);
                    }
                }
                for (int k = 0; k < enemyBullets.Count; k++)
                {
                    enemyBullets[k].intersectsPlayer(player);
                    for (int l = 0; l < player.fireballs.Count; l++)
                    {
                        player.fireballs[l].intersectsBullet(enemyBullets[k]);
                    }
                }
                enemies[i].update();
            }
            //End of intersection code

            //gravity stuff
            player.update(gt);
            GravityBody.offsetX = world.X;
            GravityBody.offsetY = world.Y;

            for (int j = 0; j < planets.Length; j++)
            {
                //enemies
                for (int i = 0; i < enemies.Count(); i++)
                {
                    planets[j].physicsstuff(enemies[i]);
                }
                //bullets
                //for (int i = 0; i < bullets.Count(); i++)
                //{
                //    planets[j].physicsstuff(enemyBullets[i]);
                //}
                //player
                planets[j].physicsstuff(player);
            }

            //enemy spawning
            if (timer % 240 == 0 && enemies.Count < numEnemies)
            {
                //enemies.Add(new Enemy(this, new Rectangle(900-getoffset(0), rand.Next(0, world.Height - 30), 60, 30)));
                enemies.Add(new Enemy(this, new Rectangle(rand.Next((int)player.pos.X,world.Width), rand.Next((int)player.pos.Y-500, world.Height), 60, 30)));
            }

            healthBar.Width = player.health;

            //powerups
            for (int i = powerups.Count - 1; i >= 0; i--)
            {
                powerups[i].update();
                if (powerups[i].timer == 60)
                {
                    if (player.rect.Intersects(powerups[i].rect))
                    {
                        powerups[i].intersected();
                    }
                }
                if (powerups[i].timer < 0) {
                    powerups.RemoveAt(i);
                }
            }

            timer++;
        }

        public void draw(SpriteBatch sb, GameTime gt)
        {
            //draw background
            //sb.Draw(worldText, world, Color.White);
            bk.draw(sb, GravityBody.offsetX, GravityBody.offsetY);

            //draw planets
            for (int i = 0; i < planets.Length; i++) {
                planets[i].draw(gt, sb);
            }

            //draw enemy
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].draw(sb, gt);
                for (int j = 0; j < enemies[i].gun.bullets.Count; j++)
                {
                    if (enemies[i].gun.bullets[j].isFired && !enemies[i].gun.bullets[j].isDestroyed)
                    {
                        enemies[i].gun.bullets[j].draw(sb, gt);
                    }
                }
            }
            if (player.health > 0)
            {
                player.draw(sb, gt);
                player.gun.draw(sb, gt, true);
            }

            //powerups
            for (int i = 0; i < powerups.Count; i++)
            {
                powerups[i].draw(sb, gt);
            }

            //healthbars
            sb.Draw(unfilledText, unfilledHealthBar, Color.White);
            sb.Draw(healthBarText, healthBar, Color.White);
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
                world.Width += 1000;
                numLevel++;
                if (numLevel == 2)
                {
                    Fireball.isActivated = true;
                }
                player.gun.capacity += 20;
                numEnemies += 5;
                numPowerups += 5;
            }
            timerBetweenLevels = 300;
            player.pos.X = 120;
            player.pos.Y = height / 2;
            player.health = 100;
            player.gun.numActive = 0;
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                enemies.RemoveAt(i);
            }
            for (int i = 0; i < numPowerups; i++)
            {
                powerups.Add(new Powerup(new Rectangle(rand.Next(500, world.Width), rand.Next(10, world.Height), 20, 20), this));
            }
        }


    }
}

