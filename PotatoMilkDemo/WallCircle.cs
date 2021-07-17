﻿using PotatoMilk.Components;
using PotatoMilk.ConsumerInterfaces;
using SFML.System;
using System;

namespace PotatoMilkDemo
{
    [ComponentName("wall_circle_beh")]
    class WallCircle : ObjectBehavior, IUpdatable
    {
        private Transform transform;
        private QuadRenderer renderer;
        private bool mousePressed = false;
        private CircleCollider collider;
        private CollisionCounter ctr;

        public override void Start()
        {
            transform = GameObject.GetComponent<Transform>();
            renderer = GameObject.GetComponent<QuadRenderer>();
            collider = GameObject.GetComponent<CircleCollider>();

            collider.CollisionEnter += (a, b) => renderer.TextureTopLeft = new Vector2f(32f, 0f);
            collider.CollisionExit += (a, b) => renderer.TextureTopLeft = new Vector2f(0f, 0f);

            ctr = new CollisionCounter(collider, nameof(WallCircle));
        }

        private int framec = 0;
        public void Update()
        {
            framec++;
            transform.Position = new Vector2f((float)Math.Sin(framec / 30f) * 25f + 100f, 200f);
        }
    }
}
