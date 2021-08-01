using PotatoMilk.Helpers;
using SFML.System;
using System;
using System.Collections.Generic;

namespace PotatoMilk.Components
{
    public class Transform : IComponent, IStateful
    {
        private Vector2f position;
        public Vector2f Position
        {
            get => position;
            set
            {
                position = value;
                StateUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        private Vector2f scale;
        public Vector2f Scale
        {
            get => scale;
            set
            {
                scale = value;
                StateUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        private Vector2f origin;
        public Vector2f Origin
        {
            get => origin;
            set
            {
                origin = value;
                StateUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        private float rotation;
        public float Rotation
        {
            get => rotation;
            set
            {
                rotation = value;
                StateUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        public GameObject GameObject { get; private set; }

        public event EventHandler StateUpdated;

        public void Initialize(GameObject container, Dictionary<string, object> data)
        {
            if (GameObject != null)
                throw new Exception("Already initialized");
            GameObject = container;
            Position = ComponentHelper.TryGetDataValue(data, "position", new Vector2f());
            Scale = ComponentHelper.TryGetDataValue(data, "scale", new Vector2f(1f, 1f));
            Origin = ComponentHelper.TryGetDataValue(data, "origin", new Vector2f());
            Rotation = ComponentHelper.TryGetDataValue(data, "rotation", 0f);
        }

        internal Vector2f TransformPoint(Vector2f point)
        {
            Vector2f scaled = new Vector2f(point.X * scale.X, point.Y * scale.Y);
            Vector2f scaledOrigin = new Vector2f(origin.X * scale.X, origin.Y * scale.Y);
            return (scaled - scaledOrigin).Rotate(rotation) + position;
        }
    }
}
