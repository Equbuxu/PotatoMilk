using PotatoMilk.Helpers;
using SFML.System;
using System;
using System.Collections.Generic;

namespace PotatoMilk.Components
{
    public class CircleCollider : IComponent, ICollider
    {
        public GameObject GameObject { get; private set; }
        private Transform transform;
        public Vector2f Position => transform.Position;

        public float Radius { get; set; }

        public event EventHandler<Collision> CollisionEnter;
        public event EventHandler<Collision> CollisionStay;
        public event EventHandler<Collision> CollisionExit;

        public Vector2f GetSupportPoint(Vector2f direction)
        {
            return direction.Norm() * Radius + Position + new Vector2f(Radius, Radius);
        }

        public void Initialize(GameObject container, Dictionary<string, object> data)
        {
            if (GameObject != null)
                throw new Exception("Already initialized");
            transform = ComponentHelper.TryGetComponent<Transform>(container);
            GameObject = container;

            Radius = ComponentHelper.TryGetDataValue(data, "radius", 32f);
        }

        public void InvokeCollisionEnter(Collision collision) => CollisionEnter?.Invoke(this, collision);
        public void InvokeCollisionStay(Collision collision) => CollisionStay?.Invoke(this, collision);
        public void InvokeCollisionExit(Collision collision) => CollisionExit?.Invoke(this, collision);
    }
}
