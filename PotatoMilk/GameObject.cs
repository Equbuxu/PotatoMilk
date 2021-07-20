using PotatoMilk.Components;
using System;
using System.Collections.Generic;

namespace PotatoMilk
{
    public sealed class GameObject
    {
        private ObjectManager manager;
        public ObjectManager Manager
        {
            get => manager;
            set
            {
                if (manager is null)
                    manager = value;
                else
                    throw new Exception("Manager already set");
            }
        }


        private Dictionary<string, IComponent> namedComponents = new();
        private List<IComponent> components = new();
        public IReadOnlyList<IComponent> Components => components.AsReadOnly();
        public bool Persistent { get; set; }
        public string Name { get; set; }

        public Transform Transform { get; private set; }

        public T AddComponent<T>(Dictionary<string, object> data = null)
            where T : IComponent, new()
        {
            T component = new();
            string name = typeof(T).Name;
            AppendNewComponent(component, name, data);
            Manager?.TrackComponent(component);
            return component;
        }

        internal void AddComponentNoTracking(Type type, IComponent preconstructed, Dictionary<string, object> data)
        {
            string name = type.Name;
            AppendNewComponent(preconstructed, name, data);
        }

        internal void AddComponentNoTracking<T>(Dictionary<string, object> data = null)
            where T : IComponent, new()
        {
            T component = new();
            string name = typeof(T).Name;
            AppendNewComponent(component, name, data);
        }

        public T GetComponent<T>()
            where T : IComponent
        {
            string name = typeof(T).Name;
            if (!namedComponents.ContainsKey(name))
                throw new Exception("The object does not have the component \"" + name + "\"");
            return (T)namedComponents[name];
        }

        public bool HasComponent(string name)
        {
            return namedComponents.ContainsKey(name);
        }

        private void AppendNewComponent(IComponent component, string name, Dictionary<string, object> data)
        {
            if (namedComponents.ContainsKey(name))
                throw new Exception("The object already has the component \"" + name + "\"");
            namedComponents.Add(name, component);
            components.Add(component);
            component.Initialize(this, data);

            if (component is Transform transform)
                Transform = transform;
        }
    }
}
