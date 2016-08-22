using System.Collections.Generic;

namespace Server
{
    public interface IQuestObjective
    {
        bool IsCompleted { get; }
        string GetCodedData();

        void Reset();

        bool Visited(int unitId);
        bool MovedTo(int x, int y);
        bool Killed(int unitId);

        IQuestObjective Copy();
    }
}
