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
        public static GraphicsDevice g;
        Bitmap hugeimage;
        Texture2D[,] text;
        Microsoft.Xna.Framework.Rectangle[,] rect;
        Microsoft.Xna.Framework.Rectangle[,] drect;
        public int w, h, xrect, yrect;

        public Background(String imagePath) {
            Console.WriteLine("Being import image");
            hugeimage = (Bitmap)Bitmap.FromFile(imagePath);
            Console.WriteLine("Finished bitmap loading");
            w = hugeimage.Width;
            h = hugeimage.Height;

            int xdivider = w / 2048 + 1;
            int ydivider = h / 2048 + 1;

            xrect = w / xdivider;
            yrect = h / ydivider;

            text = new Texture2D[xdivider, ydivider];
            rect = new Microsoft.Xna.Framework.Rectangle[xdivider, ydivider];
            drect = new Microsoft.Xna.Framework.Rectangle[xdivider, ydivider];

            Microsoft.Xna.Framework.Color[] data = new Microsoft.Xna.Framework.Color[xrect * yrect];
            for (int a = 0; a < text.GetLength(0); a++)
            {
                for (int b = 0; b < text.GetLength(1); b++)
                {
                    text[a, b] = new Texture2D(g, xrect, yrect);
                    rect[a, b] = new Microsoft.Xna.Framework.Rectangle(a * xrect, b * yrect, xrect, yrect);
                    drect[a,b] = new Microsoft.Xna.Framework.Rectangle(a * xrect, b * yrect, xrect, yrect);
                    int dataindex = 0;

                    for (int i = 0; i < yrect; i++)
                    {
                        for (int j = 0; j < xrect; j++)
                        {
                            System.Drawing.Color c = hugeimage.GetPixel(j + a * xrect, i + b * yrect);
                            data[dataindex] = new Microsoft.Xna.Framework.Color(c.R, c.G, c.B);
                            dataindex++;
                        }
                    }
                    //data[1] = Microsoft.Xna.Framework.Color.Black;
                    Console.WriteLine("Loaded image " + a + ", " + b);
                    text[a, b].SetData(data);
                }
            }
        }

        public void draw(SpriteBatch sb, int xoffset, int yoffset) {

            for (int a = 0; a < text.GetLength(0); a++)
            {
                for (int b = 0; b < text.GetLength(1); b++) {
                    drect[a, b].X = rect[a,b].X+xoffset;
                    drect[a, b].Y = rect[a,b].Y+yoffset;
                    sb.Draw(text[a, b], drect[a, b], Microsoft.Xna.Framework.Color.White);
                }
            }
        }

        public static void initialize(GraphicsDevice graphics) {
            g = graphics;
        }
    }
}
