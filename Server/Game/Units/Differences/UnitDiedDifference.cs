using System;

namespace Server
{
    public class UnitDiedDifference : IUnitDifference
    {
        private IUnit unit;

        public UnitDiedDifference(IUnit unit)
        {
            this.unit = unit;
        }

        public string GetString()
        {
            return "D";
        }
    }
}
