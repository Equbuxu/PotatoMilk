using SFML.Graphics;
using System.Collections.Generic;
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
    }
}
