using PotatoMilk;
using PotatoMilkDemo.Properties;
using SFML.Graphics;
using SFML.Window;
using System.IO;

namespace PotatoMilkDemo
{

    class Program
    {
        static ObjectManager manager;

        private static string ReadTextResource(byte[] resource) => (new StreamReader(new MemoryStream(resource))).ReadToEnd();
        static void Main(string[] args)
        {

            RenderWindow window = new(new VideoMode(640, 480), "PotatoMilk Demo Project", Styles.Default);
            window.Closed += (sender, args) => window.Close();
            window.SetVerticalSyncEnabled(true);

            string texturesJson = ReadTextResource(Resources.textures);
            string recipesJson = ReadTextResource(Resources.recipes);
            string roomsJson = ReadTextResource(Resources.rooms);
            byte[] texturesZip = Resources.textureImages;

            manager = new(window);
            manager.LoadStorage(texturesZip, texturesJson, recipesJson, roomsJson);
            manager.LoadRoom(manager.Storage.GetRoom("room1"));

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
