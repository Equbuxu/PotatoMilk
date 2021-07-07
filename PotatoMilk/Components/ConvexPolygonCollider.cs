using PotatoMilk.Helpers;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PotatoMilk.Components
{
    public class ConvexPolygonCollider : IComponent, ICollider
    {
        private Transform transform;
        private List<Vector2f> transformedVertices = new();
        private List<Vector2f> vertices = new() { new Vector2f() };

        public event EventHandler<Collision> CollisionEnter;
        public event EventHandler<Collision> CollisionStay;
        public event EventHandler<Collision> CollisionExit;

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
        public GameObject GameObject { get; private set; }

        public Vector2f Pos => transform.Pos;

        public void Initialize(GameObject parent)
        {
            if (GameObject != null)
                throw new Exception("Already initialized");
            try
            {
                transform = parent.GetComponent<Transform>();
            }
            catch (Exception e)
            {
                throw new Exception(nameof(ConvexPolygonCollider) + " depends on Transform", e);
            }
            GameObject = parent;
            transform.StateUpdated += RecalculateVertices;
            RecalculateVertices(null, EventArgs.Empty);
        }

        public void InvokeCollisionEnter(Collision collision) => CollisionEnter?.Invoke(this, collision);
        public void InvokeCollisionStay(Collision collision) => CollisionStay?.Invoke(this, collision);
        public void InvokeCollisionExit(Collision collision) => CollisionExit?.Invoke(this, collision);

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
                transformedVertices[i] = vertices[i] + transform.Pos;
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
