using System;
using System.Collections.Generic;
using System.Linq;

namespace PotatoMilk.Components
{
    internal class ObjectFactory
    {
        private ObjectManager manager;
        private Dictionary<string, Type> typesCache = new();

        public ObjectFactory(ObjectManager manager)
        {
            this.manager = manager;

            typesCache = CollectCustomTypes();
        }
        public GameObject CreateObject(ObjectRecipe recipe, ObjectManager manager)
        {
            GameObject obj = new();
            obj.Manager = manager;
            obj.Persistent = recipe.persistent;
            obj.Name = recipe.name;

            foreach (var keyvalue in recipe.componentData)
            {
                var data = keyvalue.Value;
                switch (keyvalue.Key)
                {
                    case "circle_collider": obj.AddComponentNoTracking<CircleCollider>(data); break;
                    case "convex_polygon_collider": obj.AddComponentNoTracking<ConvexPolygonCollider>(data); break;
                    case "polygon_renderer": obj.AddComponentNoTracking<PolygonRenderer>(data); break;
                    case "quad_renderer": obj.AddComponentNoTracking<QuadRenderer>(data); break;
                    case "transform": obj.AddComponentNoTracking<Transform>(data); break;
                    default: AddObjectBehaviour(obj, data, keyvalue.Key); break;
                };
            }
            return obj;
        }

        private void AddObjectBehaviour(GameObject obj, Dictionary<string, object> data, string name)
        {
            if (!typesCache.ContainsKey(name))
                throw new Exception("Unknown component type: " + name);
            Type type = typesCache[name];
            var instance = (IComponent)Activator.CreateInstance(type);
            obj.AddComponentNoTracking(type, instance, data);
        }

        private Dictionary<string, Type> CollectCustomTypes()
        {
            return (
                   from assembly in AppDomain.CurrentDomain.GetAssemblies()
                   from type in assembly.GetTypes()
                   where type.IsSubclassOf(typeof(ObjectBehavior))
                   let attributes = type.GetCustomAttributes(typeof(ComponentNameAttribute), true)
                   where attributes != null && attributes.Length > 0
                   select (((ComponentNameAttribute)attributes.First()).GetName(), type)).ToDictionary(data => data.Item1, data => data.Item2);
        }
    }
}
