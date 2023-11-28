using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace Bunzhia.Graphics
{
    internal class Texture
    {
        public int Id { get; private set; }
        public Texture(string path)
        {
            Id = GL.GenTexture();

            // Activate texture in the unit
            GL.ActiveTexture(TextureUnit.Texture0);
            Bind();

            // Texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult dirtTexture = ImageResult.FromStream(File.OpenRead("../../../Textures/" + path), ColorComponents.RedGreenBlueAlpha);

            // Give image data to OpenGL
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, dirtTexture.Width, dirtTexture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, dirtTexture.Data);
            Unbind();
        }

        public void Bind() => GL.BindTexture(TextureTarget.Texture2D, Id);
        public void Unbind() => GL.BindTexture(TextureTarget.Texture2D, 0);
        public void Delete() => GL.DeleteTexture(Id);
    }
}
