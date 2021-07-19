using SFML.System;
using SFML.Window;
using System;

namespace PotatoMilk.Containers
{
    public class WorldMouseButtonEventArgs : EventArgs
    {
        public Vector2f worldPos;
        public Mouse.Button button;

        public WorldMouseButtonEventArgs(Vector2f position, Mouse.Button button)
        {
            this.worldPos = position;
            this.button = button;
        }
    }
}
