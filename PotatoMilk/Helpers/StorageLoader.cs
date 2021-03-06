using SFML.Graphics;
using SFML.System;
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

        public static Dictionary<string, ObjectRecipe> LoadRecipes(string json, Dictionary<string, Texture> textures)
        {
            var parsed = JsonDocument.Parse(json);
            Dictionary<string, ObjectRecipe> result = new();
            foreach (var recipeJson in parsed.RootElement.EnumerateObject())
            {
                var recipe = ParseRecipe(recipeJson.Value, textures);
                recipe.type = recipeJson.Name;
                result.Add(recipe.type, recipe);
            }
            return result;
        }

        public static Dictionary<string, List<ObjectRecipe>> LoadRooms(string json, Dictionary<string, ObjectRecipe> recipes, Dictionary<string, Texture> textures)
        {
            var parsed = JsonDocument.Parse(json);
            Dictionary<string, List<ObjectRecipe>> rooms = new();
            foreach (var keyvalue in parsed.RootElement.EnumerateObject())
            {
                string roomName = keyvalue.Name;
                List<ObjectRecipe> roomRecipes = new();
                var roomRecipesJson = keyvalue.Value.GetProperty("recipes");
                foreach (var roomRecipe in roomRecipesJson.EnumerateArray())
                {
                    string recipeType = roomRecipe.GetProperty("type").GetString();
                    var originalRecipe = recipes[recipeType];
                    ObjectRecipe finalRecipe = new ObjectRecipe(originalRecipe);
                    if (roomRecipe.TryGetProperty("overrides", out JsonElement recipeOverridesJson))
                    {
                        var recipeOverridesData = ParseRecipe(recipeOverridesJson, textures);
                        finalRecipe.OverrideFrom(recipeOverridesData);
                    }
                    roomRecipes.Add(finalRecipe);
                }
                rooms.Add(roomName, roomRecipes);
            }
            return rooms;
        }

        private static ObjectRecipe ParseRecipe(JsonElement recipeJson, Dictionary<string, Texture> textures)
        {
            ObjectRecipe recipe = new();
            foreach (var component in recipeJson.EnumerateObject())
            {
                if (component.Name == "persistent")
                {
                    recipe.persistent = component.Value.GetBoolean();
                    continue;
                }
                else if (component.Name == "name")
                {
                    recipe.name = component.Value.GetString();
                    continue;
                }

                recipe.componentData.Add(component.Name, ParseComponentValues(component.Value, textures));

            }
            return recipe;
        }

        private static Dictionary<string, object> ParseComponentValues(JsonElement componentDataJson, Dictionary<string, Texture> textures)
        {
            Dictionary<string, object> result = new();
            foreach (var componentDataPoint in componentDataJson.EnumerateObject())
            {
                var (key, value) = ParseComponentDataPoint(componentDataPoint, textures);
                result.Add(key, value);
            }
            return result;
        }

        private static (string, object) ParseComponentDataPoint(JsonProperty dataPoint, Dictionary<string, Texture> textures)
        {
            var type = dataPoint.Value.GetProperty("type");
            var value = dataPoint.Value.GetProperty("value");
            var name = dataPoint.Name;
            return type.GetString() switch
            {
                "float" => (name, (float)value.GetDouble()),
                "int" => (name, value.GetInt32()),
                "bool" => (name, value.GetBoolean()),
                "string" => (name, value.GetString()),
                "Vector2f" => (name, ConvertVector2f(value)),
                "Color" => (name, ConvertColor(value)),
                "List<Vector2f>" => (name, ConvertListVector2f(value)),
                "Texture" => (name, textures[value.GetString()]),
                _ => (name, null),
            };
        }

        private static Color ConvertColor(JsonElement elem)
        {
            if (elem.ValueKind == JsonValueKind.String)
                return ColorHelper.FromString(elem.GetString());
            var enumerator = elem.EnumerateArray();
            enumerator.MoveNext();
            var r = (float)enumerator.Current.GetDouble();
            enumerator.MoveNext();
            var g = (float)enumerator.Current.GetDouble();
            enumerator.MoveNext();
            var b = (float)enumerator.Current.GetDouble();
            if (enumerator.MoveNext())
            {
                var a = (float)enumerator.Current.GetDouble();
                return ColorHelper.FromFloats(r, g, b, a);
            }
            return ColorHelper.FromFloats(r, g, b);
        }

        private static List<Vector2f> ConvertListVector2f(JsonElement elem)
        {
            List<Vector2f> list = new();
            foreach (var vec in elem.EnumerateArray())
                list.Add(ConvertVector2f(vec));
            return list;
        }

        private static Vector2f ConvertVector2f(JsonElement elem)
        {
            var enumerator = elem.EnumerateArray();
            Vector2f result = new();
            enumerator.MoveNext();
            result.X = (float)enumerator.Current.GetDouble();
            enumerator.MoveNext();
            result.Y = (float)enumerator.Current.GetDouble();
            return result;
        }
    }
}
