using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;

namespace Bunzhia
{
    public class Camera
    {
        float _speed = 8f;
        float _screenWidth, _screenHeight;
        float _sensitivity = 100f;

        public Vector3 position;
        public float fieldOfView = 60f;

        // Basis
        Vector3 right = Vector3.UnitX;
        Vector3 up = Vector3.UnitY;
        Vector3 front = -Vector3.UnitZ;

        public Camera(float width, float height, Vector3 position) 
        { 
            _screenWidth = width;
            _screenHeight = height;
            this.position = position;
        }

        private void UpdateVectors() { }

        public void InputController(KeyboardState input, MouseState mouse, FrameEventArgs e) {}
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
