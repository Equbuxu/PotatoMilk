using PotatoMilk;
using PotatoMilk.Components;
using System;

namespace PotatoMilkDemo
{
    class CollisionCounter
    {
        private int collisions = 0;
        private string name;
        public CollisionCounter(ICollider collider, string name)
        {
            this.name = name;
            collider.CollisionEnter += Collider_CollisionEnter;
            collider.CollisionStay += Collider_CollisionStay;
            collider.CollisionExit += Collider_CollisionExit;
        }

        private void Collider_CollisionExit(object sender, Collision e)
        {
            Console.WriteLine($"{name} collision EXIT");
            collisions = 0;
        }

        private void Collider_CollisionStay(object sender, Collision e)
        {
            Console.WriteLine($"{name} collides #{collisions}");
            collisions++;
        }

        private void Collider_CollisionEnter(object sender, Collision e)
        {
            Console.WriteLine($"{name} collision ENTER");
        }
    }
}
