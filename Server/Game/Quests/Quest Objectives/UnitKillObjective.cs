using System;
using System.Collections.Generic;

namespace Server
{
    public class UnitKillObjective : QuestObjective
    {
        private int unitId;
        private int unitsKilled;
        private int unitsRequired;

        public override bool IsCompleted {
            get {
                if (!checkRequisities())
                    return false;

                return unitsKilled >= unitsRequired;
            }
            protected set { }
        }

        public UnitKillObjective(int unitId, int killCount, IQuestObjective[] preRequisities = null)
            :base(preRequisities)
        {
            this.unitId = unitId;
            this.unitsRequired = killCount;
            this.unitsKilled = 0;
        }

        public override string GetCodedData()
        {
            return "K^" + unitId + "^" + unitsKilled + "^" + unitsRequired;
        }

        public override bool Killed(int unitId)
        {
            if(unitId == this.unitId && unitsRequired > unitsKilled)
            {
                unitsKilled++;
                return true;
            }
            return false;
        }

        public override void Reset()
        {
            unitsKilled = 0;
        }
    }
}
