using System;
using Shared;

namespace Server
{
    public class UnitVisitedObjective : IQuestObjective
    {
        private int unitId;

        public bool IsCompleted { get; protected set; }

        public UnitVisitedObjective(int unitId)
        {
            this.unitId = unitId;
        }

        public string GetCodedData()
        {
            return "V^" + unitId;
        }

        public bool Visited(int unitId)
        {
            if(unitId == this.unitId)
            {
                IsCompleted = true;
                return true;
            }

            return false;
        }

        public bool Killed(int unitId) { return false; }

        public bool MovedTo(int x, int y) { return false; }
    }
}
