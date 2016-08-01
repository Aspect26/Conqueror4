namespace Server
{
    public interface IPlayerAction
    {
        void Process(long timeStamp);
        Character GetCharacter();
    }
}
