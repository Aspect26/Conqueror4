using System;

namespace Server
{
    public class UnitKillObjective : IQuestObjective
    {
        private int unitId;
        private int unitsKilled;
        private int unitsRequired;

        public bool IsCompleted { get { return unitsKilled >= unitsRequired; } }

        public UnitKillObjective(int unitId, int killCount)
        {
            this.unitId = unitId;
            this.unitsRequired = killCount;
            this.unitsKilled = 0;
        }

        public string GetCodedData()
        {
            return "K^" + unitId + "^" + unitsKilled + "^" + unitsRequired;
        }

        public bool Killed(int unitId)
        {
            if(unitId == this.unitId && unitsRequired > unitsKilled)
            {
                unitsKilled++;
                return true;
            }
            return false;
        }

        public bool Visited(int unitId) { return false; }

        public bool MovedTo(int x, int y) { return false; }
    }
}
