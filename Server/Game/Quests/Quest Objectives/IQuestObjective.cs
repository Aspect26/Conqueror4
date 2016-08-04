namespace Server
{
    public interface IQuestObjective
    {
        bool IsCompleted { get; }
        string GetCodedData();

        bool Visited(int unitId);
        bool Killed(int unitId);
    }
}
