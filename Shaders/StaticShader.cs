using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;

namespace FontRenderer.Shaders
{
    class StaticShader : ShaderProgram
    {
        private int projectionMatrixLocation;
        private int modelMatrixLocation;
        protected override void BindAttribLocations()
        {
            this.BindAttribLocation(0, "position");
            this.BindAttribLocation(1, "textureCoord");
        }

        protected override void GetUniformsLocations()
        {
            this.projectionMatrixLocation = this.GetUniformLocation("projectionMatrix");
            this.modelMatrixLocation = this.GetUniformLocation("modelMatrix");
        }

        protected override void LoadShaders()
        {
            var vertex = this.LoadShader("../../../Shaders/Static/VertexShader.glsl", OpenTK.Graphics.OpenGL4.ShaderType.VertexShader);
            var frag = this.LoadShader("../../../Shaders/Static/FragmentShader.glsl", OpenTK.Graphics.OpenGL4.ShaderType.FragmentShader);

            this.AttachShader(vertex);
            this.AttachShader(frag);
        }

        public void LoadProjection(Matrix4 projection)
        {
            this.LoadMatrix4(projection, this.projectionMatrixLocation);
        }

        public void LoadModelMatrix(Matrix4 model)
        {
            this.LoadMatrix4(model, this.modelMatrixLocation);
        }
    }
}
