using PotatoMilk;
using PotatoMilk.Components;
using PotatoMilk.ConsumerInterfaces;
using SFML.System;
using SFML.Window;

namespace PotatoMilkDemo
{
    [ComponentName("player_triangle_beh")]
    class PlayerTriangle : ObjectBehavior, IMouseMoveConsumer, IUpdatable, IKeyboardConsumer
    {
        private QuadRenderer renderer;
        private CollisionCounter ctr;
        private Vector2f prevPos;
        private Vector2f mousePos;
        private bool colliding = false;
        public override void Start()
        {
            renderer = GetComponent<QuadRenderer>();
            var collider = GetComponent<ConvexPolygonCollider>();

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
            Transform.Position = e.ApproximateCollisionPosition(prevPos);
            colliding = true;
        }

        public void MouseMoved(object sender, MouseMoveEventArgs e)
        {
            mousePos = Manager.MouseCollisionManager.ScreenToWorldCoordinates(Manager.DrawingManager.ActiveCameras[^1], new Vector2i(e.X, e.Y));
        }

        public void Update()
        {
            prevPos = Transform.Position;
            if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
                Transform.Rotation += 0.1f;
            else if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
                Transform.Rotation -= 0.1f;
            if (colliding)
                return;
            if (Mouse.IsButtonPressed(Mouse.Button.Right))
            {
                Transform.Position += (mousePos - Transform.Position) * 0.1f;
            }
        }

        public void KeyPressed(object sender, KeyEventArgs args) { }

        public void KeyReleased(object sender, KeyEventArgs args)
        {
            if (args.Code == Keyboard.Key.Add)
                Transform.Scale = new Vector2f(Transform.Scale.X + 0.1f, Transform.Scale.Y + 0.1f);
            else if (args.Code == Keyboard.Key.Subtract)
                Transform.Scale = new Vector2f(Transform.Scale.X - 0.1f, Transform.Scale.Y - 0.1f);
        }

        public void TextEntered(object sender, TextEventArgs args) { }
    }
}
