namespace Server
{
    public class ActualStatsDifference : GenericDifference
    {
        private IUnit unit;

        public ActualStatsDifference(IUnit unit)
            :base(unit.UniqueID)
        {
            this.unit = unit;
        }

        public override string GetString()
        {
            return "A&" + unit.GetActualHitPoints();
        }
    }
}
