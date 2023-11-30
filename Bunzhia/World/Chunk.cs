using System;
using System.Collections.Generic;
using Bunzhia.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Bunzhia.World
{
    public class Chunk
    {
        private List<Vector3> chunkVerts = new ();
        private List<Vector2> chunkUVs = new ();
        private List<uint> chunkIndices = new ();

        const int SIZE = 16;
        const int HEIGHT = 32;
        public Vector3 position;

        private uint indexCount;

        VAO chunkVAO;
        VBO chunkVertexVBO;
        VBO chunkUvVBO;
        IBO chunkIBO;
        Texture texture;

        public Chunk(Vector3 pos)
        {
            position = pos;

            GenBlocks();
            BuildChunk();
        }

        /// <summary>
        /// Generate the data
        /// </summary>
        public void GenChunk()
        {

        }

        /// <summary>
        /// Generate the appropriate block faces based on the data
        /// </summary>
        public void GenBlocks()
        {
            for(int i = 0; i < 3; i++)
            {
                Block block = new Block(new Vector3(i, 0, 0));
                var frontFaceData = block.GetFace(Faces.Front);
                chunkVerts.AddRange(frontFaceData.vertices);
                chunkUVs.AddRange(frontFaceData.uv);
                AddIndices(1);
            }
        }

        public void AddIndices(int amount)
        {
            for(int i = 0; i < amount; i++)
            {
                chunkIndices.Add(indexCount);
                chunkIndices.Add(indexCount + 1);
                chunkIndices.Add(indexCount + 2);
                chunkIndices.Add(indexCount + 2);
                chunkIndices.Add(indexCount + 3);
                chunkIndices.Add(indexCount);

                indexCount += 4;
            }
        }

        /// <summary>
        /// Take data and process it for rendering
        /// </summary>
        public void BuildChunk()
        {
            chunkVAO = new VAO();
            chunkVAO.Bind();

            chunkVertexVBO = new VBO(chunkVerts);
            chunkVertexVBO.Bind();
            chunkVAO.LinkToVAO(0, 3, chunkVertexVBO);

            chunkUvVBO = new VBO(chunkUVs);
            chunkUvVBO.Bind();
            chunkVAO.LinkToVAO(1, 2, chunkVertexVBO);

            chunkIBO = new IBO(chunkIndices);
            texture = new Texture("Dirt.jpg");
        }

        /// <summary>
        /// Drawing the chunk
        /// </summary>
        /// <param name="program">The target shader program to render it with</param>
        public void Render(ShaderProgram program)
        {
            program.Bind();
            chunkVAO.Bind();
            chunkIBO.Bind();
            texture.Bind();

            GL.DrawElements(PrimitiveType.Triangles, chunkIndices.Count, DrawElementsType.UnsignedInt, 0);
        }

        public void Delete()
        {
            chunkVAO.Delete();
            chunkVertexVBO.Delete();
            chunkUvVBO.Delete();
            chunkIBO.Delete();
            texture.Delete();
        }
    }
}
