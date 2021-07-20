using PotatoMilk.Components;
using SFML.System;

namespace PotatoMilkDemo
{

    [ComponentName("wall_triangle_beh")]
    class WallTriangle : ObjectBehavior
    {
        private CollisionCounter ctr;
        public override void Start()
        {
            var renderer = GetComponent<QuadRenderer>();
            var collider = GetComponent<ConvexPolygonCollider>();

            collider.CollisionEnter += (a, b) => renderer.TextureTopLeft = new Vector2f(32f, 64f);
            collider.CollisionExit += (a, b) => renderer.TextureTopLeft = new Vector2f(32f, 32f);

            ctr = new CollisionCounter(collider, nameof(WallTriangle));
        }
    }
}
