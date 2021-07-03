using PotatoMilk.ConsumerInterfaces;
using SFML.Graphics;

namespace PotatoMilk.ManagerComponents
{
    internal class EventDispatcher
    {
        private RenderWindow window;
        public EventDispatcher(RenderWindow window)
        {
            this.window = window;
        }

        public void TrackGameObject(GameObject obj)
        {
            if (obj is IKeyboardConsumer keyb)
            {
                window.KeyPressed += keyb.KeyPressed;
                window.KeyReleased += keyb.KeyReleased;
                window.TextEntered += keyb.TextEntered;
            }
            if (obj is IMouseButtonConsumer mb)
            {
                window.MouseButtonPressed += mb.MouseButtonPressed;
                window.MouseButtonReleased += mb.MouseButtonReleased;
            }
            if (obj is IMouseMoveConsumer mm)
            {
                window.MouseMoved += mm.MouseMoved;
            }
        }

        public void UntrackGameObject(GameObject obj)
        {
            if (obj is IKeyboardConsumer keyb)
            {
                window.KeyPressed -= keyb.KeyPressed;
                window.KeyReleased -= keyb.KeyReleased;
                window.TextEntered -= keyb.TextEntered;
            }
            if (obj is IMouseButtonConsumer mb)
            {
                window.MouseButtonPressed -= mb.MouseButtonPressed;
                window.MouseButtonReleased -= mb.MouseButtonReleased;
            }
            if (obj is IMouseMoveConsumer mm)
            {
                window.MouseMoved -= mm.MouseMoved;
            }
        }
    }
}
