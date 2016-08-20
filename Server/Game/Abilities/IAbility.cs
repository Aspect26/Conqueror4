namespace Server
{
    public interface IAbility
    {
        int ManaCost { get; }
        void Process(MapInstance mapInstance);
    }
}
