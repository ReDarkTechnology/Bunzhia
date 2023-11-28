using System.Collections.Generic;

using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

using Bunzhia.Graphics;

namespace Bunzhia
{
    public class Game : GameWindow
    {
        #region Variables
        // Public

        // Private
        int width, height;

        List<Vector3> vertices = new List<Vector3>()
        {
            // Front face
            new Vector3(-0.5f, 0.5f, 0.5f), // topleft vert
            new Vector3(0.5f, 0.5f, 0.5f), // topright vert
            new Vector3(0.5f, -0.5f, 0.5f), // bottomright vert
            new Vector3(-0.5f, -0.5f, 0.5f), // bottomleft vert

            // Right face
            new Vector3(0.5f, 0.5f, 0.5f), // topleft vert
            new Vector3(0.5f, 0.5f, -0.5f), // topright vert
            new Vector3(0.5f, -0.5f, -0.5f), // bottomright vert
            new Vector3(0.5f, -0.5f, 0.5f), // bottomleft vert

            // Back face
            new Vector3(0.5f, 0.5f, -0.5f), // topleft vert
            new Vector3(-0.5f, 0.5f, -0.5f), // topright vert
            new Vector3(-0.5f, -0.5f, -0.5f), // bottomright vert
            new Vector3(0.5f, -0.5f, -0.5f), // bottomleft vert

            // Left face
            new Vector3(-0.5f, 0.5f, -0.5f), // topleft vert
            new Vector3(-0.5f, 0.5f, 0.5f), // topright vert
            new Vector3(-0.5f, -0.5f, 0.5f), // bottomright vert
            new Vector3(-0.5f, -0.5f, -0.5f), // bottomleft vert

            // Top face
            new Vector3(-0.5f, 0.5f, -0.5f), // topleft vert
            new Vector3(0.5f, 0.5f, -0.5f), // topright vert
            new Vector3(0.5f, 0.5f, 0.5f), // bottomright vert
            new Vector3(-0.5f, 0.5f, 0.5f), // bottomleft vert

            // Bottom face
            new Vector3(-0.5f, -0.5f, 0.5f), // topleft vert
            new Vector3(0.5f, -0.5f, 0.5f), // topright vert
            new Vector3(0.5f, -0.5f, -0.5f), // bottomright vert
            new Vector3(-0.5f, -0.5f, -0.5f), // bottomleft vert
        };

        List<Vector2> texCoords = new List<Vector2>()
        {
            new (0, 1),
            new (1, 1),
            new (1, 0),
            new (0, 0),

            new (0, 1),
            new (1, 1),
            new (1, 0),
            new (0, 0),

            new (0, 1),
            new (1, 1),
            new (1, 0),
            new (0, 0),

            new (0, 1),
            new (1, 1),
            new (1, 0),
            new (0, 0),

            new (0, 1),
            new (1, 1),
            new (1, 0),
            new (0, 0),

            new (0, 1),
            new (1, 1),
            new (1, 0),
            new (0, 0)
        };

        List<uint> indices = new List<uint>
        {
            0, 1, 2,
            2, 3, 0,

            4, 5, 6,
            6, 7, 4,

            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 19, 16,

            20, 21, 22,
            22, 23, 20
        };

        // Render pipeline
        VAO vao;
        IBO ibo;
        ShaderProgram program;
        Texture texture;

        // Transformation variables
        float yRot = 0f;
        Camera camera;
        #endregion

        #region Initialization
        public Game(int width, int height) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            this.width = width;
            this.height = height;

            // Center the window on monitor
            CenterWindow(new Vector2i(width, height));
            camera = new(width, height, Vector3.Zero);
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

            vao = new VAO();
            VBO vbo = new VBO(vertices);
            vao.LinkToVAO(0, 3, vbo);
            vbo.Unbind();

            VBO uvVBO = new VBO(texCoords);
            vao.LinkToVAO(1, 2, uvVBO);
            uvVBO.Unbind();

            ibo = new IBO(indices);
            program = new ShaderProgram("Default.vert", "Default.frag");
            texture = new Texture("Dirt.jpg");

            GL.Enable(EnableCap.DepthTest);
            CursorState = CursorState.Grabbed;
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            vao.Delete();
            ibo.Delete();
            texture.Delete();
            program.Delete();
        }
        #endregion

        #region Updates
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            MouseState mouse = MouseState;
            KeyboardState input = KeyboardState;

            base.OnUpdateFrame(args);
            yRot += 2 * (float)args.Time;
            camera?.Update(input, mouse, args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(0.6f, 0.3f, 1f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            program.Bind();
            vao.Bind();
            ibo.Bind();
            texture.Bind();

            // Transformation Matrices
            Matrix4 model = Matrix4.Identity;
            Matrix4 view = camera.GetViewMatrix();
            Matrix4 projection = camera.GetProjectionMatrix();

            model = Matrix4.CreateRotationY(yRot);
            model *= Matrix4.CreateTranslation(0, 0, -3f);

            int modelLocation = GL.GetUniformLocation(program.Id, "model");
            int viewLocation = GL.GetUniformLocation(program.Id, "view");
            int projectionLocation = GL.GetUniformLocation(program.Id, "projection");

            GL.UniformMatrix4(modelLocation, true, ref model);
            GL.UniformMatrix4(viewLocation, true, ref view);
            GL.UniformMatrix4(projectionLocation, true, ref projection);

            GL.DrawElements(PrimitiveType.Triangles, indices.Count, DrawElementsType.UnsignedInt, 0);

            Context.SwapBuffers();

            base.OnRenderFrame(args);
        }
        #endregion

        #region Utility
        #endregion
    }
}
