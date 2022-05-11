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
using System.IO;

namespace Spaceship_Battle
{
    class FileReader
    {
        public List<Object> objects;
        public FileReader()
        {
            objects = new List<Object>();
        }
        public void read(string path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    Console.WriteLine("file found");
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] split = line.Split(' ');
                        Console.WriteLine(line + "  "+split[0][0]);
                        if (split[0] == "p")
                        {
                            bool hasmoon = line.Contains("t");
                            int type = Convert.ToInt32(split[1]);
                            objects.Add(new Planet(type, Convert.ToInt32(split[2]), Convert.ToInt32(split[3]), hasmoon));
                        }
                        if (split[0] == "d")
                        {
                            objects.Add(new Debris(Convert.ToInt32(split[1]), Convert.ToInt32(split[2]), Convert.ToInt32(split[3]), Convert.ToInt32(split[4])));
                        }
                        if (split[0] == "b") {
                            objects.Add(new Background(split[1]));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(path+" could not be read.");
                Console.WriteLine(e.Message);
            }
        }
    }
}