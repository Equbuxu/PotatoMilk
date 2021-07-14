using PotatoMilk;
using PotatoMilk.Components;
using PotatoMilk.ConsumerInterfaces;
using SFML.System;
using SFML.Window;
using Transform = PotatoMilk.Components.Transform;

namespace PotatoMilkDemo
{
    class PlayerSquare : GameObject, IKeyboardConsumer, IUpdatable
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
            transform = AddComponent<Transform>();
            transform.Pos = new(300f, 200f);
            renderer = AddComponent<QuadRenderer>();
            renderer.Size = size;
            renderer.Texture = Storage.texture2;
            renderer.TextureSize = new(32f, 32f);

            collider = AddComponent<ConvexPolygonCollider>();
            collider.Vertices = new() { new(), new(size.X, 0), size, new(0, size.Y) };
            collider.CollisionEnter += OnCollision;
            collider.CollisionStay += OnCollision;
        }

        private void OnCollision(object sender, Collision e)
        {
            transform.Pos += e.CalculatePushOutVector();
            /*
            Vector2f newPos = transform.Pos;
            transform.Pos = prevPos;

            int deltaX = (int)Math.Abs(newPos.X - prevPos.X);
            int deltaY = (int)Math.Abs(newPos.Y - prevPos.Y);

            int dirX = Math.Sign(newPos.X - prevPos.X);
            int dirY = Math.Sign(newPos.Y - prevPos.Y);

            for (int i = 0; i < deltaX; i++)
            {
                transform.Pos += new Vector2f(dirX, 0);
                if (Manager.Collisions.IsColliding(collider))
                {
                    transform.Pos -= new Vector2f(dirX, 0);
                    break;
                }
            }
            for (int i = 0; i < deltaY; i++)
            {
                transform.Pos += new Vector2f(0, dirY);
                if (Manager.Collisions.IsColliding(collider))
                {
                    transform.Pos -= new Vector2f(0, dirY);
                    break;
                }
            }*/
        }

        public void Update()
        {
            prevPos = transform.Pos;
            Vector2f newPos = transform.Pos;
            if (up)
                newPos += new Vector2f(0f, -2f);
            if (down)
                newPos += new Vector2f(0f, 2f);
            if (left)
                newPos += new Vector2f(-2f, 0f);
            if (right)
                newPos += new Vector2f(2f, 0f);
            transform.Pos = newPos;
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
