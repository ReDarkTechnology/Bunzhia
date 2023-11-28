using System;

using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Bunzhia
{
    public class Camera
    {
        float _speed = 8f;
        float _screenWidth, _screenHeight;
        float _sensitivity = 15f;

        public Vector3 position;
        public float fieldOfView = 60f;

        // Basis
        Vector3 right = Vector3.UnitX;
        Vector3 up = Vector3.UnitY;
        Vector3 front = -Vector3.UnitZ;

        float pitch;
        float yaw = -90.0f;

        bool firstMove = true;
        public Vector2 lastPosition;

        public Camera(float width, float height, Vector3 position) 
        { 
            _screenWidth = width;
            _screenHeight = height;
            this.position = position;
        }

        private void UpdateVectors() 
        {
            if (pitch > 89f) pitch = 89f;
            if (pitch < -89f) pitch = -89f;

            front.X = MathF.Cos(MathHelper.DegreesToRadians(pitch)) * MathF.Cos(MathHelper.DegreesToRadians(yaw));
            front.Y = MathF.Sin(MathHelper.DegreesToRadians(pitch));
            front.Z = MathF.Cos(MathHelper.DegreesToRadians(pitch)) * MathF.Sin(MathHelper.DegreesToRadians(yaw));

            front = Vector3.Normalize(front);
            right = Vector3.Normalize(Vector3.Cross(front, Vector3.UnitY));
            up = Vector3.Normalize(Vector3.Cross(right, front));
        }

        public void InputController(KeyboardState input, MouseState mouse, FrameEventArgs e) 
        {
            Vector3 transform = Vector3.Zero;
            if (input.IsKeyDown(Keys.W)) transform = front;
            if (input.IsKeyDown(Keys.A)) transform += -right;
            if (input.IsKeyDown(Keys.S)) transform += -front;
            if (input.IsKeyDown(Keys.D)) transform += right;
            if (input.IsKeyDown(Keys.Space)) transform = new Vector3(0, 1, 0);
            if (input.IsKeyDown(Keys.LeftShift)) transform = new Vector3(0, -1, 0);

            position += transform * _speed * (float)e.Time;

            if(firstMove)
            {
                lastPosition = new Vector2(mouse.X, mouse.Y);
                firstMove = false;
            }
            else
            {
                var dx = mouse.X - lastPosition.X;
                var dy = mouse.Y - lastPosition.Y;
                lastPosition = new Vector2(mouse.X, mouse.Y);

                yaw += dx * _sensitivity * (float)e.Time;
                pitch -= dy * _sensitivity * (float)e.Time;
                UpdateVectors();
            }
        }

        public void Update(KeyboardState input, MouseState mouse, FrameEventArgs e)
        {
            InputController(input, mouse, e);
        }

        public Matrix4 GetViewMatrix() 
        {
            return Matrix4.LookAt(position, position + front, up);
        }

        public Matrix4 GetProjectionMatrix() 
        {
            return Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(fieldOfView),
                _screenWidth / _screenHeight, 
                0.1f, 100.0f
            );
        }
    }
}
