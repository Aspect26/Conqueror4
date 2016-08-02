using System;

namespace Server
{
    public abstract class GenericDifference : IUnitDifference
    {
        public int UnitUID { get; protected set; }

        public GenericDifference(int UnitUid)
        {
            this.UnitUID = UnitUid;
        }

        public abstract string GetString();
    }
}
