using SFML.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text.Json;

namespace PotatoMilk.Helpers
{
    internal static class StorageLoader
    {
        public static Dictionary<string, Texture> LoadTextures(byte[] zip, string json)
        {
            var deser = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

            Dictionary<string, Texture> textures = new();
            using (MemoryStream stream = new MemoryStream(zip))
            {
                using (ZipArchive archive = new ZipArchive(stream))
                {
                    foreach (var keyvalue in deser)
                    {
                        var entry = archive.GetEntry(keyvalue.Value);
                        using (var entryStream = entry.Open())
                        {
                            using (var textureStream = new MemoryStream())
                            {
                                entryStream.CopyTo(textureStream);
                                var texture = new Texture(textureStream.ToArray());
                                textures.Add(keyvalue.Key, texture);
                            }
                        }
                    }
                }
            }
            return textures;
        }

        public static Dictionary<string, ObjectRecipe> LoadObjectRecipes(string json, Dictionary<string, Texture> textures)
        {
            var parsed = JsonDocument.Parse(json);
            foreach (var obj in parsed.RootElement.EnumerateObject())
            {
                ObjectRecipe recipe = new();
                recipe.name = obj.Name;
                foreach (var component in obj.Value.EnumerateObject())
                {
                    if (obj.Name == "persistent")
                    {
                        recipe.persistent = obj.Value.GetBoolean();
                        continue;
                    }

                    foreach (var componentDataPoint in obj.Value.EnumerateObject())
                    {
                        var key, value = ParseComponentDataPoint(componentDataPoint);
                    }
                    Debug.Write(component.Name);
                }
            }
            return null;
        }

        private static (string, object) ParseComponentDataPoint()
        {

        }
    }
}
