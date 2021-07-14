using PotatoMilk.Components;
using SFML.System;

namespace PotatoMilk.Helpers
{
    public class CollisionHelper
    {
        public static bool IsPointInside(ICollider a, Vector2f point)
        {
            var direction = a.Pos - point;
            var firstVertex = a.GetSupportPoint(direction) - point;
            var secondVertex = a.GetSupportPoint(direction * -1) - point;
            var perp = (secondVertex - firstVertex).Perp(-firstVertex);
            var toCenterVertex = a.GetSupportPoint(perp) - point;

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
                    toCenterVertex = a.GetSupportPoint(perp1) - point;

                    if (toCenterVertex.Dot(perp1) <= 0)
                        return false;
                }
                else
                {
                    secondVertex = toCenterVertex;
                    toCenterVertex = a.GetSupportPoint(perp2) - point;

                    if (toCenterVertex.Dot(perp2) <= 0)
                        return false;
                }
            }
        }

        internal static (Vector2f, Vector2f, Vector2f)? CheckPairCollision(ICollider a, ICollider b)
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
                    return (firstVertex, secondVertex, toCenterVertex);

                if (!inside1)
                {
                    firstVertex = toCenterVertex;
                    toCenterVertex = a.GetSupportPoint(perp1) - b.GetSupportPoint(perp1 * -1);

                    if (toCenterVertex.Dot(perp1) <= 0)
                        return null;
                }
                else
                {
                    secondVertex = toCenterVertex;
                    toCenterVertex = a.GetSupportPoint(perp2) - b.GetSupportPoint(perp2 * -1);

                    if (toCenterVertex.Dot(perp2) <= 0)
                        return null;
                }
            }
        }
    }
}
