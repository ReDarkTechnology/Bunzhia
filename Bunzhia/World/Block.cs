using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace Bunzhia.World
{
    public class Block
    {
        public Vector3 position;
        private Dictionary<Faces, FaceData> faces;
        private List<Vector2> dirtUv = new List<Vector2> {
            new (0, 1),
            new (1, 1),
            new (1, 0),
            new (0, 0)
        };

        Faces[] allFaces = { 
            Faces.Front,
            Faces.Right,
            Faces.Back,
            Faces.Left,
            Faces.Top,
            Faces.Bottom
        };

        public Block(Vector3 pos) 
        {
            position = pos;
            faces = new Dictionary<Faces, FaceData>();
            foreach(var face in allFaces)
                faces.Add(face, new FaceData() { vertices = AddTransformedVertices(FaceDataRaw.rawVertexData[face]), uv = dirtUv });
        }

        public List<Vector3> AddTransformedVertices(List<Vector3> vertices)
        {
            List<Vector3> transformed = new List<Vector3>();
            foreach(var vert in vertices)
                transformed.Add(vert + position);
            return transformed;
        }

        public FaceData GetFace(Faces face)
        {
            return faces[face];
        }
    }
}
