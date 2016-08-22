using System;

namespace Server
{
    public class UnitVisitedObjective : QuestObjective
    {
        private int unitId;

        public override bool IsCompleted { get; protected set; }

        public UnitVisitedObjective(int unitId)
        {
            this.unitId = unitId;
        }

        public override string GetCodedData()
        {
            return "V^" + unitId;
        }

        public override bool Visited(int unitId)
        {
            if(unitId == this.unitId)
            {
                IsCompleted = true;
                return true;
            }

            return false;
        }

        public override void Reset()
        {
            IsCompleted = false;
        }

        public override IQuestObjective Copy()
        {
            return new UnitVisitedObjective(unitId);
        }
    }
}
