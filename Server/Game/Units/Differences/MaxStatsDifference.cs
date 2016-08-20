namespace Server
{
    public class MaxStatsDifference : GenericDifference
    {
        private IUnit unit;

        public MaxStatsDifference(IUnit unit)
            : base(unit.UniqueID)
        {
            this.unit = unit;
        }

        public override string GetString()
        {
            return "M&" + unit.GetMaxHitPoints();
        }
    }
}
