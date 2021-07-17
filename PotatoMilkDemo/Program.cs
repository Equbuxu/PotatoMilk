using PotatoMilk;
using PotatoMilkDemo.Properties;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Collections.Generic;

namespace PotatoMilkDemo
{

    class Program
    {
        static ObjectManager manager;
        static void Main(string[] args)
        {
            //Storage.Load();

            RenderWindow window = new(new VideoMode(640, 480), "PotatoMilk Demo Project", Styles.Default);
            window.Closed += (sender, args) => window.Close();
            window.SetVerticalSyncEnabled(true);

            string texturesJson = System.Text.Encoding.UTF8.GetString(Resources.textures);
            string objectsJson = System.Text.Encoding.UTF8.GetString(Resources.objects);
            string roomsJson = System.Text.Encoding.UTF8.GetString(Resources.rooms);
            byte[] texturesZip = Resources.textureImages;

            manager = new(window);
            manager.LoadStorage(texturesZip, texturesJson, objectsJson, roomsJson);
            manager.Instantiate(Storage.recipies["player_square"]);
            manager.Instantiate(Storage.recipies["player_triangle"]);
            manager.Instantiate(Storage.recipies["polygon"]);
            manager.Instantiate(Storage.recipies["wall_circle"]);
            Image map = new("map.png");

            for (uint i = 0; i < map.Size.X; i++)
            {
                for (uint j = 0; j < map.Size.Y; j++)
                {
                    Color c = map.GetPixel(i, j);
                    if (c == Color.White)
                    {
                        var wall = manager.Instantiate(Storage.recipies["wall_square"]);
                        wall.GetComponent<WallSquare>().startPos = new Vector2f(i * 32f, j * 32f);
                    }
                    else if (c == Color.Red)
                    {
                        var recipe = Storage.recipies["wall_triangle"];
                        var copy = new ObjectRecipe();
                        foreach (var keyvalue in recipe.componentData)
                        {
                            copy.componentData.Add(keyvalue.Key, new Dictionary<string, object>());
                            foreach (var intkeyvalue in keyvalue.Value)
                            {
                                copy.componentData[keyvalue.Key].Add(intkeyvalue.Key, intkeyvalue.Value);
                            }
                        }
                        copy.componentData["transform"]["position"] = new Vector2f(i * 32f, j * 32f);
                        var wall = manager.Instantiate(copy);
                    }
                }
            }

            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear(new Color(128, 128, 128));
                manager.Draw(window);
                window.Display();
            }

        }

    }
}
