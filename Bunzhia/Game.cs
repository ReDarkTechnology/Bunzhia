using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Bunzhia
{
    public class Game : GameWindow
    {
        #region Variables
        // Public

        // Private
        int width, height;
        float[] vertices =
        {
            0f, 0.5f, 0f,       // Top vertex
            -0.5f, -0.5f, 0f,   // Bottom left vertex
           0.5f, -0.5f, 0f      // Bottom right vertex
        };

        // Render pipeline
        int vao;
        int shaderProgram;

        #endregion

        #region Initialization
        public Game(int width, int height) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            this.width = width;
            this.height = height;

            // Center the window on monitor
            CenterWindow(new Vector2i(width, height));
        }
        #endregion

        #region Window Events
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);

            width = e.Width;
            height = e.Height;
        }
        #endregion

        #region Load Events
        protected override void OnLoad()
        {
            base.OnLoad();

            // Start creating the VAO
            vao = GL.GenVertexArray();

            // Create the VBO Buffer
            int vbo = GL.GenBuffer();

            // Start using the VBO and draw the buffer data
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            {
                GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            }

            // Bind the VAO
            GL.BindVertexArray(vao);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

            // Enable VAO Slot 0
            GL.EnableVertexArrayAttrib(vao, 0);

            // Unbinding the vbo and vao
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);

            // Create the shader program
            shaderProgram = GL.CreateProgram();

            // Create the actual shaders
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, LoadShaderSource("Default.vert"));
            GL.CompileShader(vertexShader);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, LoadShaderSource("Default.frag"));
            GL.CompileShader(fragmentShader);

            // Put the shaders into the program
            GL.AttachShader(shaderProgram, vertexShader);
            GL.AttachShader(shaderProgram, fragmentShader);

            // Link the program to the GL Context
            GL.LinkProgram(shaderProgram);

            // Free the memory by deleting the shader
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            GL.DeleteVertexArray(vao);
            GL.DeleteProgram(shaderProgram);
        }
        #endregion

        #region Updates
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(0.6f, 0.3f, 1f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            // Draw triangle
            GL.UseProgram(shaderProgram);
            GL.BindVertexArray(vao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            Context.SwapBuffers();

            base.OnRenderFrame(args);
        }
        #endregion

        #region Utility
        // Function to load a text file and return its contents as a string
        public static string LoadShaderSource(string filePath)
        {
            string path = "../../../Shaders/" + filePath;
            if (File.Exists(path))
                return File.ReadAllText(path);
            return null;
        }
        #endregion
    }
}
