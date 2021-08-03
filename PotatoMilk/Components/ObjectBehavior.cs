using System;
using System.Collections.Generic;

namespace PotatoMilk.Components
{
    public abstract class ObjectBehavior : IComponent
    {
        public string TypeName { get; private set; }
        public GameObject GameObject { get; private set; }
        protected Dictionary<string, object> data;

        public T AddComponent<T>(Dictionary<string, object> data = null)
            where T : IComponent, new()
        {
            return GameObject.AddComponent<T>(data);
        }

        public T GetComponent<T>()
            where T : IComponent
        {
            return GameObject.GetComponent<T>();
        }

        public Transform Transform => GameObject.Transform;
        public ObjectManager Manager => GameObject.Manager;

        public void Initialize(GameObject container, Dictionary<string, object> data, string typeName)
        {
            if (GameObject != null)
                throw new Exception("Already initialized");
            TypeName = typeName;
            GameObject = container;
            this.data = data;
        }

        public abstract void Start();
    }
}
