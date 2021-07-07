using PotatoMilk;
using PotatoMilk.Components;
using PotatoMilk.ConsumerInterfaces;
using PotatoMilk.Helpers;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using Transform = PotatoMilk.Components.Transform;

namespace PotatoMilkDemo
{
    static class Storage
    {
        public static Texture texture;
        public static Texture texture2;
        public static void Load()
        {
            texture = new Texture("textures.png");
            texture2 = new Texture("textures2.png");
        }
    }

    class CollisionCounter
    {
        private int collisions = 0;
        private string name;
        public CollisionCounter(ICollider collider, string name)
        {
            this.name = name;
            collider.CollisionEnter += Collider_CollisionEnter;
            collider.CollisionStay += Collider_CollisionStay;
            collider.CollisionExit += Collider_CollisionExit;
        }

        private void Collider_CollisionExit(object sender, Collision e)
        {
            Console.WriteLine($"{name} collision EXIT");
            collisions = 0;
        }

        private void Collider_CollisionStay(object sender, Collision e)
        {
            Console.WriteLine($"{name} collides #{collisions}");
            collisions++;
        }

        private void Collider_CollisionEnter(object sender, Collision e)
        {
            Console.WriteLine($"{name} collision ENTER");
        }
    }

    class Ball : GameObject, IUpdatable
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

