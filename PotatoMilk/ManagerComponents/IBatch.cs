using SFML.Graphics;

namespace PotatoMilk.ManagerComponents
{
    internal interface IBatch<T>
    {
        public VertexArray VertexArray { get; }
        public RenderStates RenderStates { get; }
        void Add(T renderer);
        void Remove(T renderer);
        bool Contains(T renderer);
        void UpdatePosition(T renderer);
    }
}
