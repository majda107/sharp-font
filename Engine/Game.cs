using FontRenderer.Builders;
using FontRenderer.Data;
using FontRenderer.Loaders;
using FontRenderer.Meshes;
using FontRenderer.Render;
using System;
using System.Collections.Generic;
using System.Text;

using OpenTK.Graphics.OpenGL4;
using FontRenderer.Shaders;
using OpenTK;
using FontRenderer.Entities;
using OpenTK.Input;

namespace FontRenderer.Engine
{
    class Game
    {
        public Display Display { get; private set; }

        public StaticShader Shader { get; private set; }

        public FontLoader FontLoader { get; private set; }
        public TextureLoader TextureLoader { get; private set; }
        public FontMeshBuilder FontMeshBuilder { get; private set; }
        public Font Font { get; private set; }

        private Matrix4 projection;

        //private Text fontEntity;
        private AlignedText alignedText;

        private string textString;

        public Game()
        {
            this.Display = new Display(1280, 720);
            this.HookEvents();
        }
        
        private void HookEvents()
        {
            this.Display.Load += Init;
            this.Display.RenderFrame += RenderFrame;

            this.Display.KeyDown += (o, e) =>
            {
                switch (e.Key)
                {
                    case Key.BackSpace:
                        if (this.textString.Length > 0)
                            this.textString = this.textString.Substring(0, this.textString.Length - 1);
                        break;

                    case Key.Enter:
                        this.textString += '\n';
                        break;

                    case Key.Left:
                        this.alignedText.Align = Align.Left;
                        break;

                    case Key.Right:
                        this.alignedText.Align = Align.Right;
                        break;

                    case Key.Up:
                        this.alignedText.Align = Align.Center;
                        break;

                    case Key.Minus:
                        this.alignedText.FontSize -= 1f;
                        break;

                    case Key.Plus:
                        this.alignedText.FontSize += 1f;
                        break;
                }

                this.FontMeshBuilder.Build(this.alignedText, this.textString);
            };

            this.Display.KeyPress += (o, e) =>
            {
                this.textString += e.KeyChar;
                this.FontMeshBuilder.Build(this.alignedText, this.textString);
            };
        }

        private void Init(object sender, EventArgs e)
        {
            this.Shader = new StaticShader();

            this.FontLoader = new FontLoader();
            this.TextureLoader = new TextureLoader();
            this.FontMeshBuilder = new FontMeshBuilder();

            //this.Font = this.FontLoader.Load(this.TextureLoader, "../../../res/Montserrat_high.fnt", "../../../res/Montserrat_high.png");
            this.Font = this.FontLoader.Load(this.TextureLoader, "../../../res/Montserrat_sdf.fnt", "../../../res/Montserrat_sdf.png");



            //this.fontEntity = new Text(this.Font, new Vector3(10, this.Display.Height/2, 0), 100);
            //this.FontMeshBuilder.Build(this.fontEntity.Mesh, "ahoj uwu nuzzles");

            this.alignedText = new AlignedText(this.Font, new Vector3(0, this.Display.Height, 0), this.Display.Width, 5f, Align.Center, 100);

            this.textString = "hahahahahahahahahahaha\n\nlol";
            this.FontMeshBuilder.Build(this.alignedText, textString);



            this.projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(90), (float)this.Display.Width / (float)this.Display.Height, 0.1f, 1000.0f);
            this.projection = Matrix4.CreateOrthographicOffCenter(0, this.Display.Width, 0, this.Display.Height, -1.0f, 100.0f);
        }

        public void Start()
        {
            this.Display.Start();
        }

        private void RenderFrame(object sender, OpenTK.FrameEventArgs e)
        {
            if (!this.alignedText.Mesh.Buffered)
                this.alignedText.Mesh.BufferMesh();

            this.Display.Clear();

            this.Shader.Start();
            this.Shader.LoadProjection(this.projection);

            Matrix4 model = Matrix4.Identity;
            model *= Matrix4.CreateScale(this.alignedText.FontSize);
            model *= Matrix4.CreateTranslation(this.alignedText.Position);

            this.Shader.LoadModelMatrix(model);
            this.Shader.LoadTextData(this.alignedText);

            GL.BindVertexArray(this.alignedText.Mesh.Id);

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, (int)this.alignedText.Mesh.Font.Atlas.Id);
            GL.DrawArrays(PrimitiveType.Quads, 0, this.alignedText.Mesh.Count);

            GL.BindTexture(TextureTarget.Texture2D, 0);

            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);

            GL.BindVertexArray(0);

            this.Shader.Stop();

            this.Display.SwapBuffers();
        }
    }
}
