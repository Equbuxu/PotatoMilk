using PotatoMilk.Helpers;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;

namespace PotatoMilk.Components
{
    public class QuadRenderer : IComponent, IStateful
    {
        private Transform transform;

        public event EventHandler StateUpdated;

        private Vector2f size;
        public Vector2f Size
        {
            get => size;
            set
            {
                size = value;
                StateUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        private Texture texture;
        public Texture Texture
        {
            get => texture;
            set
            {
                texture = value;
                StateUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        private Vector2f textureTopLeft;
        public Vector2f TextureTopLeft
        {
            get => textureTopLeft;
            set
            {
                textureTopLeft = value;
                StateUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        private Vector2f textureSize;
        public Vector2f TextureSize
        {
            get => textureSize;
            set
            {
                textureSize = value;
                StateUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        public (Vector2f, Vector2f, Vector2f, Vector2f) TransformedCorners
        {
            get
            {
                return (transform.Position,
                        new Vector2f(transform.Position.X, transform.Position.Y + Size.Y),
                        transform.Position + Size,
                        new Vector2f(transform.Position.X + Size.X, transform.Position.Y));
            }
        }

        public GameObject GameObject { get; private set; }


        public void Initialize(GameObject container, Dictionary<string, object> data)
        {
            if (GameObject != null)
                throw new Exception("Already initialized");
            transform = ComponentHelper.TryGetComponent<Transform>(container);
            GameObject = container;
            transform.StateUpdated += (sender, args) =>
            {
                StateUpdated?.Invoke(this, args);
            };

            Size = ComponentHelper.TryGetDataValue(data, "size", new Vector2f());
            Texture = ComponentHelper.TryGetDataValue<Texture>(data, "texture", null);
            TextureTopLeft = ComponentHelper.TryGetDataValue(data, "texture_top_left", new Vector2f());
            TextureSize = ComponentHelper.TryGetDataValue(data, "texture_size", new Vector2f());
        }
    }
}
