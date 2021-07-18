using PotatoMilk.Components;
using PotatoMilk.ConsumerInterfaces;
using PotatoMilk.Helpers;
using SFML.System;
using SFML.Window;

namespace PotatoMilkDemo
{
    [ComponentName("wall_square_beh")]
    class WallSquare : ObjectBehavior, IMouseButtonConsumer, IMouseMoveConsumer
    {
        private bool mouseHeld = false;

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
                transform.Position = new(transform.Position.X + e.X - prevPos.X, transform.Position.Y + e.Y - prevPos.Y);
            prevPos = new Vector2f(e.X, e.Y);
        }

        public override void Start()
        {
            transform = GameObject.GetComponent<Transform>();
            collider = GameObject.GetComponent<ConvexPolygonCollider>();

            collider.CollisionEnter += (a, b) =>
            {
                if ((b.Other as IComponent).GameObject.Name == "player_triangle")
                    GameObject.Manager.Destroy(GameObject);
            };
        }
    }
}
