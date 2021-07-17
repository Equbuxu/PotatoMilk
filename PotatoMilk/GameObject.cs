using PotatoMilk.Components;
using System;
using System.Collections.Generic;

namespace PotatoMilk
{
    public class GameObject
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
        public T AddComponent<T>(Dictionary<string, object> data = null)
            where T : IComponent, new()
        {
            T component = new();
            string name = typeof(T).Name;
            TrackNewComponent(component, name, data);
            return component;
        }

        internal void AddComponent(Type type, IComponent preconstructed, Dictionary<string, object> data)
        {
            string name = type.Name;
            TrackNewComponent(preconstructed, name, data);
        }

        public T GetComponent<T>()
            where T : IComponent
        {
            string name = typeof(T).Name;
            if (!namedComponents.ContainsKey(name))
                throw new Exception("The object does not have the component \"" + name + "\"");
            return (T)namedComponents[name];
        }

        private void TrackNewComponent(IComponent component, string name, Dictionary<string, object> data)
        {
            if (namedComponents.ContainsKey(name))
                throw new Exception("The object already has the component \"" + name + "\"");
            namedComponents.Add(name, component);
            components.Add(component);
            component.Initialize(this, data);
            Manager?.TrackComponent(component);
        }
    }
}
