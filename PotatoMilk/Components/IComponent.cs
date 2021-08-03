using System.Collections.Generic;

namespace PotatoMilk.Components
{
    public interface IComponent
    {
        string TypeName { get; }
        GameObject GameObject { get; }
        void Initialize(GameObject container, Dictionary<string, object> data, string typeName);

    }
}
