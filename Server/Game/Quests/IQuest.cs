namespace Server
{
    public interface IQuest
    {
        int QuestID { get; }
        int NextQuestID { get; }

        bool IsCompleted();

        string GetCodedData();
        string GetObjectivesCodedData();

        bool Visited(int unitId);
        bool Killed(int unitId);
    }
}
