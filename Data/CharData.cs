using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;

namespace FontRenderer.Data
{
    class CharData
    {
        public Vector2 Position { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Vector2 Offset { get; private set; }
        public int Xadvance { get; private set; }

        public CharData(Vector2 position, int width, int height, Vector2 offset, int xadvance)
        {
            this.Position = position;
            this.Width = width;
            this.Height = height;
            this.Offset = offset;
            this.Xadvance = xadvance;
        }
    }
}
