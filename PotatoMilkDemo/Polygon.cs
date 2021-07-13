using PotatoMilk;
using PotatoMilk.Components;
using PotatoMilk.ConsumerInterfaces;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Collections.Generic;
using Transform = PotatoMilk.Components.Transform;

namespace PotatoMilkDemo
{
    class Polygon : GameObject, IKeyboardConsumer
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
                Manager.Instantiate<Polygon>();
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
}
