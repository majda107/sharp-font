using FontRenderer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using FontRenderer.Data;
using FontRenderer.Meshes;

namespace FontRenderer.Entities
{
    class Text
    {
        public FontMesh Mesh { get; private set; }
        public Vector3 Position { get; set; }
        public Vector3 Color { get; set; }
        public float FontSize { get; set; }

        public Text(Font font, Vector3 position, float fontSize = 1f)
        {
            this.Mesh = new FontMesh(font);
            this.Position = position;
            this.Color = new Vector3(1, 1, 1);
            this.FontSize = fontSize;
        }
    }
}
