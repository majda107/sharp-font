using FontRenderer.Data;
using FontRenderer.Entities;
using FontRenderer.Meshes;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace FontRenderer.Builders
{
    class FontMeshBuilder
    {
        public void Build(FontMesh mesh, string text)
        {
            List<Vector2> vertexBuffer = new List<Vector2>();
            List<Vector2> textureCoordBuffer = new List<Vector2>();
            Vector2 cursor = Vector2.Zero;

            var font = mesh.Font;
            foreach (var character in text)
            {
                if(character == '\n')
                {
                    cursor.X = 0;
                    cursor.Y -= (float)font.LineHeight / (float)font.GivenSize;
                    continue;
                }

                var data = font.Data[character];

                //// WORKING WITHOUT SCALING, DON'T TOUCH
                ////vertexBuffer.Add(new Vector2(cursor + data.Offset.X, font.GivenSize - data.Offset.Y)); // top left
                ////vertexBuffer.Add(new Vector2(cursor + data.Offset.X, font.GivenSize - data.Offset.Y - data.Height)); // bottom left
                ////vertexBuffer.Add(new Vector2(cursor + data.Offset.X + data.Width, font.GivenSize - data.Offset.Y - data.Height)); // bottom right
                ////vertexBuffer.Add(new Vector2(cursor + data.Offset.X + data.Width, font.GivenSize - data.Offset.Y)); // top right


                //var localOffset = new Vector2(data.Offset.X / (float)font.GivenSize, data.Offset.Y / (float)font.GivenSize);
                //var localHeight = (float)data.Height / (float)font.GivenSize;
                //var localWidth = (float)data.Width / (float)font.GivenSize;

                //vertexBuffer.Add(new Vector2(cursor.X + localOffset.X, cursor.Y - localOffset.Y)); // top left
                //vertexBuffer.Add(new Vector2(cursor.X + localOffset.X, cursor.Y - localOffset.Y - localHeight)); // bottom left
                //vertexBuffer.Add(new Vector2(cursor.X + localOffset.X + localWidth, cursor.Y - localOffset.Y - localHeight)); // bottom right
                //vertexBuffer.Add(new Vector2(cursor.X + localOffset.X + localWidth, cursor.Y - localOffset.Y)); // top right




                //Vector2 texturePosition = new Vector2(data.Position.X / font.AtlasWidth, data.Position.Y / font.AtlasHeight);
                //float textureWidth = (float)data.Width / (float)font.AtlasWidth;
                //float textureHeight = (float)data.Height / (float)font.AtlasHeight;

                //textureCoordBuffer.Add(new Vector2(texturePosition.X, texturePosition.Y)); // top left
                //textureCoordBuffer.Add(new Vector2(texturePosition.X, texturePosition.Y + textureHeight)); // bottom left
                //textureCoordBuffer.Add(new Vector2(texturePosition.X + textureWidth, texturePosition.Y + textureHeight)); // bottom right
                //textureCoordBuffer.Add(new Vector2(texturePosition.X + textureWidth, texturePosition.Y)); // top right

                // VERTICES

                vertexBuffer.Add(new Vector2(cursor.X + data.LocalOffset.X, cursor.Y - data.LocalOffset.Y)); // top left
                vertexBuffer.Add(new Vector2(cursor.X + data.LocalOffset.X, cursor.Y - data.LocalOffset.Y - data.LocalHeight)); // bottom left
                vertexBuffer.Add(new Vector2(cursor.X + data.LocalOffset.X + data.LocalWidth, cursor.Y - data.LocalOffset.Y - data.LocalHeight)); // bottom right
                vertexBuffer.Add(new Vector2(cursor.X + data.LocalOffset.X + data.LocalWidth, cursor.Y - data.LocalOffset.Y)); // top right




                // TEXTURE COORDS

                Vector2 texturePosition = new Vector2(data.Position.X / font.AtlasWidth, data.Position.Y / font.AtlasHeight);
                float textureWidth = (float)data.LocalWidth * font.GivenSize / (float)font.AtlasWidth;
                float textureHeight = (float)data.LocalHeight * font.GivenSize / (float)font.AtlasHeight;

                textureCoordBuffer.Add(new Vector2(texturePosition.X, texturePosition.Y)); // top left
                textureCoordBuffer.Add(new Vector2(texturePosition.X, texturePosition.Y + textureHeight)); // bottom left
                textureCoordBuffer.Add(new Vector2(texturePosition.X + textureWidth, texturePosition.Y + textureHeight)); // bottom right
                textureCoordBuffer.Add(new Vector2(texturePosition.X + textureWidth, texturePosition.Y)); // top right

                //cursor.X += (float)data.Xadvance / (float)font.GivenSize;
                cursor.X += data.LocalXadvance;
            }

            float[] vertices = new float[vertexBuffer.Count * 2];
            float[] textureCoords = new float[textureCoordBuffer.Count * 2];

            int vertexIndex = 0;
            foreach (var vertex in vertexBuffer)
            {
                vertices[vertexIndex++] = vertex.X;
                vertices[vertexIndex++] = vertex.Y;
            }

            int textureCoordIndex = 0;
            foreach (var textureCoord in textureCoordBuffer)
            {
                textureCoords[textureCoordIndex++] = textureCoord.X;
                textureCoords[textureCoordIndex++] = textureCoord.Y;
            }

            vertexBuffer.Clear();
            textureCoordBuffer.Clear();

            mesh.UpdateCoords(vertices, textureCoords);
            //return new FontMesh(vertices, textureCoords, font);
        }

        public void Build(AlignedText alignedText, string text)
        {
            var mesh = alignedText.Mesh;
            var font = mesh.Font;

            var lineLength = alignedText.LineLength / (float)alignedText.FontSize;

            float lineSum = 0;
            string currentLine = String.Empty;

            List<Vector2> vertexBuffer = new List<Vector2>();
            List<Vector2> textureCoordBuffer = new List<Vector2>();
            Vector2 cursor = Vector2.Zero;

            for(int i = 0; i < text.Length; i++)
            {
                var character = text[i];

                var charLength = font.Data[character].LocalXadvance;

                if (lineSum + charLength < lineLength && character != '\n')
                {
                    lineSum += charLength;
                    currentLine += character;
                    if(i != text.Length -1)
                        continue;
                }

                switch (alignedText.Align)
                {
                    case Align.Right:
                        cursor.X = lineLength - lineSum;
                        break;

                    case Align.Center:
                        cursor.X = (lineLength - lineSum) / 2;
                        break;
                }

                this.BuildLine(currentLine, vertexBuffer, textureCoordBuffer, font, cursor);

                currentLine = String.Empty;
                lineSum = 0;

                cursor.Y -= (float)font.LineHeight / (float)font.GivenSize;
            }


            float[] vertices = new float[vertexBuffer.Count * 2];
            float[] textureCoords = new float[textureCoordBuffer.Count * 2];

            int vertexIndex = 0;
            foreach (var vertex in vertexBuffer)
            {
                vertices[vertexIndex++] = vertex.X;
                vertices[vertexIndex++] = vertex.Y;
            }

            int textureCoordIndex = 0;
            foreach (var textureCoord in textureCoordBuffer)
            {
                textureCoords[textureCoordIndex++] = textureCoord.X;
                textureCoords[textureCoordIndex++] = textureCoord.Y;
            }

            vertexBuffer.Clear();
            textureCoordBuffer.Clear();

            mesh.UpdateCoords(vertices, textureCoords);
        }

        private void BuildLine(string text, List<Vector2> vertexBuffer, List<Vector2> textureCoordBuffer, Font font, Vector2 cursor)
        {
            float padding = 5f;
            float localPadding = padding / (float)font.GivenSize;
            float texturePadding = padding / (float)font.AtlasWidth;

            foreach(var character in text)
            {
                var data = font.Data[character];

                // VERTICES

                vertexBuffer.Add(new Vector2(cursor.X + data.LocalOffset.X, cursor.Y - data.LocalOffset.Y)); // top left
                vertexBuffer.Add(new Vector2(cursor.X + data.LocalOffset.X, cursor.Y - data.LocalOffset.Y - data.LocalHeight)); // bottom left
                vertexBuffer.Add(new Vector2(cursor.X + data.LocalOffset.X + data.LocalWidth, cursor.Y - data.LocalOffset.Y - data.LocalHeight)); // bottom right
                vertexBuffer.Add(new Vector2(cursor.X + data.LocalOffset.X + data.LocalWidth, cursor.Y - data.LocalOffset.Y)); // top right




                // TEXTURE COORDS

                Vector2 texturePosition = new Vector2(data.Position.X / font.AtlasWidth, data.Position.Y / font.AtlasHeight);
                float textureWidth = (float)data.LocalWidth * font.GivenSize / (float)font.AtlasWidth;
                float textureHeight = (float)data.LocalHeight * font.GivenSize / (float)font.AtlasHeight;

                textureCoordBuffer.Add(new Vector2(texturePosition.X + texturePadding, texturePosition.Y + texturePadding)); // top left
                textureCoordBuffer.Add(new Vector2(texturePosition.X + texturePadding, texturePosition.Y + textureHeight - texturePadding)); // bottom left
                textureCoordBuffer.Add(new Vector2(texturePosition.X + textureWidth - texturePadding, texturePosition.Y + textureHeight - texturePadding)); // bottom right
                textureCoordBuffer.Add(new Vector2(texturePosition.X + textureWidth - texturePadding, texturePosition.Y + texturePadding)); // top right

                cursor.X += data.LocalXadvance;
            }    
        }
    }
}
