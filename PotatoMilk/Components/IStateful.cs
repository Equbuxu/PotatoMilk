using System;

namespace PotatoMilk.Components
{
    interface IStateful
    {
        event EventHandler StateUpdated;
    }
}
