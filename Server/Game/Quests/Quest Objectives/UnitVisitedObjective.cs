using System;

namespace Server
{
    public class UnitVisitedObjective : QuestObjective
    {
        private int unitId;

        public override bool IsCompleted { get; protected set; }

        public UnitVisitedObjective(int unitId, IQuestObjective[] preRequisites = null)
            :base(preRequisites)
        {
            this.unitId = unitId;
        }

        public override string GetCodedData()
        {
            return "V^" + unitId;
        }

        public override bool Visited(int unitId)
        {
            if (!checkRequisities())
                return false;

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
    }
}
