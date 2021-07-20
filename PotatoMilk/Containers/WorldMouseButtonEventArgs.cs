using PotatoMilk.Components;
using SFML.System;
using SFML.Window;
using System;

namespace PotatoMilk.Containers
{
    public class WorldMouseButtonEventArgs : EventArgs
    {
        public Vector2f worldPos;
        public Mouse.Button button;
        public Camera camera;

        public WorldMouseButtonEventArgs(Vector2f position, Mouse.Button button, Camera camera)
        {
            this.worldPos = position;
            this.button = button;
            this.camera = camera;
        }
    }
}
