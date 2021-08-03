using PotatoMilk.Helpers;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PotatoMilk.Components
{
    [ComponentName("polygon_renderer")]
    public class PolygonRenderer : IComponent, IStateful
    {
        public GameObject GameObject { get; private set; }
        public string TypeName { get; private set; }
        public event EventHandler StateUpdated;

        private Transform transform;
        private List<Vector2f> vertices;
        public List<Vector2f> Vertices
        {
            set
            {
                if (value == null)
                    throw new Exception("Value was null");
                vertices = value.Select(a => a).ToList();
                TriangulateVertices(null, EventArgs.Empty);
            }
        }

        private List<(Vector2f, Vector2f, Vector2f)> triangles = new();
        internal IReadOnlyList<(Vector2f, Vector2f, Vector2f)> Triangles => triangles;

        private Color color;
        public Color Color
        {
            get => color;
            set
            {
                color = value;
                StateUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Initialize(GameObject container, Dictionary<string, object> data, string typeName)
        {
            if (GameObject != null)
                throw new Exception("Already initialized");
            TypeName = typeName;

            transform = ComponentHelper.TryGetComponent<Transform>(container);
            GameObject = container;
            transform.StateUpdated += TriangulateVertices;

            Vertices = ComponentHelper.TryGetDataValue(data, "vertices", new List<Vector2f>());
            Color = ComponentHelper.TryGetDataValue(data, "color", Color.Red);
        }

        private void TriangulateVertices(object sender, EventArgs args)
        {
            LinkedList<Vector2f> transformed = new();
            for (int i = 0; i < vertices.Count; i++)
            {
                transformed.AddLast(transform.TransformPoint(vertices[i]));
            }

            triangles.Clear();
            while (transformed.Count > 2)
            {
                var current = transformed.First.Next;
                LinkedListNode<Vector2f> convex = null;
                while (current != transformed.Last)
                {
                    var fromPrev = (current.Value - current.Previous.Value);
                    if (new Vector2f(-fromPrev.Y, fromPrev.X).Dot(current.Value - current.Next.Value) < 0 &&
                        !TriagleContainsAnyPoint(current.Previous.Value, current.Value, current.Next.Value, transformed))
                    {
                        convex = current;
                        break;
                    }
                    current = current.Next;
                }
                if (convex == null)
                    throw new Exception("Vertices are not in CW (assuming Y pointing downwards) order");
                triangles.Add((convex.Previous.Value, convex.Value, convex.Next.Value));
                transformed.Remove(convex);
            }
            StateUpdated?.Invoke(this, EventArgs.Empty);
        }

        private bool TriagleContainsAnyPoint(Vector2f a, Vector2f b, Vector2f c, LinkedList<Vector2f> points)
        {
            foreach (var point in points)
            {
                if (PointIsToTheLeft(a, b, point) && PointIsToTheLeft(b, c, point) && PointIsToTheLeft(c, a, point))
                    return true;
            }
            return false;
            bool PointIsToTheLeft(Vector2f a, Vector2f b, Vector2f point) => (b - a).TurnLeft90().Dot(point - a) > 0;
        }
    }
}
