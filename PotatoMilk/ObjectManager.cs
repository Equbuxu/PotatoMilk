﻿using PotatoMilk.Components;
using PotatoMilk.ConsumerInterfaces;
using PotatoMilk.ManagerComponents;
using SFML.Graphics;
using System.Collections.Generic;

namespace PotatoMilk
{
    public class ObjectManager
    {
        private QuadBatchingManager quadBatchingManager = new();
        private PolygonBatchingManager polygonBatchingManager = new();
        public CollisionManager Collisions { get; private set; } = new();

        private HashSet<GameObject> allObjects = new();
        private HashSet<IUpdatable> updatables = new();
        private EventDispatcher eventDispatcher;
        private HashSet<GameObject> toDestroy = new();
        private HashSet<GameObject> toInstantiate = new();

        public ObjectManager(RenderWindow window)
        {
            eventDispatcher = new(window);
        }

        public T Instantiate<T>()
            where T : GameObject, new()
        {
            T obj = new();
            obj.Manager = this;
            toInstantiate.Add(obj);
            return obj;
        }

        public void TrackComponent(IComponent component)
        {
            quadBatchingManager.TrackComponent(component);
            polygonBatchingManager.TrackComponent(component);
            Collisions.TrackComponent(component);
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

        private void InstantianteQueued()
        {
            foreach (var obj in toInstantiate)
            {
                eventDispatcher.TrackGameObject(obj);

                if (obj is IUpdatable upd)
                    updatables.Add(upd);
                allObjects.Add(obj);
                obj.Start();
            }
            toInstantiate.Clear();
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
                }

                eventDispatcher.UntrackGameObject(obj);
                allObjects.Remove(obj);
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
            InstantianteQueued();
            DestroyQueued();
            quadBatchingManager.Draw(window);
            polygonBatchingManager.Draw(window);
        }
    }
}
