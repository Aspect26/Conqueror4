namespace Server
{
    public interface IPlayerAction
    {
        void Process(MapInstance mapInstance, long timeStamp);
        Character GetCharacter();
    }
}
