using PotatoMilk.Helpers;
using SFML.System;
using System;

namespace PotatoMilk.Components
{
    public class CircleCollider : IComponent, ICollider
    {
        public GameObject GameObject { get; private set; }
        private Transform transform;
        public Vector2f Pos => transform.Pos;

        public float Radius { get; set; } = 32f;

        public event EventHandler<Collision> CollisionEnter;
        public event EventHandler<Collision> CollisionStay;
        public event EventHandler<Collision> CollisionExit;

        public Vector2f GetSupportPoint(Vector2f direction)
        {
            return direction.Norm() * Radius + Pos + new Vector2f(Radius, Radius);
        }

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
        }

        public void InvokeCollisionEnter(Collision collision) => CollisionEnter?.Invoke(this, collision);
        public void InvokeCollisionStay(Collision collision) => CollisionStay?.Invoke(this, collision);
        public void InvokeCollisionExit(Collision collision) => CollisionExit?.Invoke(this, collision);
    }
}
