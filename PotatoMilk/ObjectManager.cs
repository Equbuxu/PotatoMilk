using PotatoMilk.Components;
using PotatoMilk.ManagerComponents;
using SFML.Graphics;
using System.Collections.Generic;

namespace PotatoMilk
{
    public class ObjectManager
    {
        private QuadBatchingManager quadBatchingManager = new();
        private PolygonBatchingManager polygonBatchingManager = new();
        private EventDispatcher eventDispatcher;
        private BehaviorManager behaviorManager = new();

        public CollisionManager Collisions { get; private set; } = new();

        private ObjectFactory objectFactory;
        private HashSet<GameObject> toTrack = new();

        private HashSet<GameObject> allObjects = new();
        private HashSet<GameObject> toDestroy = new();

        public ObjectManager(RenderWindow window)
        {
            eventDispatcher = new(window);
            objectFactory = new(this);
        }

        public GameObject Instantiate(ObjectRecipe recipe)
        {
            GameObject obj = objectFactory.CreateObject(recipe, this);
            toTrack.Add(obj);
            return obj;
        }

        public void TrackComponent(IComponent component)
        {
            quadBatchingManager.TrackComponent(component);
            polygonBatchingManager.TrackComponent(component);
            Collisions.TrackComponent(component);
            eventDispatcher.TrackComponent(component);
            behaviorManager.TrackComponent(component);
        }
        public void Destroy(GameObject obj)
        {
            if (!toDestroy.Contains(obj))
                toDestroy.Add(obj);
        }

        public void ClearRoom()
        {
            foreach (var obj in allObjects)
            {
                if (obj.Persistent)
                    continue;
                Destroy(obj);
            }
        }

        private void TrackQueued()
        {
            foreach (GameObject obj in toTrack)
            {
                allObjects.Add(obj);
                foreach (var cmp in obj.Components)
                {
                    TrackComponent(cmp);
                }
            }
            toTrack.Clear();
        }

        private void DestroyQueued()
        {
            foreach (var obj in toDestroy)
            {
                foreach (var cmp in obj.Components)
                {
                    quadBatchingManager.UntrackComponent(cmp);
                    polygonBatchingManager.UntrackComponent(cmp);
                    Collisions.UntrackComponent(cmp);
                    eventDispatcher.UntrackComponent(cmp);
                    behaviorManager.UntrackComponent(cmp);
                }

                allObjects.Remove(obj);
            }
            toDestroy.Clear();
        }

        public void Draw(RenderWindow window)
        {
            behaviorManager.Update();
            Collisions.CalculateCollisions();
            TrackQueued();
            DestroyQueued();
            quadBatchingManager.Draw(window);
            polygonBatchingManager.Draw(window);
        }
    }
}
