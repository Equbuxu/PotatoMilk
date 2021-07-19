using System;

namespace PotatoMilk.Components
{
    public interface IStateful
    {
        event EventHandler StateUpdated;
    }
}
