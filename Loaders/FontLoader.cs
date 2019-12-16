using FontRenderer.Data;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Font = FontRenderer.Data.Font;

namespace FontRenderer.Loaders
{
    class FontLoader
    {
        public Font Load(TextureLoader loader, string font, string atlas)
        {
            using (var bmp = (Bitmap)Bitmap.FromFile(atlas))
            {
                Font data = null;

                using (StreamReader sr = new StreamReader(font))
                {
                    var line = sr.ReadLine();
                    string[] split = null;
                    while (line != null)
                    {
                        split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                        switch (split[0])
                        {
                            case "info":

                                int size = 0;
                                for (int i = 0; i < split.Length; i++)
                                    if(split[i].Contains("size"))
                                    {
                                        size = int.Parse(split[i].Split('=')[1]);
                                        break;
                                    }

                                line = sr.ReadLine();
                                split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                                var lineHeight = int.Parse(split[1].Split('=')[1]);
                                data = new Font(loader.Load(bmp), bmp.Width, bmp.Height, size, lineHeight);
                                break;

                            case "char":
                                if (data == null) break;

                                data.Add((char)int.Parse(split[1].Split('=')[1]), new CharData(
                                    new Vector2(int.Parse(split[2].Split('=')[1]), int.Parse(split[3].Split('=')[1])),
                                    int.Parse(split[4].Split('=')[1]),
                                    int.Parse(split[5].Split('=')[1]),
                                    new Vector2(int.Parse(split[6].Split('=')[1]), int.Parse(split[7].Split('=')[1])),
                                    int.Parse(split[8].Split('=')[1])
                                ));
                                break;
                        }

                        line = sr.ReadLine();
                    }
                }

                return data;
            }
        }
    }
}
