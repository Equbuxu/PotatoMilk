using PotatoMilk.Components;
using PotatoMilk.ConsumerInterfaces;
using System.Collections.Generic;

namespace PotatoMilk.ManagerComponents
{
    internal class BehaviorManager
    {
        private HashSet<IUpdatable> updatables = new();
        private HashSet<ObjectBehavior> queuedToStart = new();
        public void TrackComponent(IComponent component)
        {
            if (component is not ObjectBehavior)
                return;
            queuedToStart.Add((ObjectBehavior)component);
            if (component is IUpdatable upd)
                updatables.Add(upd);
        }

        public void UntrackComponent(IComponent component)
        {
            if (component is not ObjectBehavior)
                return;
            if (component is IUpdatable upd)
                updatables.Remove(upd);
        }

        public void Update()
        {
            foreach (var q in queuedToStart)
            {
                q.Start();
            }
            queuedToStart.Clear();
            foreach (var upd in updatables)
            {
                upd.Update();
            }
        }
    }
}
