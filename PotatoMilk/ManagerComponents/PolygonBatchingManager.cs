using PotatoMilk.Components;
using SFML.Graphics;
using System;

namespace PotatoMilk.ManagerComponents
{
    class PolygonBatchingManager
    {
        private PolygonBatch batch = new();
        public void TrackComponent(IComponent component)
        {
            if (component is PolygonRenderer rend)
            {
                batch.Add(rend);
                rend.StateUpdated += OnStateUpdate;
            }
        }

        private void OnStateUpdate(object sender, EventArgs args)
        {
            batch.UpdatePosition((PolygonRenderer)sender);
        }
        public void UntrackComponent(IComponent component)
        {
            if (component is PolygonRenderer rend)
            {
                rend.StateUpdated -= OnStateUpdate;
                batch.Remove(rend);
            }
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(batch.VertexArray, batch.RenderStates);
        }
    }
}
