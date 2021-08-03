using SFML.Graphics;
using System;
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

        public Texture GetTexture(string name) => textures.ContainsKey(name) ? textures[name] : throw new Exception($"No texture with name {name}");
        public ObjectRecipe GetObjectRecipe(string name) => recipes.ContainsKey(name) ? recipes[name] : throw new Exception($"No recipe with name {name}");
        public List<ObjectRecipe> GetRoom(string name) => rooms.ContainsKey(name) ? rooms[name] : throw new Exception($"No room with name {name}");
    }
}
