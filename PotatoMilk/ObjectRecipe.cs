using System.Collections.Generic;

namespace PotatoMilk
{
    public class ObjectRecipe
    {
        public Dictionary<string, Dictionary<string, object>> componentData = new();
        public string name = "unnamed_object";
        public bool persistent = false;
    }
}
