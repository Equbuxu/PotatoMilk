using PotatoMilk.Components;
using PotatoMilk.Helpers;
using SFML.System;
using System;
using System.Collections.Generic;

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

            public event EventHandler<Collision> CollisionStay;
            public event EventHandler<Collision> CollisionExit;
            public event EventHandler<Collision> CollisionEnter;

            public Vector2f GetSupportPoint(Vector2f direction) => decorated.GetSupportPoint(direction) + Offset;
            public void InvokeCollisionEnter(Collision collision) { }
            public void InvokeCollisionExit(Collision collision) { }
            public void InvokeCollisionStay(Collision collision) { }
        }
        public ICollider Self { get; }
        public ICollider Other { get; }
        private bool exit;
        private (Vector2f, Vector2f, Vector2f) gjkTriangle;
        private ICollider gjkObjA;
        private ICollider gjkObjB;
        public Collision(ICollider other, ICollider self, bool inv, (Vector2f, Vector2f, Vector2f) gjkTriangle)
        {
            exit = false;
            Other = other;
            Self = self;
            this.gjkTriangle = gjkTriangle;
            if (!inv)
            {
                gjkObjA = other;
                gjkObjB = self;
            }
            else
            {
                gjkObjA = self;
                gjkObjB = other;
            }
        }
        public Collision(ICollider other, ICollider self)
        {
            exit = true;
            Other = other;
            Self = self;
            gjkTriangle = (new(), new(), new());
            gjkObjA = null;
            gjkObjB = null;
        }

        //TODO move this somewhere if it's even needed
        public Vector2f ApproximateCollisionPosition(Vector2f prevPos)
        {
            if (exit)
                throw new Exception("Objects no longer collide");
            Vector2f front = new Vector2f(0, 0);
            Vector2f back = prevPos - Self.Pos;
            ColliderDecorator collider = new ColliderDecorator(Self);
            for (int i = 0; i < 10; i++)
            {
                Vector2f testPos = (back + front) / 2;
                collider.Offset = testPos;
                if (CollisionHelper.CheckPairCollision(collider, Other).HasValue)
                    front = testPos;
                else
                    back = testPos;
            }
            return Self.Pos + back;
        }

        public Vector2f CalculatePushOutVector()
        {
            if (exit)
                throw new Exception("Objects no longer collide");
            List<Vector2f> verticesCW = new();
            if ((gjkTriangle.Item2 - gjkTriangle.Item1).TurnLeft90().Dot(gjkTriangle.Item2 - gjkTriangle.Item3) > 0)
            {
                verticesCW.Add(gjkTriangle.Item1);
                verticesCW.Add(gjkTriangle.Item2);
                verticesCW.Add(gjkTriangle.Item3);
            }
            else
            {
                verticesCW.Add(gjkTriangle.Item1);
                verticesCW.Add(gjkTriangle.Item3);
                verticesCW.Add(gjkTriangle.Item2);
            }
            int FindClosestEdgeAndPoint(out Vector2f closest)
            {
                float min = float.MaxValue;
                int minIndex = -1;
                Vector2f minPoint = new();
                for (int i = 0; i < verticesCW.Count; i++)
                {
                    var a = verticesCW[i];
                    var b = verticesCW[(i + 1) % verticesCW.Count];
                    Vector2f closestPoint = (b - a).Norm() * (-a).Dot((b - a).Norm()) + a;
                    float dist = closestPoint.Length();
                    if (dist < min)
                    {
                        min = dist;
                        minIndex = i;
                        minPoint = closestPoint;
                    }
                }
                closest = minPoint;
                return minIndex;
            }
            Vector2f prevClosestPoint = new Vector2f(float.PositiveInfinity, float.PositiveInfinity);
            while (true)
            {
                int closestEdge = FindClosestEdgeAndPoint(out Vector2f closestPoint);
                if ((prevClosestPoint - closestPoint).LengthSquared() < 0.0001f)
                    return Self == gjkObjA ? -closestPoint : closestPoint;
                prevClosestPoint = closestPoint;
                Vector2f dir = (verticesCW[(closestEdge + 1) % verticesCW.Count] - verticesCW[closestEdge]).TurnLeft90();
                Vector2f newSupport = gjkObjA.GetSupportPoint(dir) - gjkObjB.GetSupportPoint(-dir);
                verticesCW.Insert(closestEdge + 1, newSupport);
            }

        }
    }
}
