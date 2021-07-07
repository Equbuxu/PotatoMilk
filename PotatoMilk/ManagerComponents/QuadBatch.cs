using PotatoMilk.Components;
using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;

namespace PotatoMilk.ManagerComponents
{
    internal class QuadBatch : IBatch<QuadRenderer>
    {
        private Stack<uint> freePositions = new();
        private readonly Vertex emptyVertex = new(new Vector2f(0, 0), Color.Transparent);
        private Dictionary<QuadRenderer, uint> positions = new();
        public VertexArray VertexArray { get; private set; }
        public RenderStates RenderStates { get; }
        public Texture Texture { get; }
        public QuadBatch(Texture texture)
        {
            Texture = texture;
            var states = RenderStates.Default;
            states.Texture = texture;
            RenderStates = states;

            const int initQuadCount = 16;
            VertexArray = new(PrimitiveType.Quads, 4 * initQuadCount);
            for (uint i = 0; i < initQuadCount; i++)
            {
                freePositions.Push(i);
                ResetQuad(i);
            }
        }

        public void Add(QuadRenderer renderer)
        {

            if (freePositions.Count == 0)
                DoubleVertexArray();
            uint pos = freePositions.Pop();
            positions.Add(renderer, pos);
            UpdatePosition(renderer);
        }
        public void Remove(QuadRenderer renderer)
        {
            if (!positions.ContainsKey(renderer))
                throw new System.Exception("Renderer not in batch");
            uint pos = positions[renderer];
            positions.Remove(renderer);
            ResetQuad(pos);
            freePositions.Push(pos);
        }
        public bool Contains(QuadRenderer renderer)
        {
            return positions.ContainsKey(renderer);
        }
        public void UpdatePosition(QuadRenderer renderer)
        {
            uint index = positions[renderer];
            uint vertexPos = index * 4;
            var (p1, p2, p3, p4) = renderer.TransformedCorners;
            VertexArray[vertexPos] = new Vertex(p1, Color.White, renderer.TextureTopLeft);
            VertexArray[vertexPos + 1] = new Vertex(p2, Color.White, new Vector2f(renderer.TextureTopLeft.X, renderer.TextureTopLeft.Y + renderer.TextureSize.Y));
            VertexArray[vertexPos + 2] = new Vertex(p3, Color.White, renderer.TextureTopLeft + renderer.TextureSize);
            VertexArray[vertexPos + 3] = new Vertex(p4, Color.White, new Vector2f(renderer.TextureTopLeft.X + renderer.TextureSize.X, renderer.TextureTopLeft.Y));
        }

        private void ResetQuad(uint index)
        {
            uint pos = index * 4;
            VertexArray[pos] = emptyVertex;
            VertexArray[pos + 1] = emptyVertex;
            VertexArray[pos + 2] = emptyVertex;
            VertexArray[pos + 3] = emptyVertex;
        }

        private void DoubleVertexArray()
        {
            uint quadCount = VertexArray.VertexCount / 4;
            VertexArray.Resize(quadCount * 4 * 2);
            for (uint i = quadCount; i < quadCount * 2; i++)
            {
                freePositions.Push(i);
                ResetQuad(i);
            }
        }
    }
}
