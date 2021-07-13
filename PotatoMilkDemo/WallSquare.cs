using PotatoMilk;
using PotatoMilk.Components;
using PotatoMilk.ConsumerInterfaces;
using PotatoMilk.Helpers;
using SFML.System;
using SFML.Window;
using Transform = PotatoMilk.Components.Transform;

namespace PotatoMilkDemo
{
    class WallSquare : GameObject, IMouseButtonConsumer, IMouseMoveConsumer
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
                if ((b.Other as IComponent).GameObject is PlayerTriangle)
                    Manager.Destroy(this);
            };
        }
    }
}
