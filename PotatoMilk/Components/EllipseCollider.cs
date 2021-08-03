using PotatoMilk.Containers;
using PotatoMilk.Helpers;
using SFML.System;
using System;
using System.Collections.Generic;

namespace PotatoMilk.Components
{
    [ComponentName("ellipse_collider")]
    public class EllipseCollider : IComponent, ICollider
    {
        public GameObject GameObject { get; private set; }
        public string TypeName { get; private set; }
        private Transform transform;
        public Vector2f Position => transform.Position;

        public Vector2f Radii { get; set; }
        public float Rotation { get; set; }

        private bool enabled;
        public bool Enabled
        {
            get => enabled;
            set
            {
                enabled = value;
                StateUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        private bool mouseHitEnabled;
        public bool MouseHitEnabled
        {
            get => mouseHitEnabled;
            set
            {
                mouseHitEnabled = value;
                StateUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler<Collision> CollisionEnter;
        public event EventHandler<Collision> CollisionStay;
        public event EventHandler<Collision> CollisionExit;
        public event EventHandler StateUpdated;
        public event EventHandler<WorldMouseButtonEventArgs> MouseButtonPress;
        public event EventHandler<WorldMouseButtonEventArgs> MouseButtonRelease;

        public Vector2f GetSupportPoint(Vector2f direction)
        {
            Vector2f unrotate = direction.Rotate(-Rotation);
            Vector2f scaled = new Vector2f(unrotate.X * Radii.X, unrotate.Y * Radii.Y).Norm();
            Vector2f onEllipse = new Vector2f(scaled.X * Radii.X, scaled.Y * Radii.Y).Rotate(Rotation);
            return onEllipse + transform.Position;
        }

        public void Initialize(GameObject container, Dictionary<string, object> data, string typeName)
        {
            if (GameObject != null)
                throw new Exception("Already initialized");
            TypeName = typeName;
            transform = ComponentHelper.TryGetComponent<Transform>(container, nameof(EllipseCollider));
            GameObject = container;

            Radii = ComponentHelper.TryGetDataValue(data, "radii", new Vector2f(32f, 32f));
            Rotation = ComponentHelper.TryGetDataValue(data, "rotation", 0f);
            Enabled = ComponentHelper.TryGetDataValue(data, "enabled", true);
            MouseHitEnabled = ComponentHelper.TryGetDataValue(data, "mouse_hit_enabled", false);


            List<Vector2f> points = new();
            for (int i = 0; i < 16; i++)
            {
                float angle = i / 8f * (float)Math.PI;
                points.Add(GetSupportPoint(new Vector2f((float)Math.Cos(angle), (float)Math.Sin(angle))));
            }

        }

        public void InvokeCollisionEnter(Collision collision) => CollisionEnter?.Invoke(this, collision);
        public void InvokeCollisionStay(Collision collision) => CollisionStay?.Invoke(this, collision);
        public void InvokeCollisionExit(Collision collision) => CollisionExit?.Invoke(this, collision);
        public void InvokeMouseButtonPress(WorldMouseButtonEventArgs args) => MouseButtonPress?.Invoke(this, args);
        public void InvokeMouseButtonRelease(WorldMouseButtonEventArgs args) => MouseButtonRelease?.Invoke(this, args);
    }
}
