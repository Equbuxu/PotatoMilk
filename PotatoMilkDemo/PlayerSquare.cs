using PotatoMilk.Components;
using PotatoMilk.ConsumerInterfaces;
using PotatoMilk.Containers;
using PotatoMilk.Helpers;
using SFML.System;
using SFML.Window;
using System.Diagnostics;
using Transform = PotatoMilk.Components.Transform;

namespace PotatoMilkDemo
{
    [ComponentName("player_square_beh")]
    class PlayerSquare : ObjectBehavior, IKeyboardConsumer, IUpdatable
    {
        private bool up = false;
        private bool down = false;
        private bool left = false;
        private bool right = false;
        private Vector2f prevPos;
        private QuadRenderer renderer;
        private ConvexPolygonCollider collider;
        private Vector2f size = new(16f, 16f);
        private Camera camera;
        public override void Start()
        {
            renderer = GetComponent<QuadRenderer>();
            collider = GetComponent<ConvexPolygonCollider>();
            camera = GetComponent<Camera>();
            collider.CollisionEnter += OnCollision;
            collider.CollisionStay += OnCollision;
            Debug.Write(ComponentHelper.TryGetDataValue<string>(data, "test", "failed"));
        }

        private void OnCollision(object sender, Collision e)
        {
            Transform.Position += e.CalculatePushOutVector();
        }

        public void Update()
        {
            prevPos = Transform.Position;
            Vector2f newPos = Transform.Position;
            if (up)
                newPos += new Vector2f(0f, -2f);
            if (down)
                newPos += new Vector2f(0f, 2f);
            if (left)
                newPos += new Vector2f(-2f, 0f);
            if (right)
                newPos += new Vector2f(2f, 0f);
            Transform.Position = newPos;
        }
        public void KeyPressed(object sender, KeyEventArgs args)
        {
            switch (args.Code)
            {
                case Keyboard.Key.W:
                    up = true;
                    break;
                case Keyboard.Key.A:
                    left = true;
                    break;
                case Keyboard.Key.S:
                    down = true;
                    break;
                case Keyboard.Key.D:
                    right = true;
                    break;
                case Keyboard.Key.Num1:
                    {
                        size = new(16f, 16f);
                        collider.Vertices = new() { new(), new(size.X, 0), size, new(0, size.Y) };
                        renderer.Size = size;
                        break;
                    }
                case Keyboard.Key.Num2:
                    {
                        size = new(32f, 32f);
                        collider.Vertices = new() { new(), new(size.X, 0), size, new(0, size.Y) };
                        renderer.Size = size;
                        break;
                    }
                case Keyboard.Key.Num3:
                    {
                        size = new(48f, 48f);
                        collider.Vertices = new() { new(), new(size.X, 0), size, new(0, size.Y) };
                        renderer.Size = size;
                        break;
                    }
                case Keyboard.Key.Numpad1:
                    camera.RenderPriority = -1;
                    break;
                case Keyboard.Key.Numpad2:
                    camera.RenderPriority = 1;
                    break;
                case Keyboard.Key.Numpad3:
                    collider.Enabled = !collider.Enabled;
                    break;
            }
        }

        public void KeyReleased(object sender, KeyEventArgs args)
        {
            switch (args.Code)
            {
                case Keyboard.Key.W:
                    up = false;
                    break;
                case Keyboard.Key.A:
                    left = false;
                    break;
                case Keyboard.Key.S:
                    down = false;
                    break;
                case Keyboard.Key.D:
                    right = false;
                    break;
            }
        }

        public void TextEntered(object sender, TextEventArgs args) { }

    }
}
