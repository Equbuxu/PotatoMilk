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
        public T AddComponent<T>()
            where T : IComponent, new()
        {
            T component = new();
            string name = typeof(T).Name;
            if (namedComponents.ContainsKey(name))
                throw new Exception("The object already has the component \"" + name + "\"");
            namedComponents.Add(name, component);
            components.Add(component);
            component.Initialize(this, null);
            Manager?.TrackComponent(component);
            return component;
        }

        public T GetComponent<T>()
            where T : IComponent
        {
            string name = typeof(T).Name;
            if (!namedComponents.ContainsKey(name))
                throw new Exception("The object does not have the component \"" + name + "\"");
            return (T)namedComponents[name];
        }
    }
}
