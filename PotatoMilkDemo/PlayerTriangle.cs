using PotatoMilk;
using PotatoMilk.Components;
using PotatoMilk.ConsumerInterfaces;
using SFML.System;
using SFML.Window;

namespace PotatoMilkDemo
{
    [ObjectName("player_triangle")]
    class PlayerTriangle : ObjectBehavior, IMouseMoveConsumer, IUpdatable
    {
        private Transform transform;
        private QuadRenderer renderer;
        private CollisionCounter ctr;
        private Vector2f prevPos;
        private Vector2f mousePos;
        private bool colliding = false;
        public override void Start()
        {
            transform = GameObject.GetComponent<Transform>();
            renderer = GameObject.GetComponent<QuadRenderer>();
            var collider = GameObject.GetComponent<ConvexPolygonCollider>();

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
