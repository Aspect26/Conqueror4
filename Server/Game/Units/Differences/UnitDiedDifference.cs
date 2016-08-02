using System;

namespace Server
{
    public class UnitDiedDifference : GenericDifference
    {
        private IUnit unit;

        public UnitDiedDifference(IUnit unit) : base(unit.UniqueID)
        {
            this.unit = unit;
        }

        public override string GetString()
        {
            return "D";
        }
    }
}
