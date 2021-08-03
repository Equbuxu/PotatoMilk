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
            internal set
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
        public string Type { get; internal set; }
        public string Name { get; set; }
        public Transform Transform { get; private set; }

        public T AddComponent<T>(Dictionary<string, object> data = null)
            where T : IComponent
        {
            string name = manager.ObjectFactory.GetTypeName<T>();
            T component = (T)manager.ObjectFactory.CreateComponent(name);
            AppendNewComponent(component, name, data);
            Manager?.TrackComponent(component);
            return component;
        }

        /// <summary>
        /// Used to add new components on creation, as they should only start working (getting tracked) on the next tick and can't be created with new
        /// </summary>
        internal void AddComponentNoTracking(string name, IComponent preconstructed, Dictionary<string, object> data)
        {
            AppendNewComponent(preconstructed, name, data);
        }

        public T GetComponent<T>()
            where T : IComponent
        {
            string name = manager.ObjectFactory.GetTypeName<T>();
            if (!namedComponents.ContainsKey(name))
                throw new Exception("The object does not have the component \"" + name + "\"");
            return (T)namedComponents[name];
        }

        public IComponent GetComponent(string name)
        {
            if (!namedComponents.ContainsKey(name))
                throw new Exception("The object does not have the component \"" + name + "\"");
            return namedComponents[name];
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
            component.Initialize(this, data, name);

            if (component is Transform transform)
                Transform = transform;
        }
    }
}
