using PotatoMilk.Components;
using PotatoMilk.Helpers;
using PotatoMilk.ManagerComponents;
using SFML.Graphics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("PotatoMilkTests")]
namespace PotatoMilk
{
    public class ObjectManager
    {
        private QuadBatchingManager quadBatchingManager = new();
        private PolygonBatchingManager polygonBatchingManager = new();
        private EventDispatcher eventDispatcher;
        private BehaviorManager behaviorManager = new();
        public DrawingManager DrawingManager { get; private set; }
        public CollisionManager CollisionManager { get; private set; } = new();
        public MouseCollisionManager MouseCollisionManager { get; private set; }


        public Storage Storage { get; private set; }

        private ObjectFactory objectFactory;
        private HashSet<GameObject> toTrack = new();

        private HashSet<GameObject> allObjects = new();
        private HashSet<GameObject> toDestroy = new();

        public ObjectManager(RenderWindow window)
        {
            eventDispatcher = new(window);
            objectFactory = new(this);
            DrawingManager = new(quadBatchingManager, polygonBatchingManager);
            MouseCollisionManager = new(window);
        }

        public void LoadStorage(byte[] texturesZip, string texturesJson, string recipesJson, string roomsJson)
        {
            var textures = StorageLoader.LoadTextures(texturesZip, texturesJson);
            var recipes = StorageLoader.LoadRecipes(recipesJson, textures);
            var rooms = StorageLoader.LoadRooms(roomsJson, recipes, textures);
            Storage = new(textures, recipes, rooms);
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
            CollisionManager.TrackComponent(component);
            eventDispatcher.TrackComponent(component);
            behaviorManager.TrackComponent(component);
            DrawingManager.TrackComponent(component);
            MouseCollisionManager.TrackComponent(component);
        }

        public void Destroy(GameObject obj)
        {
            if (!toDestroy.Contains(obj))
                toDestroy.Add(obj);
        }

        public void LoadRoom(List<ObjectRecipe> room)
        {
            ClearRoom();
            foreach (var recipe in room)
            {
                Instantiate(recipe);
            }
        }

        private void ClearRoom()
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
                    CollisionManager.UntrackComponent(cmp);
                    eventDispatcher.UntrackComponent(cmp);
                    behaviorManager.UntrackComponent(cmp);
                    DrawingManager.UntrackComponent(cmp);
                    MouseCollisionManager.UntrackComponent(cmp);
                }

                allObjects.Remove(obj);
            }
            toDestroy.Clear();
        }

        public void Update(RenderWindow window)
        {
            window.DispatchEvents();
            behaviorManager.Update();
            CollisionManager.CalculateCollisions();
            TrackQueued();
            DestroyQueued();
        }

        public void Draw(RenderWindow window)
        {
            DrawingManager.Draw(window);
        }
    }
}
