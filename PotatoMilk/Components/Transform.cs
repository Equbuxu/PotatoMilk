using SFML.System;
using System;

namespace PotatoMilk.Components
{
    public class Transform : IComponent, IStateful
    {
        private Vector2f pos;
        public Vector2f Pos
        {
            get => pos;
            set
            {
                pos = value;
                StateUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        public GameObject GameObject { get; private set; }

        public event EventHandler StateUpdated;

        public void Initialize(GameObject parent)
        {
            if (GameObject != null)
                throw new Exception("Already initialized");
            GameObject = parent;
        }
    }
}
