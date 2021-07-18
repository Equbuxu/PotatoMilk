using System.Collections.Generic;

namespace PotatoMilk
{
    public class ObjectRecipe
    {
        public Dictionary<string, Dictionary<string, object>> componentData;
        public string name;
        public bool? persistent;

        public ObjectRecipe()
        {
            componentData = new();
            name = null;
            persistent = null;
        }

        /// <summary>
        /// Performs a copy of the original, does not copy component data values (reuses references)
        /// </summary>
        public ObjectRecipe(ObjectRecipe original)
        {
            name = original.name;
            persistent = original.persistent;
            componentData = new();
            foreach (var component in original.componentData)
            {
                Dictionary<string, object> componentCopy = new();
                foreach (var componentValue in component.Value)
                {
                    componentCopy.Add(componentValue.Key, componentValue.Value);
                }
                componentData.Add(component.Key, componentCopy);
            }
        }

        /// <summary>
        /// Adds data from other to this recipe, replacing values with matching keys. Reuses data value references from other.
        /// </summary>
        public void OverrideFrom(ObjectRecipe other)
        {
            name = other.name ?? name;
            persistent = other.persistent ?? persistent;
            foreach (var component in other.componentData)
            {
                if (!componentData.ContainsKey(component.Key))
                    componentData.Add(component.Key, new());
                var curComponentData = componentData[component.Key];
                foreach (var componentValue in component.Value)
                {
                    if (curComponentData.ContainsKey(componentValue.Key))
                        curComponentData[componentValue.Key] = componentValue.Value;
                    else
                        curComponentData.Add(componentValue.Key, componentValue.Value);
                }
            }
        }
    }
}
