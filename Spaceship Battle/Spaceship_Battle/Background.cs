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
using System.Drawing;

namespace Spaceship_Battle
{
    class Background
    {
        Bitmap bp;
        Texture2D[,] text;
        Microsoft.Xna.Framework.Rectangle[,] rect;
        int w, h, xrect, yrect;

        public Background(GraphicsDevice g, String imagePath) {
            bp = new Bitmap(imagePath);
            w = bp.Width;
            h = bp.Height;
            int xdivider = w / 2048 + 1;
            int ydivider = h / 2048 + 1;
            xrect = w / xdivider;
            yrect = h / ydivider;

            text = new Texture2D[w,h];
            rect = new Microsoft.Xna.Framework.Rectangle[w, h];

            for (int i = 0; i < text.GetLength(0); i++) {
                for (int j = 0; j < text.GetLength(1); i++) {
                    text[i, j] = new Texture2D(g, xrect, yrect);
                    //text[i,j].SetData<Color>(new Color(), 0,  )
                }
            }
        }

        public void draw(int xoffset, int yoffset) {

        }
    }
}
