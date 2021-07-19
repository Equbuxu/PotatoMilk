using PotatoMilk.Components;
using PotatoMilk.Containers;
using PotatoMilk.Helpers;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace PotatoMilk.ManagerComponents
{
    internal class MouseCollisionManager
    {
        private HashSet<ICollider> activeColliders = new();
        private HashSet<ICollider> inactiveColliders = new();
        private HashSet<Camera> cameras = new();
        private RenderWindow window;

        public MouseCollisionManager(RenderWindow window)
        {
            this.window = window;
            window.MouseButtonPressed += OnMousePress;
            window.MouseButtonPressed += OnMouseRelease;
        }

        public void TrackComponent(IComponent component)
        {
            if (component is ICollider coll)
            {
                coll.StateUpdated += OnStateUpdate;
                if (coll.MouseHitEnabled)
                    ActivateCollider(coll);
                else
                    DeactivateCollider(coll);
            }
            else if (component is Camera cam)
                cameras.Add(cam);
        }

        public void UntrackComponent(IComponent component)
        {
            if (component is ICollider coll)
            {
                coll.StateUpdated -= OnStateUpdate;
                DeactivateCollider(coll);
            }
            else if (component is Camera cam)
                cameras.Remove(cam);
        }

        private void ActivateCollider(ICollider collider)
        {
            inactiveColliders.Remove(collider);
            activeColliders.Add(collider);
        }

        private void DeactivateCollider(ICollider collider)
        {
            activeColliders.Remove(collider);
            inactiveColliders.Add(collider);
        }

        private void OnStateUpdate(object sender, EventArgs args)
        {
            var coll = (ICollider)sender;
            if (coll.MouseHitEnabled)
                ActivateCollider(coll);
            else
                DeactivateCollider(coll);
        }

        private void OnMousePress(object sender, MouseButtonEventArgs args)
        {
            var hit = FindHitColliders(new Vector2i(args.X, args.Y));
            foreach (var coll in hit)
                coll.Key.InvokeMouseButtonPress(new WorldMouseButtonEventArgs(coll.Value, args.Button));
        }

        private void OnMouseRelease(object sender, MouseButtonEventArgs args)
        {
            var hit = FindHitColliders(new Vector2i(args.X, args.Y));
            foreach (var coll in hit)
                coll.Key.InvokeMouseButtonRelease(new WorldMouseButtonEventArgs(coll.Value, args.Button));
        }

        private Dictionary<ICollider, Vector2f> FindHitColliders(Vector2i screenPos)
        {
            Dictionary<ICollider, Vector2f> colliders = new();
            foreach (var camera in cameras)
            {
                if (screenPos.X < window.Size.X * camera.ScreenViewportPos.X ||
                    screenPos.Y < window.Size.Y * camera.ScreenViewportPos.Y ||
                    screenPos.X > window.Size.X * (camera.ScreenViewportPos.X + camera.ScreenViewportSize.X) ||
                    screenPos.Y > window.Size.Y * (camera.ScreenViewportPos.Y + camera.ScreenViewportSize.Y))
                    continue;
                Vector2f transformedPos = window.MapPixelToCoords(screenPos, camera.SFMLView);

                foreach (ICollider collider in activeColliders)
                {
                    if (CollisionHelper.IsPointInside(collider, transformedPos))
                    {
                        if (!colliders.ContainsKey(collider))
                            colliders.Add(collider, transformedPos);
                    }
                }
            }
            return colliders;
        }
    }
}
