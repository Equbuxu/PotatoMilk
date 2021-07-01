using PotatoMilk.Components;
using SFML.System;
using System;

namespace PotatoMilk
{
    public struct Collision
    {
        class ColliderDecorator : ICollider
        {
            private ICollider decorated;
            public Vector2f Offset { get; set; }
            public ColliderDecorator(ICollider decorated)
            {
                this.decorated = decorated;
            }

            public Vector2f Pos => decorated.Pos + Offset;

            public event EventHandler<Collision> CollisionEnter;
            public event EventHandler<Collision> CollisionStay;
            public event EventHandler<Collision> CollisionExit;

            public Vector2f GetSupportPoint(Vector2f direction) => decorated.GetSupportPoint(direction) + Offset;
            public void InvokeCollisionEnter(Collision collision) { }
            public void InvokeCollisionExit(Collision collision) { }
            public void InvokeCollisionStay(Collision collision) { }
        }
        public ICollider Self { get; }
        public ICollider Other { get; }

        public Collision(ICollider other, ICollider self)
        {
            Other = other;
            Self = self;
        }

        //TODO move this somewhere if it's even needed
        public Vector2f ApproximateCollisionPosition(Vector2f prevPos)
        {
            Vector2f front = new Vector2f(0, 0);
            Vector2f back = prevPos - Self.Pos;
            ColliderDecorator collider = new ColliderDecorator(Self);
            for (int i = 0; i < 10; i++)
            {
                Vector2f testPos = (back + front) / 2;
                collider.Offset = testPos;
                if (CollisionManager.CheckPairCollision(collider, Other))
                    front = testPos;
                else
                    back = testPos;
            }
            return Self.Pos + back;
        }
    }
}
