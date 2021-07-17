using PotatoMilk.Components;
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

        public void TrackComponent(IComponent component)
        {
            if (component is not ObjectBehavior)
                return;
            if (component is IKeyboardConsumer keyb)
            {
                window.KeyPressed += keyb.KeyPressed;
                window.KeyReleased += keyb.KeyReleased;
                window.TextEntered += keyb.TextEntered;
            }
            if (component is IMouseButtonConsumer mb)
            {
                window.MouseButtonPressed += mb.MouseButtonPressed;
                window.MouseButtonReleased += mb.MouseButtonReleased;
            }
            if (component is IMouseMoveConsumer mm)
            {
                window.MouseMoved += mm.MouseMoved;
            }
        }

        public void UntrackComponent(IComponent component)
        {
            if (component is not ObjectBehavior)
                return;
            if (component is IKeyboardConsumer keyb)
            {
                window.KeyPressed -= keyb.KeyPressed;
                window.KeyReleased -= keyb.KeyReleased;
                window.TextEntered -= keyb.TextEntered;
            }
            if (component is IMouseButtonConsumer mb)
            {
                window.MouseButtonPressed -= mb.MouseButtonPressed;
                window.MouseButtonReleased -= mb.MouseButtonReleased;
            }
            if (component is IMouseMoveConsumer mm)
            {
                window.MouseMoved -= mm.MouseMoved;
            }
        }
    }
}
