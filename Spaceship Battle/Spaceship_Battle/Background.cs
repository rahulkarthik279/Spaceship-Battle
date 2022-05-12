using System;
using System.Threading;
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
using System.IO;

namespace Spaceship_Battle
{
    class Background
    {
        public static GraphicsDevice g;
        Bitmap hugeimage;
        Texture2D[,] text;
        System.Drawing.Rectangle[,] rect;
        Microsoft.Xna.Framework.Rectangle[,] drect;
        public int w, h, xrect, yrect;

        public Background(String imagePath)
        {
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
            rect = new System.Drawing.Rectangle[xdivider, ydivider];
            drect = new Microsoft.Xna.Framework.Rectangle[xdivider, ydivider];
            Thread[,] t = new Thread[xdivider, ydivider];

            for (int a = 0; a < text.GetLength(0); a++)
            {
                for (int b = 0; b < text.GetLength(1); b++)
                {
                    int tempa = a;
                    int tempb = b;
                    Console.WriteLine("in loop" + a + " " + b);
                    t[a,b] = new Thread(new ThreadStart(() => loadpiece(tempa,tempb)));
                    t[a, b].Start();
                    //loadpiece(a, b);
                }
            }

            for (int a = 0; a < text.GetLength(0); a++)
            {
                for (int b = 0; b < text.GetLength(1); b++)
                {
                    t[a, b].Join();
                }
            }
        }

        public void loadpiece(int a,int b) {
            Console.WriteLine("Start loading piece " + a + ", " + b);
            text[a, b] = new Texture2D(g, xrect, yrect);
            rect[a, b] = new System.Drawing.Rectangle(a * xrect, b * yrect, xrect, yrect);
            drect[a, b] = new Microsoft.Xna.Framework.Rectangle(a * xrect, b * yrect, xrect, yrect);
            Bitmap thisslice;
            lock (hugeimage)
            {
                thisslice = CropImage(hugeimage, rect[a, b]);
            }
            using (MemoryStream s = new MemoryStream())
            {
                thisslice.Save(s, System.Drawing.Imaging.ImageFormat.Png);
                s.Seek(0, SeekOrigin.Begin); //must do this, or error is thrown in next line
                text[a, b] = Texture2D.FromStream(g, s);
            }
            Console.WriteLine("Loaded piece " + a + ", " + b);
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

        public static Bitmap CropImage(Bitmap orgImg, System.Drawing.Rectangle sRect)
        {
            return orgImg.Clone(sRect,orgImg.PixelFormat);
        }
    }
}
