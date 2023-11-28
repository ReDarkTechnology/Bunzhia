using System;
using System.Collections.Generic;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace Bunzhia.Graphics
{
    internal class VBO
    {
        public int Id { get; private set; }
        public VBO(List<Vector3> data)
        {
            Id = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, Id);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Count * Vector3.SizeInBytes, data.ToArray(), BufferUsageHint.StaticDraw);
        }

        public VBO(List<Vector2> data)
        {
            Id = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, Id);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Count * Vector2.SizeInBytes, data.ToArray(), BufferUsageHint.StaticDraw);
        }

        public void Bind() => GL.BindBuffer(BufferTarget.ArrayBuffer, Id);
        public void Unbind() => GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        public void Delete() => GL.DeleteBuffer(Id);
    }
}
