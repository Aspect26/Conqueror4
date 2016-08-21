namespace Server
{
    public interface IAbility
    {
        IUnit Source { get; }
        int ManaCost { get; }
        int ID { get; }

        void Process(MapInstance mapInstance);
        string GetCodedData();
    }
}
