using PotatoMilk.Components;
using PotatoMilk.ConsumerInterfaces;
using SFML.System;
using SFML.Window;

namespace PotatoMilkDemo
{
    [ComponentName("wall_square_beh")]
    class WallSquare : ObjectBehavior, IMouseButtonConsumer, IMouseMoveConsumer, IKeyboardConsumer
    {
        private bool mouseHeld = false;

        private ConvexPolygonCollider collider;
        private Vector2f prevPos;

        public void KeyPressed(object sender, KeyEventArgs args) { }

        public void KeyReleased(object sender, KeyEventArgs args)
        {
            if (args.Code == Keyboard.Key.Space)
                collider.MouseHitEnabled = !collider.MouseHitEnabled;
        }

        public void MouseButtonPressed(object sender, MouseButtonEventArgs args) { }

        public void MouseButtonReleased(object sender, MouseButtonEventArgs args)
        {
            mouseHeld = false;
        }

        public void MouseMoved(object sender, MouseMoveEventArgs e)
        {
            if (mouseHeld)
                GameObject.Transform.Position = new(GameObject.Transform.Position.X + e.X - prevPos.X, GameObject.Transform.Position.Y + e.Y - prevPos.Y);
            prevPos = new Vector2f(e.X, e.Y);
        }

        public override void Start()
        {
            collider = GameObject.GetComponent<ConvexPolygonCollider>();

            collider.CollisionEnter += (a, b) =>
            {
                if ((b.Other as IComponent).GameObject.Name == "player_triangle")
                    GameObject.Manager.Destroy(GameObject);
            };
            collider.MouseButtonPress += (a, b) => mouseHeld = true;
        }

        public void TextEntered(object sender, TextEventArgs args) { }
    }
}
