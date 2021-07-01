using SFML.Graphics;
using SFML.System;
using System;

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
                return (transform.Pos,
                        new Vector2f(transform.Pos.X, transform.Pos.Y + Size.Y),
                        transform.Pos + Size,
                        new Vector2f(transform.Pos.X + Size.X, transform.Pos.Y));
            }
        }

        public GameObject GameObject { get; private set; }


        public void Initialize(GameObject parent)
        {
            if (GameObject != null)
                throw new Exception("Already initialized");
            try
            {
                transform = parent.GetComponent<Transform>();
            }
            catch (Exception e)
            {
                throw new Exception("QuadRenderer depends on Transform", e);
            }
            GameObject = parent;
            transform.StateUpdated += (sender, args) =>
            {
                StateUpdated?.Invoke(this, args);
            };
        }
    }
}
