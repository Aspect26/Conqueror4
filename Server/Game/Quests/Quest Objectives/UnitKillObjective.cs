namespace Server
{
    public class UnitKillObjective : IQuestObjective
    {
        private int unitId;
        private int unitsKilled;
        private int unitsRequired;

        public bool IsCompleted { get; protected set; }

        public UnitKillObjective(int unitId, int killCount)
        {
            this.unitId = unitId;
            this.unitsRequired = killCount;
            this.unitsKilled = 0;
        }
    }
}
