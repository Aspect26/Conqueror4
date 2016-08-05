namespace Server
{
    public interface IQuestObjective
    {
        bool IsCompleted { get; }
        string GetCodedData();

        bool Visited(int unitId);
        bool MovedTo(int x, int y);
        bool Killed(int unitId);
    }
}
