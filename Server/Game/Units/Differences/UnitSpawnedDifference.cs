﻿using System;

namespace Server
{
    public class UnitSpawnedDifference : GenericDifference
    {
        private IUnit unit;

        public UnitSpawnedDifference(IUnit unit): base(unit.UnitID)
        {
            this.unit = unit;
        }

        public override string GetString()
        {
            return "R&" + unit.GetName() + "&" + unit.UnitID + "&" + unit.GetLocation().X + "&" + unit.GetLocation().Y 
                + "&" + unit.MaxStats.HitPoints + "&" + unit.ActualStats.HitPoints + "&" + unit.Fraction;
        }
    }
}
