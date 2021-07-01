using SFML.Window;

namespace PotatoMilk
{
    public interface IKeyboardConsumer
    {
        void KeyPressed(object sender, KeyEventArgs args);
        void KeyReleased(object sender, KeyEventArgs args);
        void TextEntered(object sender, TextEventArgs args);
    }
}
