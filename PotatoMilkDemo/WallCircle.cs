using PotatoMilk;
using PotatoMilk.Components;
using PotatoMilk.ConsumerInterfaces;
using SFML.System;
using System;
using Transform = PotatoMilk.Components.Transform;

namespace PotatoMilkDemo
{
    class WallCircle : GameObject, IUpdatable
    {
        private Transform transform;
        private QuadRenderer renderer;
        private bool mousePressed = false;
        private CircleCollider collider;
        private CollisionCounter ctr;

        public override void Start()
        {
            transform = AddComponent<Transform>();
            transform.Pos = new Vector2f(100f, 200f);
            renderer = AddComponent<QuadRenderer>();

            renderer.Size = new Vector2f(32f, 32f);
            renderer.Texture = Storage.texture;
            renderer.TextureSize = new Vector2f(32f, 32f);
            renderer.TextureTopLeft = new Vector2f(0f, 0f);

            collider = AddComponent<CircleCollider>();
            collider.Radius = 16f;

            collider.CollisionEnter += (a, b) => renderer.TextureTopLeft = new Vector2f(32f, 0f);
            collider.CollisionExit += (a, b) => renderer.TextureTopLeft = new Vector2f(0f, 0f);

            ctr = new CollisionCounter(collider, nameof(WallCircle));
        }

        private int framec = 0;
        public void Update()
        {
            framec++;
            transform.Pos = new Vector2f((float)Math.Sin(framec / 30f) * 25f + 100f, 200f);
        }
    }
}
