using FontRenderer.Data;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace FontRenderer.Entities
{
    class AlignedText: Text
    {
        public float LineLength { get; set; }
        public Align Align { get; set; }

        public AlignedText(Font font, Vector3 position, float lineLength, Align align, float fontSize):base(font, position, fontSize)
        {
            this.LineLength = lineLength;
            this.Align = align;
        }
    }
}
