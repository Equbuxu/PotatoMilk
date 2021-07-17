using PotatoMilk;
using PotatoMilk.Components;
using PotatoMilk.ConsumerInterfaces;
using SFML.System;
using SFML.Window;
using System.Collections.Generic;
using Transform = PotatoMilk.Components.Transform;

namespace PotatoMilkDemo
{
    class PlayerTriangle : GameObject, IMouseMoveConsumer, IUpdatable
    {
        private Transform transform;
        private QuadRenderer renderer;
        private CollisionCounter ctr;
        private Vector2f prevPos;
        private Vector2f mousePos;
        private bool colliding = false;
        public override void Start()
        {
            transform = AddComponent<Transform>();
            transform.Position = new Vector2f(10f, 0f);
            renderer = AddComponent<QuadRenderer>();
            renderer.Texture = Storage.texture;
            renderer.TextureTopLeft = new Vector2f(0f, 32f);
            renderer.TextureSize = new Vector2f(32f, 32f);
            renderer.Size = new Vector2f(32f, 32f);
            var collider = AddComponent<ConvexPolygonCollider>();
            collider.Vertices = new List<Vector2f> { new Vector2f(0f, 0f), new Vector2f(32f, 32f), new Vector2f(0f, 32f) };

            collider.CollisionEnter += OnCollision;
            collider.CollisionExit += OnCollisionExit;

            ctr = new CollisionCounter(collider, nameof(PlayerTriangle));
        }

        private void OnCollisionExit(object sender, Collision e)
        {
            renderer.TextureTopLeft = new Vector2f(0f, 32f);
            colliding = false;
        }

        private void OnCollision(object sender, Collision e)
        {
            renderer.TextureTopLeft = new Vector2f(0f, 64f);
            transform.Position = e.ApproximateCollisionPosition(prevPos);
            colliding = true;
        }

        public void MouseMoved(object sender, MouseMoveEventArgs e)
        {
            mousePos = new Vector2f(e.X, e.Y);
        }

        public void Update()
        {
            prevPos = transform.Position;
            if (colliding)
                return;
            if (Mouse.IsButtonPressed(Mouse.Button.Right))
            {
                transform.Position += (mousePos - transform.Position) * 0.1f;
            }
        }
    }
}
