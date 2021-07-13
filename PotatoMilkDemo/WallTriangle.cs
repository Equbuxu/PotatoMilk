using PotatoMilk;
using PotatoMilk.Components;
using SFML.System;
using System.Collections.Generic;
using Transform = PotatoMilk.Components.Transform;

namespace PotatoMilkDemo
{
    class WallTriangle : GameObject
    {
        public Vector2f startPos;
        private QuadRenderer renderer;
        private CollisionCounter ctr;
        public override void Start()
        {
            var transform = AddComponent<Transform>();
            transform.Pos = startPos;
            renderer = AddComponent<QuadRenderer>();
            renderer.Texture = Storage.texture;
            renderer.TextureTopLeft = new Vector2f(32f, 32f);
            renderer.TextureSize = new Vector2f(32f, 32f);
            renderer.Size = new Vector2f(32f, 32f);
            var collider = AddComponent<ConvexPolygonCollider>();
            collider.Vertices = new List<Vector2f> { new Vector2f(0f, 0f), new Vector2f(32f, 0f), new Vector2f(0f, 32f) };

            collider.CollisionEnter += (a, b) => renderer.TextureTopLeft = new Vector2f(32f, 64f);
            collider.CollisionExit += (a, b) => renderer.TextureTopLeft = new Vector2f(32f, 32f);

            ctr = new CollisionCounter(collider, nameof(WallTriangle));
        }
    }
}
