using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using FontRenderer.Models;

namespace FontRenderer.Data
{
    class Font
    {
        public Texture Atlas { get; private set; }
        public int AtlasWidth { get; private set; }
        public int AtlasHeight { get; private set; }
        public Dictionary<char, CharData> Data { get; private set; }
        public int GivenSize { get; private set; }
        public int LineHeight { get; private set; }

        public Font(Texture atlas, int atlasWidth, int atlasHeight, int givenSize, int lineHeight)
        {
            this.Atlas = atlas;
            this.AtlasWidth = atlasWidth;
            this.AtlasHeight = atlasHeight;

            this.GivenSize = givenSize;
            this.LineHeight = lineHeight;
            
            this.Data = new Dictionary<char, CharData>();
        }

        public void Add(char character, CharData data) => this.Data.Add(character, data);

        public int GetLineLength(string text)
        {
            int length = 0;
            foreach (var character in text)
                length += this.Data[character].Width;

            return length;
        }
    }
}
