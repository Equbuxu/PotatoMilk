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

        public GameObject GameObject { get; private set; }

        public event EventHandler StateUpdated;

        public void Initialize(GameObject container, Dictionary<string, object> data)
        {
            if (GameObject != null)
                throw new Exception("Already initialized");
            GameObject = container;
            Position = ComponentHelper.TryGetDataValue(data, "position", new Vector2f());
        }
    }
}
