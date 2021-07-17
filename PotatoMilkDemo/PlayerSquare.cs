using PotatoMilk;
using PotatoMilk.Components;
using PotatoMilk.ConsumerInterfaces;
using PotatoMilk.Helpers;
using SFML.System;
using SFML.Window;
using System.Diagnostics;
using Transform = PotatoMilk.Components.Transform;

namespace PotatoMilkDemo
{
    [ObjectName("player_square")]
    class PlayerSquare : ObjectBehavior, IKeyboardConsumer, IUpdatable
    {
        private bool up = false;
        private bool down = false;
        private bool left = false;
        private bool right = false;
        private Transform transform;
        private Vector2f prevPos;
        private QuadRenderer renderer;
        private ConvexPolygonCollider collider;
        private Vector2f size = new(16f, 16f);
        public override void Start()
        {
            transform = GameObject.GetComponent<Transform>();
            renderer = GameObject.GetComponent<QuadRenderer>();
            collider = GameObject.GetComponent<ConvexPolygonCollider>();
            collider.CollisionEnter += OnCollision;
            collider.CollisionStay += OnCollision;
            Debug.Write(ComponentHelper.TryGetDataValue<string>(data, "test", "failed"));
        }

        private void OnCollision(object sender, Collision e)
        {
            transform.Position += e.CalculatePushOutVector();
        }

        public void Update()
        {
            prevPos = transform.Position;
            Vector2f newPos = transform.Position;
            if (up)
                newPos += new Vector2f(0f, -2f);
            if (down)
                newPos += new Vector2f(0f, 2f);
            if (left)
                newPos += new Vector2f(-2f, 0f);
            if (right)
                newPos += new Vector2f(2f, 0f);
            transform.Position = newPos;
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
