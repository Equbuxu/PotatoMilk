using PotatoMilk.Components;
using SFML.Graphics;
using System;
using System.Collections.Generic;

namespace PotatoMilk.ManagerComponents
{
    internal class DrawingManager
    {
        private QuadBatchingManager quadBatchingManager;
        private PolygonBatchingManager polygonBatchingManager;
        private List<Camera> cameras = new();

        public DrawingManager(QuadBatchingManager quadBatchingManager, PolygonBatchingManager polygonBatchingManager)
        {
            this.quadBatchingManager = quadBatchingManager;
            this.polygonBatchingManager = polygonBatchingManager;
        }

        internal void TrackComponent(IComponent component)
        {
            if (component is Camera camera)
            {
                cameras.Add(camera);
                camera.StateUpdated += UpdateCameraOrder;
                UpdateCameraOrder(null, null);
            }
        }

        internal void UntrackComponent(IComponent component)
        {
            if (component is Camera camera)
            {
                cameras.Remove(camera);
                camera.StateUpdated -= UpdateCameraOrder;
            }
        }
        private void UpdateCameraOrder(object sender, EventArgs args)
        {
            cameras.Sort((a, b) => a.RenderPriority - b.RenderPriority);
        }

        internal void Draw(RenderWindow window)
        {
            foreach (Camera camera in cameras)
            {
                window.SetView(camera.SFMLView);
                if (camera.DoClear)
                    window.Clear(camera.ClearColor);
                quadBatchingManager.Draw(window);
                polygonBatchingManager.Draw(window);
            }
            window.Display();
        }
    }
}
