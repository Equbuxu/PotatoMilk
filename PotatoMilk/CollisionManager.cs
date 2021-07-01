using PotatoMilk.Components;
using System.Collections.Generic;

namespace PotatoMilk
{
    public class CollisionManager
    {
        private List<ICollider> colliders = new();
        private HashSet<(ICollider, ICollider)> activeCollisionPairs = new();
        private HashSet<(ICollider, ICollider)> detachedCollisions = new();

        public bool IsColliding(ICollider a)
        {
            for (int j = 0; j < colliders.Count; j++)
            {
                var b = colliders[j];
                if (a == b)
                    continue;
                if (CheckPairCollision(a, b))
                    return true;
            }
            return false;
        }

        internal CollisionManager() { }

        internal void TrackComponent(IComponent component)
        {
            if (component is ICollider collider)
                colliders.Add(collider);
        }
        internal void UntrackComponent(IComponent component)
        {
            if (component is ICollider collider)
            {
                colliders.Remove(collider);

                foreach (var pair in activeCollisionPairs)
                {
                    if (pair.Item1 == collider || pair.Item2 == collider)
                    {
                        detachedCollisions.Add(pair);
                    }
                }
            }
        }

        internal void CalculateCollisions()
        {
            for (int i = 0; i < colliders.Count - 1; i++)
            {
                for (int j = i + 1; j < colliders.Count; j++)
                {
                    var a = colliders[i];
                    var b = colliders[j];
                    var pair = (a, b);
                    bool colliding = CheckPairCollision(a, b);
                    if (activeCollisionPairs.Contains(pair))
                    {
                        if (colliding)
                        {
                            a.InvokeCollisionStay(new Collision(b, a));
                            b.InvokeCollisionStay(new Collision(a, b));
                        }
                        else
                        {
                            a.InvokeCollisionExit(new Collision(b, a));
                            b.InvokeCollisionExit(new Collision(a, b));
                            activeCollisionPairs.Remove(pair);
                        }
                    }
                    else if (colliding)
                    {
                        a.InvokeCollisionEnter(new Collision(b, a));
                        b.InvokeCollisionEnter(new Collision(a, b));
                        activeCollisionPairs.Add(pair);
                    }
                }
            }
            RemoveDetachedCollisions();
        }

        internal static bool CheckPairCollision(ICollider a, ICollider b)
        {
            var direction = b.Pos - a.Pos;
            var firstVertex = a.GetSupportPoint(direction) - b.GetSupportPoint(direction * -1);
            var secondVertex = a.GetSupportPoint(direction * -1) - b.GetSupportPoint(direction);
            var perp = (secondVertex - firstVertex).Perp(-firstVertex);
            var toCenterVertex = a.GetSupportPoint(perp) - b.GetSupportPoint(perp * -1);

            while (true)
            {
                var perp1 = (toCenterVertex - secondVertex).Perp(secondVertex - firstVertex);
                var perp2 = (toCenterVertex - firstVertex).Perp(firstVertex - secondVertex);

                bool inside1 = perp1.Dot(-secondVertex) < 0;
                bool inside2 = perp2.Dot(-firstVertex) < 0;

                if (inside1 && inside2)
                {
                    return true;
                }

                if (!inside1)
                {
                    firstVertex = toCenterVertex;
                    toCenterVertex = a.GetSupportPoint(perp1) - b.GetSupportPoint(perp1 * -1);

                    if (toCenterVertex.Dot(perp1) <= 0)
                        return false;
                }
                else
                {
                    secondVertex = toCenterVertex;
                    toCenterVertex = a.GetSupportPoint(perp2) - b.GetSupportPoint(perp2 * -1);

                    if (toCenterVertex.Dot(perp2) <= 0)
                        return false;
                }
            }
        }

        private void RemoveDetachedCollisions()
        {
            foreach (var pair in detachedCollisions)
            {
                pair.Item1.InvokeCollisionExit(new Collision(pair.Item2, pair.Item1));
                pair.Item2.InvokeCollisionExit(new Collision(pair.Item1, pair.Item2));
                activeCollisionPairs.Remove(pair);
            }
            detachedCollisions.Clear();
        }
    }
}
