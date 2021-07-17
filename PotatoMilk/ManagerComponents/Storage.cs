using SFML.Graphics;
using System.Collections.Generic;

namespace PotatoMilk.ManagerComponents
{
    public class Storage
    {
        private readonly Dictionary<string, Texture> textures;
        private readonly Dictionary<string, ObjectRecipe> objects;
        private readonly Dictionary<string, List<ObjectRecipe>> rooms;

        internal Storage(
            Dictionary<string, Texture> textures,
            Dictionary<string, ObjectRecipe> objects,
            Dictionary<string, List<ObjectRecipe>> rooms)
        {
            this.textures = textures;
            this.objects = objects;
            this.rooms = rooms;
        }

        private Texture GetTexture(string name) => textures.ContainsKey(name) ? textures[name] : null;
        private ObjectRecipe GetObjectRecipe(string name) => objects.ContainsKey(name) ? objects[name] : null;
        private List<ObjectRecipe> GetRoom(string name) => rooms.ContainsKey(name) ? rooms[name] : null;
    }
}
