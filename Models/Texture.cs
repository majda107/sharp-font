using System;
using System.Collections.Generic;
using System.Text;

namespace FontRenderer.Models
{
    class Texture
    {
        public int Id { get; private set; }
        public Texture(int id)
        {
            this.Id = id;
        }
    }
}
