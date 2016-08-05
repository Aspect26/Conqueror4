using System.Collections.Generic;

namespace Server
{
    public interface IQuestObjective
    {
        bool IsCompleted { get; }
        string GetCodedData();

        IQuestObjective[] PreRequiredObjecties { get; }

        bool Visited(int unitId);
        bool MovedTo(int x, int y);
        bool Killed(int unitId);
    }
}
