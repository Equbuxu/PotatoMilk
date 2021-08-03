using PotatoMilk.Containers;
using PotatoMilk.Helpers;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PotatoMilk.Components
{
    [ComponentName("convex_polygon_collider")]
    public class ConvexPolygonCollider : IComponent, ICollider
    {
        private Transform transform;
        private List<Vector2f> transformedVertices = new();
        private List<Vector2f> vertices;

        public event EventHandler<Collision> CollisionEnter;
        public event EventHandler<Collision> CollisionStay;
        public event EventHandler<Collision> CollisionExit;
        public event EventHandler StateUpdated;
        public event EventHandler<WorldMouseButtonEventArgs> MouseButtonPress;
        public event EventHandler<WorldMouseButtonEventArgs> MouseButtonRelease;

        public List<Vector2f> Vertices
        {
            set
            {
                if (value == null)
                    throw new Exception("Value was null");
                if (value.Count < 1)
                    throw new Exception($"A {nameof(ConvexPolygonCollider)} must have at least one point");
                vertices = value.Select(a => a).ToList(); ;
                RecalculateVertices(null, EventArgs.Empty);
            }
        }

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

        public string TypeName { get; private set; }
        public GameObject GameObject { get; private set; }

        public Vector2f Position => transform.Position;

        public void Initialize(GameObject container, Dictionary<string, object> data, string typeName)
        {
            if (GameObject != null)
                throw new Exception("Already initialized");
            TypeName = typeName;
            transform = ComponentHelper.TryGetComponent<Transform>(container);
            Vertices = ComponentHelper.TryGetDataValue(data, "vertices", new List<Vector2f>() { new Vector2f() });
            Enabled = ComponentHelper.TryGetDataValue(data, "enabled", true);
            MouseHitEnabled = ComponentHelper.TryGetDataValue(data, "mouse_hit_enabled", false);
            GameObject = container;
            transform.StateUpdated += RecalculateVertices;

            RecalculateVertices(null, EventArgs.Empty);
        }

        public void InvokeCollisionEnter(Collision collision) => CollisionEnter?.Invoke(this, collision);
        public void InvokeCollisionStay(Collision collision) => CollisionStay?.Invoke(this, collision);
        public void InvokeCollisionExit(Collision collision) => CollisionExit?.Invoke(this, collision);
        public void InvokeMouseButtonPress(WorldMouseButtonEventArgs args) => MouseButtonPress?.Invoke(this, args);
        public void InvokeMouseButtonRelease(WorldMouseButtonEventArgs args) => MouseButtonRelease?.Invoke(this, args);

        public Vector2f GetSupportPoint(Vector2f direction)
        {
            float furthest = float.MinValue;
            Vector2f furthestVertex = transformedVertices[0];
            foreach (var v in transformedVertices)
            {
                float dist = v.Dot(direction);
                if (dist > furthest)
                {
                    furthest = dist;
                    furthestVertex = v;
                }
            }
            return furthestVertex;
        }
        private void RecalculateVertices(object sender, EventArgs args)
        {
            if (transformedVertices.Count != vertices.Count)
            {
                transformedVertices.Clear();
                transformedVertices.AddRange(Enumerable.Repeat(new Vector2f(), vertices.Count));
            }
            for (int i = 0; i < vertices.Count; i++)
            {
                transformedVertices[i] = transform.TransformPoint(vertices[i]);
            }
            for (int i = 0; i < transformedVertices.Count; i++)
            {
                Vector2f cur = transformedVertices[i];
                Vector2f prev = transformedVertices[i == 0 ? ^1 : i - 1];
                Vector2f next = transformedVertices[(i + 1) % transformedVertices.Count];
                var fromPrev = (cur - prev);
                if (new Vector2f(-fromPrev.Y, fromPrev.X).Dot(cur - next) > 0)
                    throw new Exception("Polygon is either not convex or is not laid out in CW direction (assuming Y points downwards)");
            }
        }

    }
}
