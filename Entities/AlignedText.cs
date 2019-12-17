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
        public float LetterPadding { get; set; }
        public Align Align { get; set; }

        public AlignedText(Font font, Vector3 position, float lineLength, float letterPadding, Align align, float fontSize):base(font, position, fontSize)
        {
            this.LineLength = lineLength;
            this.Align = align;
            this.LetterPadding = letterPadding;
        }
    }
}
