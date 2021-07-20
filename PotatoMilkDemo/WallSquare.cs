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
        private Camera heldOn;

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

            if (!mouseHeld)
                return;
            Vector2f transformed = Manager.MouseCollisionManager.ScreenToWorldCoordinates(heldOn, new Vector2i(e.X, e.Y));
            Transform.Position = new(GameObject.Transform.Position.X + transformed.X - prevPos.X, GameObject.Transform.Position.Y + transformed.Y - prevPos.Y);
            prevPos = transformed;
        }

        public override void Start()
        {
            collider = GetComponent<ConvexPolygonCollider>();

            collider.CollisionEnter += (a, b) =>
            {
                if ((b.Other as IComponent).GameObject.Type == "player_triangle")
                    GameObject.Manager.Destroy(GameObject);
            };
            collider.MouseButtonPress += (a, b) =>
            {
                heldOn = b.camera;
                mouseHeld = true;
                prevPos = b.worldPos;
            };
        }

        public void TextEntered(object sender, TextEventArgs args) { }
    }
}
