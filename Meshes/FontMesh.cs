using System;
using System.Collections.Generic;
using System.Text;
using FontRenderer.Data;
using OpenTK.Graphics.OpenGL4;

namespace FontRenderer.Meshes
{
    class FontMesh : Mesh
    {
        public FontMesh(float[] vertices, float[] textureCoords, Font font)
        {
            this.vertices = vertices;
            this.textureCoords = textureCoords;
            this.Font = font;
        }

        public FontMesh(Font font)
        {
            this.Font = font;
            this.vertices = new float[0];
            this.textureCoords = new float[0];
        }

        public Font Font { get; private set; }
        private float[] vertices;
        private float[] textureCoords;

        public void UpdateCoords(float[] vertices, float[] textureCoords)
        {
            this.vertices = vertices;
            this.textureCoords = textureCoords;
            this.Buffered = false;
        }

        public override void BufferMesh()
        {
            this.Buffered = true;

            int id = GL.GenVertexArray();
            this.vaos.Add(id);
            GL.BindVertexArray(id);

            this.StoreData(0, 2, this.vertices);
            this.StoreData(1, 2, this.textureCoords);

            GL.BindVertexArray(0);

            this.Id = id;
            this.Count = this.vertices.Length / 2;

            Array.Clear(this.vertices, 0, this.vertices.Length);
            Array.Clear(this.textureCoords, 0, this.textureCoords.Length);
        }

        private void StoreData(int location, int size, float[] data)
        {
            int vbo = GL.GenBuffer();
            this.vbos.Add(vbo);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(location, size, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        protected override void DeleteMeshModel()
        {
            foreach (var vbo in this.vbos)
                GL.DeleteBuffer(vbo);

            foreach (var vao in this.vaos)
                GL.DeleteVertexArray(vao);

            this.vaos.Clear();
            this.vbos.Clear();
        }
    }
}