            ctr = new CollisionCounter(collider, "Ball");
        }

        private int framec = 0;
        public void Update()
        {
            framec++;
            transform.Pos = new Vector2f((float)Math.Sin(framec / 30f) * 25f + 100f, 200f);
        }
    }

    class BlueTriangle : GameObject, IUpdatable
    {
        private Transform transform;
        private QuadRenderer renderer;
        private CollisionCounter ctr;
        private Vector2f prevPos;
        private bool colliding = false;
        public override void Start()
        {
            transform = AddComponent<Transform>();
            transform.Pos = new Vector2f(10f, 0f);
            renderer = AddComponent<QuadRenderer>();
            renderer.Texture = Storage.texture;
            renderer.TextureTopLeft = new Vector2f(0f, 32f);
            renderer.TextureSize = new Vector2f(32f, 32f);
            renderer.Size = new Vector2f(32f, 32f);
            var collider = AddComponent<ConvexPolygonCollider>();
            collider.Vertices = new List<Vector2f> { new Vector2f(0f, 0f), new Vector2f(32f, 32f), new Vector2f(0f, 32f) };

            collider.CollisionEnter += OnCollision;
            collider.CollisionExit += OnCollisionExit;

            ctr = new CollisionCounter(collider, "BlueTriangle");
        }

        private void OnCollisionExit(object sender, Collision e)
        {
            renderer.TextureTopLeft = new Vector2f(0f, 32f);
            colliding = false;
        }

        private void OnCollision(object sender, Collision e)
        {
            renderer.TextureTopLeft = new Vector2f(0f, 64f);
            transform.Pos = e.ApproximateCollisionPosition(prevPos);
            colliding = true;
        }

        public void Update()
        {
            prevPos = transform.Pos;
            if (colliding)
                return;
            if (Mouse.IsButtonPressed(Mouse.Button.Right))
            {
                transform.Pos += ((Vector2f)Mouse.GetPosition() - transform.Pos) * 0.1f;
            }
        }

    }

    class Mesh : GameObject, IKeyboardConsumer
    {
        private PolygonRenderer rend;
        private Transform transform;
        public void KeyPressed(object sender, KeyEventArgs args)
        {

        }

        public void KeyReleased(object sender, KeyEventArgs args)
        {
            if (args.Code == Keyboard.Key.Numpad0)
                rend.Color = Color.Green;
            else if (args.Code == Keyboard.Key.Numpad1)
                transform.Pos = transform.Pos + new Vector2f(10f, 10f);
            else if (args.Code == Keyboard.Key.Numpad2)
                Manager.Destroy(this);
            else if (args.Code == Keyboard.Key.Numpad3)
                Manager.Instantiate<Mesh>();
        }

        public override void Start()
        {
            transform = AddComponent<Transform>();
            rend = AddComponent<PolygonRenderer>();
            rend.Color = Color.Blue;
            rend.Vertices = new List<Vector2f>()
            {
                new (0,0),
                new (10,10),
                new (20,0),
                new (30,20),
                new (20,40),
                new (0, 30),
            };
        }

        public void TextEntered(object sender, TextEventArgs args)
        {

        }
    }

    class Player : GameObject, IKeyboardConsumer, IUpdatable
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
            }
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

    class Wall : GameObject, IMouseButtonConsumer, IMouseMoveConsumer
    {
        public Vector2f startPos;
        public bool mouseHeld = false;

        private Transform transform;
        private ConvexPolygonCollider collider;
        private Vector2f prevPos;

        public void MouseButtonPressed(object sender, MouseButtonEventArgs args)
        {
            if (CollisionHelper.IsPointInside(collider, new Vector2f(args.X, args.Y)))
                mouseHeld = true;
        }

        public void MouseButtonReleased(object sender, MouseButtonEventArgs args)
        {
            mouseHeld = false;
        }

        public void MouseMoved(object sender, MouseMoveEventArgs e)
        {
            if (mouseHeld)
                transform.Pos = new(transform.Pos.X + e.X - prevPos.X, transform.Pos.Y + e.Y - prevPos.Y);
            prevPos = new Vector2f(e.X, e.Y);
        }

        public override void Start()
        {
            transform = AddComponent<Transform>();
            transform.Pos = startPos;
            var renderer = AddComponent<QuadRenderer>();
            renderer.Texture = Storage.texture2;
            renderer.Size = new(32f, 32f);
            renderer.TextureSize = new(32f, 32f);
            renderer.TextureTopLeft = new(32f, 0f);
            collider = AddComponent<ConvexPolygonCollider>();
            collider.Vertices = new() { new(0, 0), new(32, 0), new(32, 32), new(0, 32) };

            collider.CollisionEnter += (a, b) =>
            {
                if ((b.Other as IComponent).GameObject is BlueTriangle)
                    Manager.Destroy(this);
            };
        }
    }

    class GreenTriangle : GameObject
    {
        private QuadRenderer renderer;
        private CollisionCounter ctr;
        public override void Start()
        {
            var transform = AddComponent<Transform>();
            transform.Pos = new Vector2f(100f, 100f);
            renderer = AddComponent<QuadRenderer>();
            renderer.Texture = Storage.texture;
            renderer.TextureTopLeft = new Vector2f(32f, 32f);
            renderer.TextureSize = new Vector2f(32f, 32f);
            renderer.Size = new Vector2f(32f, 32f);
            var collider = AddComponent<ConvexPolygonCollider>();
            collider.Vertices = new List<Vector2f> { new Vector2f(0f, 0f), new Vector2f(32f, 0f), new Vector2f(0f, 32f) };

            collider.CollisionEnter += (a, b) => renderer.TextureTopLeft = new Vector2f(32f, 64f);
            collider.CollisionExit += (a, b) => renderer.TextureTopLeft = new Vector2f(32f, 32f);

            ctr = new CollisionCounter(collider, "GreenTriangle");
        }
    }

    class Program
    {
        static ObjectManager manager;
        static void Main(string[] args)
        {
            Storage.Load();

            RenderWindow window = new(new VideoMode(640, 450), "test", Styles.Default);
            window.Closed += (sender, args) => window.Close();
            window.SetVerticalSyncEnabled(true);
            //window.SetFramerateLimit(5);

            manager = new(window);
            manager.Instantiate<Ball>();
            manager.Instantiate<BlueTriangle>();
            manager.Instantiate<GreenTriangle>();
            manager.Instantiate<Player>();
            manager.Instantiate<Mesh>();

            Image map = new("map.png");
            for (uint i = 0; i < map.Size.X; i++)
            {
                for (uint j = 0; j < map.Size.Y; j++)
                {
                    if (map.GetPixel(i, j) != Color.Black)
                    {
                        var wall = manager.Instantiate<Wall>();
                        wall.startPos = new Vector2f(i * 32f, j * 32f);
                    }
                }
            }

            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear(new Color(128, 128, 128));
                manager.Draw(window);
                window.Display();
            }

        }

    }
}
