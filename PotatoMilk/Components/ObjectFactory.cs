using System;
using System.Collections.Generic;
using System.Linq;

namespace PotatoMilk.Components
{
    internal class ObjectFactory
    {
        private ObjectManager manager;
        private Dictionary<string, Type> typesCache = new();
        private Dictionary<Type, string> typesCacheBackwards = new();

        public ObjectFactory(ObjectManager manager)
        {
            this.manager = manager;

            typesCache = CollectCustomTypes();
            foreach (var keyvalue in typesCache)
                typesCacheBackwards.Add(keyvalue.Value, keyvalue.Key);
        }
        public GameObject CreateObject(ObjectRecipe recipe, ObjectManager manager)
        {
            GameObject obj = new();
            obj.Manager = manager;
            obj.Persistent = recipe.persistent ?? false;
            obj.Name = recipe.name;
            obj.Type = recipe.type;

            foreach (var keyvalue in recipe.componentData)
            {
                var component = CreateComponent(keyvalue.Key);
                obj.AddComponentNoTracking(keyvalue.Key, component, keyvalue.Value);
            }
            return obj;
        }

        public IComponent CreateComponent(string name)
        {
            if (!typesCache.ContainsKey(name))
                throw new Exception("Unknown component type: " + name);
            Type type = typesCache[name];
            var instance = (IComponent)Activator.CreateInstance(type);
            return instance;
        }

        public string GetTypeName<T>()
            where T : IComponent
        {
            var type = typeof(T);
            if (!typesCacheBackwards.ContainsKey(type))
                throw new Exception("Unknown component type: " + type);
            return typesCacheBackwards[type];
        }

        private Dictionary<string, Type> CollectCustomTypes()
        {
            try
            {
                return (
                       from assembly in AppDomain.CurrentDomain.GetAssemblies()
                       from type in assembly.GetTypes()
                       where type.IsAssignableTo(typeof(IComponent))
                       let attributes = type.GetCustomAttributes(typeof(ComponentNameAttribute), true)
                       where attributes != null && attributes.Length > 0
                       select (((ComponentNameAttribute)attributes.First()).GetName(), type)).ToDictionary(data => data.Item1, data => data.Item2);
            }
            catch (Exception e)
            {
                throw new Exception("Error while collecting types, there might be a name conflict among components", e);
            }
        }
    }
}
