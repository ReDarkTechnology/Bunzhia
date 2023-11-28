using OpenTK.Graphics.OpenGL4;

namespace Bunzhia.Graphics
{
    internal class VAO
    {
        public int Id { get; private set; }
        public VAO() 
        {
            Id = GL.GenVertexArray();
            GL.BindVertexArray(Id);
        }

        public void LinkToVAO(int location, int size, VBO vbo)
        {
            Bind();
            vbo.Bind();
            GL.VertexAttribPointer(location, size, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(location);
            Unbind();
        }

        public void Bind() => GL.BindVertexArray(Id);
        public void Unbind() => GL.BindVertexArray(0);
        public void Delete() => GL.DeleteVertexArray(Id);
    }
}
