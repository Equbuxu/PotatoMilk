using System;
using System.Collections.Generic;

namespace PotatoMilk.Components
{
    public abstract class ObjectBehavior : IComponent
    {
        public GameObject GameObject { get; private set; }
        protected Dictionary<string, object> data;

        public void Initialize(GameObject container, Dictionary<string, object> data)
        {
            if (GameObject != null)
                throw new Exception("Already initialized");
            GameObject = container;
            this.data = data;
        }

        public abstract void Start();
    }
}
