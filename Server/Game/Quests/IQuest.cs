namespace Server
{
    public interface IQuest
    {
        int QuestID { get; }
        int NextQuestID { get; }

        bool IsCompleted();

        string GetCodedData();
        string GetObjectivesCodedData();

        void Visited(int unitId);
    }
}
