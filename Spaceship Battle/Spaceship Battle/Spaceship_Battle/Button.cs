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
    class Button
    {
        public Rectangle drect;
        public Rectangle[] sourcearray;
        public Rectangle srect;
        public String insideText;
        public bool active;
        public Button(Rectangle d, Rectangle[] s, String c) {
            drect = d;
            sourcearray = s;
            srect = sourcearray[0];
            insideText = c;
        }
        public void setActive(bool input) {
            if (input)
            {
                srect = sourcearray[1];
            }
            else {
                srect = sourcearray[0];
            }
        }

    }
}
