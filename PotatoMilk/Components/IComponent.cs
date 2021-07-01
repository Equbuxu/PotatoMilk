namespace PotatoMilk.Components
{
    public interface IComponent
    {
        GameObject GameObject { get; }
        void Initialize(GameObject parent);
    }
}
