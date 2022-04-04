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
    class LoadingScreen
    {
        SpriteFont f;
        Vector2 pos;
        ContentManager content;
        public LoadingScreen(IServiceProvider s,int w, int h) {
            content = new ContentManager(s, "Content");
            f = content.Load<SpriteFont>("SpriteFont1");
            pos = new Vector2(20, 20);
        }
        public void draw(SpriteBatch sb) {
            sb.DrawString(f, "Loading...", pos, Color.White);
        }
    }
}
