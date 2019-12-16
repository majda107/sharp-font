using FontRenderer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FontRenderer.Meshes
{
    abstract class Mesh : IDisposable
    {
        public int Id { get; protected set; }
        public int Count { get; protected set; }
        public bool Buffered { get; protected set; }

        protected List<int> vaos;
        protected List<int> vbos;


        public Mesh()
        {
            this.vaos = new List<int>();
            this.vbos = new List<int>();
        }

        public abstract void BufferMesh();
        protected abstract void DeleteMeshModel();

        public void Dispose()
        {
            this.DeleteMeshModel();
        }

        ~Mesh() { this.Dispose(); }
    }
}
