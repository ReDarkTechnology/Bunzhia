using System.Collections.Generic;

using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

using Bunzhia.Graphics;
using Bunzhia.World;

namespace Bunzhia
{
    public class Game : GameWindow
    {
        #region Variables
        // Public
        public Chunk chunk;
        public ShaderProgram program;

        // Private
        int width, height;

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

            chunk = new Chunk(Vector3.Zero);
            program = new ShaderProgram("Default.vert", "Default.frag");

            GL.Enable(EnableCap.DepthTest);
            CursorState = CursorState.Grabbed;
        }

        protected override void OnUnload()
        {
            base.OnUnload();
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
            // Transformation Matrices
            Matrix4 model = Matrix4.Identity;
            Matrix4 view = camera.GetViewMatrix();
            Matrix4 projection = camera.GetProjectionMatrix();

            int modelLocation = GL.GetUniformLocation(program.Id, "model");
            int viewLocation = GL.GetUniformLocation(program.Id, "view");
            int projectionLocation = GL.GetUniformLocation(program.Id, "projection");

            GL.UniformMatrix4(modelLocation, true, ref model);
            GL.UniformMatrix4(viewLocation, true, ref view);
            GL.UniformMatrix4(projectionLocation, true, ref projection);

            chunk.Render(program);

            Context.SwapBuffers();

            base.OnRenderFrame(args);
        }
        #endregion

        #region Utility
        #endregion
    }
}
