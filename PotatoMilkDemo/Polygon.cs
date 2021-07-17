using PotatoMilk;
using PotatoMilk.Components;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Collections.Generic;
using Transform = PotatoMilk.Components.Transform;

namespace PotatoMilkDemo
{
    class Polygon : GameObject
    {
        private PolygonRenderer rend;
        private Transform transform;
        public override void Start()
        {
            transform = AddComponent<Transform>();
            transform.Position = new(200, 100);
            rend = AddComponent<PolygonRenderer>();
            rend.Color = Color.Blue;
            var vert = new List<Vector2f>()
            {
                new (-19,-33),
                new (56,-35),
                new (93,12),
                new (-11,59),
                new (-98,37),
                new (-80, -9),
            };
            rend.Vertices = vert;
            var collider = AddComponent<ConvexPolygonCollider>();
            collider.Vertices = vert;
        }

        public void TextEntered(object sender, TextEventArgs args)
        {

        }
    }
}
