using PotatoMilk.Components;
using PotatoMilk.ConsumerInterfaces;
using SFML.Graphics;
using System.Collections.Generic;

namespace PotatoMilk
{
    public class ObjectManager
    {
        private BatchingManager batchingManager = new();
        public CollisionManager Collisions { get; private set; } = new();

        private HashSet<IUpdatable> updatables = new();
        private EventDispatcher eventDispatcher;
        private HashSet<GameObject> toDestroy = new();

        public ObjectManager(RenderWindow window)
        {
            eventDispatcher = new(window);
        }

        public T Instantiate<T>()
            where T : GameObject, new()
        {
            T obj = new();
            obj.Manager = this;

            eventDispatcher.TrackGameObject(obj);

            if (obj is IUpdatable upd)
                updatables.Add(upd);

            obj.Start();
            return obj;
        }

        public void TrackComponent(IComponent component)
        {
            batchingManager.TrackComponent(component);
            Collisions.TrackComponent(component);
        }
        public void Destroy(GameObject obj)
        {
            if (!toDestroy.Contains(obj))
                toDestroy.Add(obj);
        }

        private void DestroyQueued()
        {
            foreach (var obj in toDestroy)
            {
                foreach (var cmp in obj.Components)
                {
                    batchingManager.UntrackComponent(cmp);
                    Collisions.UntrackComponent(cmp);
                }

                eventDispatcher.UntrackGameObject(obj);

                if (obj is IUpdatable upd)
                    updatables.Remove(upd);
            }
            toDestroy.Clear();
        }

        public void Draw(RenderWindow window)
        {
            foreach (var upd in updatables)
                upd.Update();
            Collisions.CalculateCollisions();
            DestroyQueued();
            batchingManager.Draw(window);
        }
    }
}
