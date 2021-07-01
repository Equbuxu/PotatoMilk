using SFML.System;
using System;

namespace PotatoMilk.Components
{
    public interface ICollider
    {
        Vector2f Pos { get; }
        Vector2f GetSupportPoint(Vector2f direction);
        event EventHandler<Collision> CollisionEnter;
        event EventHandler<Collision> CollisionStay;
        event EventHandler<Collision> CollisionExit;
        public void InvokeCollisionEnter(Collision collision);
        public void InvokeCollisionStay(Collision collision);
        public void InvokeCollisionExit(Collision collision);
    }
}
