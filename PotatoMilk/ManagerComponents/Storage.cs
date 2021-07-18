using SFML.Graphics;
using System.Collections.Generic;

namespace PotatoMilk.ManagerComponents
{
    public class Storage
    {
        private readonly Dictionary<string, Texture> textures;
        private readonly Dictionary<string, ObjectRecipe> recipes;
        private readonly Dictionary<string, List<ObjectRecipe>> rooms;

        internal Storage(
            Dictionary<string, Texture> textures,
            Dictionary<string, ObjectRecipe> recipes,
            Dictionary<string, List<ObjectRecipe>> rooms)
        {
            this.textures = textures;
            this.recipes = recipes;
            this.rooms = rooms;
        }

        public Texture GetTexture(string name) => textures.ContainsKey(name) ? textures[name] : null;
        public ObjectRecipe GetObjectRecipe(string name) => recipes.ContainsKey(name) ? recipes[name] : null;
        public List<ObjectRecipe> GetRoom(string name) => rooms.ContainsKey(name) ? rooms[name] : null;
    }
}
