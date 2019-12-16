using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace FontRenderer.Shaders
{
    abstract class ShaderProgram
    {
        public int ProgramId { get; private set; }
        public ShaderProgram()
        {
            this.ProgramId = GL.CreateProgram();
            this.LoadShaders();

            GL.LinkProgram(this.ProgramId);
            GL.ValidateProgram(this.ProgramId);

            this.BindAttribLocations();
            this.GetUniformsLocations();

        }

        public void Start()
        {
            GL.UseProgram(this.ProgramId);
        }

        public void Stop()
        {
            GL.UseProgram(0);
        }

        protected abstract void LoadShaders();
        protected abstract void BindAttribLocations();
        protected void BindAttribLocation(int location, string name)
        {
            GL.BindAttribLocation(this.ProgramId, location, name);
        }
        protected abstract void GetUniformsLocations();

        protected int GetUniformLocation(string name)
        {
            return GL.GetUniformLocation(this.ProgramId, name);
        }

        protected void AttachShader(int id)
        {
            GL.AttachShader(this.ProgramId, id);
        }
        protected int LoadShader(string filename, ShaderType type)
        {
            if (!File.Exists(filename)) throw new FileNotFoundException();

            int id = GL.CreateShader(type);
            var content = File.ReadAllText(filename);
            GL.ShaderSource(id, content);
            GL.CompileShader(id);

            GL.GetShader(id, ShaderParameter.CompileStatus, out int status);
            if (status != 1)
            {
                GL.GetShaderInfoLog(id, out string log);
                throw new Exception(log);
            }

            return id;
        }

        protected void LoadMatrix4(Matrix4 mat, int location)
        {
            GL.UniformMatrix4(location, false, ref mat);
        }
    }
}
