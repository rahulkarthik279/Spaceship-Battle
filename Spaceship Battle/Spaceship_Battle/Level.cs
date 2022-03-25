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

        int timer = 0;
        Random rand = new Random();
        public int width, height;

        public Level(IServiceProvider sp, GraphicsDevice g, int w, int h) {
            ContentManager content = new ContentManager(sp, "Content");
            width = w;
            height = h;
            world = new Rectangle(0, 0, 3000, 400);
            //bk = new Background(g, @"Content\huge mountain.jpg");
            enemies = new List<Enemy>();
            content = new ContentManager(sp, "Content");
            worldText = content.Load<Texture2D>("space");
            Enemy.text = content.Load<Texture2D>("airplane");
            Bullet.text = content.Load<Texture2D>("redRectForBorg");
            Gun.text = content.Load<Texture2D>("spaceship_rifle");
            Planet.pics = new Texture2D[] { content.Load<Texture2D>("planet"), content.Load<Texture2D>("planet2"), content.Load<Texture2D>("moon") };

            player = new Player(this, new Rectangle(120, h / 2, 60, 30));
            player.text = content.Load<Texture2D>("playerspaceship");
            //player = new Player(this, new Rectangle(30, h / 2, 60, 30));
            //player.LoadContent();
            planets = new Planet[] { new Planet(1, 200, 200, true) };
            numEnemies = 0;// rand.Next(5, 11);
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
                planets[i].update(gt);
            }

            List<Bullet> bullets = player.gun.bullets;
            for (int i = 0; i < enemies.Count; i++)
            {
                if (player.rect.Intersects(enemies[i].rect))
                {
                    if (enemies[i].health != 0)
                    {
                        player.health -= 30;
                    }
                    enemies[i].health = 0;

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
                }
                enemies[i].update();
            }

            player.update(gt);
            GravityBody.offsetX = world.X;
            GravityBody.offsetY = world.Y;
            for (int j = 0; j < planets.Length; j++)
            {
                for (int i = 0; i < enemies.Count(); i++)
                {
                    planets[j].physicsstuff(enemies[i]);
                }
                planets[j].physicsstuff(player);
            }


            if (timer % 240 == 0 && enemies.Count < numEnemies)
            {
                enemies.Add(new Enemy(this, new Rectangle(900-getoffset(0), rand.Next(0, world.Height - 30), 60, 30)));
            }
            if (player.rect.X + player.rect.Width == width)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].health = 0;
                }
            }
            healthBar.Width = player.health;



            timer++;
        }

        public void draw(SpriteBatch sb, GameTime gt)
        {
            //draw background
            sb.Draw(worldText, world, Color.White);

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
    }
}

