using PotatoMilk.Components;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;

namespace PotatoMilk.ManagerComponents
{
    class PolygonBatch : IBatch<PolygonRenderer>
    {
        public VertexArray VertexArray { get; private set; }
        public RenderStates RenderStates { get; private set; }

        private Stack<uint> freePositions = new();
        private Dictionary<PolygonRenderer, List<uint>> positions = new();

        private const int initTriangleCount = 10;
        private readonly Vertex emptyVertex = new(new Vector2f(0, 0), Color.Transparent);

        public PolygonBatch()
        {
            VertexArray = new VertexArray(PrimitiveType.Triangles, initTriangleCount * 3);
            RenderStates = RenderStates.Default;
            for (uint i = 0; i < initTriangleCount; i++)
                freePositions.Push(i);
        }

        public void Add(PolygonRenderer renderer)
        {
            var triangles = renderer.Triangles;
            if (freePositions.Count < triangles.Count)
                DoubleVertexArray();
            List<uint> occupiedSpaces = new();
            foreach (var triangle in triangles)
                occupiedSpaces.Add(AddTriangle(triangle, renderer.Color));
            positions.Add(renderer, occupiedSpaces);
        }

        private uint AddTriangle((Vector2f, Vector2f, Vector2f) triangle, Color color)
        {
            uint pos = freePositions.Pop();
            uint vertexPos = pos * 3;
            VertexArray[vertexPos] = new Vertex(triangle.Item1, color);
            VertexArray[vertexPos + 1] = new Vertex(triangle.Item2, color);
            VertexArray[vertexPos + 2] = new Vertex(triangle.Item3, color);
            return pos;
        }

        public bool Contains(PolygonRenderer renderer)
        {
            return positions.ContainsKey(renderer);
        }

        public void Remove(PolygonRenderer renderer)
        {
            if (!positions.ContainsKey(renderer))
                throw new Exception("Renderer not in batch");
            List<uint> occupiedSpaces = positions[renderer];
            positions.Remove(renderer);
            ResetTriangles(occupiedSpaces);
            occupiedSpaces.ForEach(a => freePositions.Push(a));
        }

        public void UpdatePosition(PolygonRenderer renderer)
        {
            Remove(renderer);
            Add(renderer);
        }

        private void ResetTriangles(List<uint> positions)
        {
            foreach (var index in positions)
            {
                uint pos = index * 3;
                VertexArray[pos] = emptyVertex;
                VertexArray[pos + 1] = emptyVertex;
                VertexArray[pos + 2] = emptyVertex;
            }
        }

        private void DoubleVertexArray()
        {
            uint triangleCount = VertexArray.VertexCount / 3;
            VertexArray.Resize(triangleCount * 3 * 2);
            for (uint i = triangleCount; i < triangleCount * 2; i++)
            {
                freePositions.Push(i);
            }
        }
    }
}
