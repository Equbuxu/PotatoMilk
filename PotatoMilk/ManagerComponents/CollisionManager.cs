using PotatoMilk.Components;
using PotatoMilk.Containers;
using PotatoMilk.Helpers;
using System;
using System.Collections.Generic;

namespace PotatoMilk.ManagerComponents
{
    public class CollisionManager
    {
        private HashSet<ICollider> toUpdateState = new();
        private List<ICollider> colliders = new();
        private HashSet<(ICollider, ICollider)> activeCollisionPairs = new();
        private HashSet<(ICollider, ICollider)> detachedCollisions = new();

        public bool IsColliding(ICollider a)
        {
            for (int j = 0; j < colliders.Count; j++)
            {
                var b = colliders[j];
                if (a == b)
                    continue;
                if (CollisionHelper.CheckPairCollision(a, b).HasValue)
                    return true;
            }
            return false;
        }

        internal CollisionManager() { }

        internal void TrackComponent(IComponent component)
        {
            if (component is ICollider collider)
            {
                if (collider.Enabled)
                    ActivateCollider(collider);
                else
                    DeactivateCollider(collider);
                collider.StateUpdated += OnStateUpdate;
            }
        }
        internal void UntrackComponent(IComponent component)
        {
            if (component is ICollider collider)
            {
                DeactivateCollider(collider);
                collider.StateUpdated -= OnStateUpdate;
            }
        }

        internal void CalculateCollisions()
        {
            UpdateState();
            for (int i = 0; i < colliders.Count - 1; i++)
            {
                for (int j = i + 1; j < colliders.Count; j++)
                {
                    var a = colliders[i];
                    var b = colliders[j];
                    var pair = (a, b);
                    var gjkTriangle = CollisionHelper.CheckPairCollision(a, b);
                    if (activeCollisionPairs.Contains(pair))
                    {
                        if (gjkTriangle.HasValue)
                        {
                            a.InvokeCollisionStay(new Collision(b, a, true, gjkTriangle.Value));
                            b.InvokeCollisionStay(new Collision(a, b, false, gjkTriangle.Value));
                        }
                        else
                        {
                            a.InvokeCollisionExit(new Collision(b, a));
                            b.InvokeCollisionExit(new Collision(a, b));
                            activeCollisionPairs.Remove(pair);
                        }
                    }
                    else if (gjkTriangle.HasValue)
                    {
                        a.InvokeCollisionEnter(new Collision(b, a, true, gjkTriangle.Value));
                        b.InvokeCollisionEnter(new Collision(a, b, false, gjkTriangle.Value));
                        activeCollisionPairs.Add(pair);
                    }
                }
            }
            RemoveDetachedCollisions();
        }

        private void OnStateUpdate(object sender, EventArgs args)
        {
            ICollider collider = (ICollider)sender;
            toUpdateState.Add(collider);
        }

        private void UpdateState()
        {
            foreach (ICollider collider in toUpdateState)
            {
                if (collider.Enabled)
                    ActivateCollider(collider);
                else
                    DeactivateCollider(collider);
            }
            toUpdateState.Clear();
        }

        private void ActivateCollider(ICollider collider)
        {
            if (!colliders.Contains(collider))
                colliders.Add(collider);
        }

        private void DeactivateCollider(ICollider collider)
        {
            if (!colliders.Remove(collider))
                return;
            toUpdateState.Remove(collider);

            foreach (var pair in activeCollisionPairs)
            {
                if (pair.Item1 == collider || pair.Item2 == collider)
                {
                    detachedCollisions.Add(pair);
                }
            }
        }

        private void RemoveDetachedCollisions()
        {
            foreach (var pair in detachedCollisions)
            {
                pair.Item1.InvokeCollisionExit(new Collision(pair.Item2, pair.Item1));
                pair.Item2.InvokeCollisionExit(new Collision(pair.Item1, pair.Item2));
                activeCollisionPairs.Remove(pair);
            }
            detachedCollisions.Clear();
        }
    }
}
