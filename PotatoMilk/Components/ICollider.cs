using PotatoMilk.Containers;
using SFML.System;
using System;

namespace PotatoMilk.Components
{
    public interface ICollider : IStateful
    {
        Vector2f Position { get; }
        Vector2f GetSupportPoint(Vector2f direction);
        bool Enabled { get; }
        bool MouseHitEnabled { get; }
        event EventHandler<Collision> CollisionEnter;
        event EventHandler<Collision> CollisionStay;
        event EventHandler<Collision> CollisionExit;
        event EventHandler<WorldMouseButtonEventArgs> MouseButtonPress;
        event EventHandler<WorldMouseButtonEventArgs> MouseButtonRelease;
        void InvokeCollisionEnter(Collision collision);
        void InvokeCollisionStay(Collision collision);
        void InvokeCollisionExit(Collision collision);
        void InvokeMouseButtonPress(WorldMouseButtonEventArgs args);
        void InvokeMouseButtonRelease(WorldMouseButtonEventArgs args);
    }
}
