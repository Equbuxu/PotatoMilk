using SFML.Window;

namespace PotatoMilk
{
    public interface IMouseButtonConsumer
    {
        void MouseButtonPressed(object sender, MouseButtonEventArgs args);
        void MouseButtonReleased(object sender, MouseButtonEventArgs args);
    }
}
