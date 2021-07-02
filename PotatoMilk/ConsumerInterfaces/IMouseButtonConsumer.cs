using SFML.Window;

namespace PotatoMilk.ConsumerInterfaces
{
    public interface IMouseButtonConsumer
    {
        void MouseButtonPressed(object sender, MouseButtonEventArgs args);
        void MouseButtonReleased(object sender, MouseButtonEventArgs args);
    }
}
