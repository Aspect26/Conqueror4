namespace Server
{
    public interface IQuest
    {
        int QuestID { get; }

        bool IsCompleted();
    }
}
