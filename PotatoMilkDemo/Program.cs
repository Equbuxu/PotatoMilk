using PotatoMilk;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace PotatoMilkDemo
{
    static class Storage
    {
        public static Texture texture;
        public static Texture texture2;
        public static void Load()
        {
            texture = new Texture("textures.png");
            texture2 = new Texture("textures2.png");
        }
    }

    class Program
    {
        static ObjectManager manager;
        static void Main(string[] args)
        {
            Storage.Load();

            RenderWindow window = new(new VideoMode(640, 480), "PotatoMilk Demo Project", Styles.Default);
            window.Closed += (sender, args) => window.Close();
            window.SetVerticalSyncEnabled(true);

            manager = new(window);
            manager.Instantiate<WallCircle>();
            manager.Instantiate<PlayerTriangle>();
            manager.Instantiate<PlayerSquare>();
            manager.Instantiate<Polygon>();

            Image map = new("map.png");

            for (uint i = 0; i < map.Size.X; i++)
            {
                for (uint j = 0; j < map.Size.Y; j++)
                {
                    Color c = map.GetPixel(i, j);
                    if (c == Color.White)
                    {
                        var wall = manager.Instantiate<WallSquare>();
                        wall.startPos = new Vector2f(i * 32f, j * 32f);
                    }
                    else if (c == Color.Red)
                    {
                        var wall = manager.Instantiate<WallTriangle>();
                        wall.startPos = new Vector2f(i * 32f, j * 32f);
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
