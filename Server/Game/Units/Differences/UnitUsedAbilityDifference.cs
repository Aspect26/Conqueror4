using System;

namespace Server
{
    public class UnitUsedAbilityDifference : GenericDifference
    {
        private IAbility ability;
        private IUnit source;

        public UnitUsedAbilityDifference(IUnit source, IAbility ability)
            :base (source.UniqueID)
        {
            this.source = source;
            this.ability = ability;
        }

        public override string GetString()
        {
            return "AB&" + ability.ID + "&" + ability.GetCodedData();
        }
    }
}
