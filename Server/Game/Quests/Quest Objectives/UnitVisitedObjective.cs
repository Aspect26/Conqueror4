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

        public void Visited(int unitId)
        {
            if(unitId == this.unitId)
            {
                IsCompleted = true;
            }
        }
    }
}
