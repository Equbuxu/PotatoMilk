using System.Collections.Generic;

namespace PotatoMilk.Components
{
    public interface IComponent
    {
        GameObject GameObject { get; }
        void Initialize(GameObject container, Dictionary<string, object> data);

    }
}
