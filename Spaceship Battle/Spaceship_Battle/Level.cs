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

        public Level() {

        }

        public static void LoadContent(IServiceProvider service, int w, int h) {
            Planet.LoadContent(service, w, h);
        }
        public void update(GameTime gt) {

        }
    }
}
