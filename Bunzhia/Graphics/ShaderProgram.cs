using System.IO;

using OpenTK.Graphics.OpenGL4;

namespace Bunzhia.Graphics
{
    internal class ShaderProgram
    {
        public int Id { get; private set; }
        public ShaderProgram(string vertexShaderPath, string fragmentShaderPath)
        {
            // Create the shader program
            Id = GL.CreateProgram();

            // Create the actual shaders
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, LoadShaderSource(vertexShaderPath));
            GL.CompileShader(vertexShader);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, LoadShaderSource(fragmentShaderPath));
            GL.CompileShader(fragmentShader);

            // Put the shaders into the program
            GL.AttachShader(Id, vertexShader);
            GL.AttachShader(Id, fragmentShader);

            // Link the program to the GL Context
            GL.LinkProgram(Id);

            // Free the memory by deleting the shader
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        public void Bind() => GL.UseProgram(Id);
        public void Unind() => GL.UseProgram(0);
        public void Delete() => GL.DeleteProgram(Id);

        // Function to load a text file and return its contents as a string
        public static string LoadShaderSource(string filePath)
        {
            string path = "../../../Shaders/" + filePath;
            if (File.Exists(path))
                return File.ReadAllText(path);
            return null;
        }
    }
}
