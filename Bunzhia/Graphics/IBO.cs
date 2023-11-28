using System.Collections.Generic;

using OpenTK.Graphics.OpenGL4;

namespace Bunzhia.Graphics
{
    internal class IBO
    {
        public int Id { get; private set; }
        public IBO(List<uint> data)
        {
            Id = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Id);
            GL.BufferData(BufferTarget.ElementArrayBuffer, data.Count * sizeof(uint), data.ToArray(), BufferUsageHint.StaticDraw);
        }

        public void Bind() => GL.BindBuffer(BufferTarget.ElementArrayBuffer, Id);
        public void Unbind() => GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        public void Delete() => GL.DeleteBuffer(Id);
    }
}
