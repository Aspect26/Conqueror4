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
        bool MovedTo(int x, int y);
        bool Killed(int unitId);
    }
}
