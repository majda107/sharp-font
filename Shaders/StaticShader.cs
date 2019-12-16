using System;
using System.Collections.Generic;
using System.Text;
using FontRenderer.Data;
using FontRenderer.Entities;
using OpenTK;

namespace FontRenderer.Shaders
{
    class StaticShader : ShaderProgram
    {
        private int projectionMatrixLocation;
        private int modelMatrixLocation;
        private int lineLengthLocation;
        protected override void BindAttribLocations()
        {
            this.BindAttribLocation(0, "position");
            this.BindAttribLocation(1, "textureCoord");
        }

        protected override void GetUniformsLocations()
        {
            this.projectionMatrixLocation = this.GetUniformLocation("projectionMatrix");
            this.modelMatrixLocation = this.GetUniformLocation("modelMatrix");
            this.lineLengthLocation = this.GetUniformLocation("lineLength");
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

        public void LoadTextData(AlignedText text)
        {
            this.LoadFloat(text.LineLength / (float)text.FontSize, this.lineLengthLocation);
        }
    }
}
