using Shared;
using System;

namespace Server
{
    public sealed class PriestHealAbility : CentralAOEAbility
    {
        public override int ID { get { return SharedData.ABILITY_PRIEST_HEAL; } }

        private const int MANA_COST = 20;
        private const int RANGE = 300;
        private const int BASE_HEAL = 20;
        private readonly int healAmount;

        public PriestHealAbility(IUnit source)
            :base(source, MANA_COST, RANGE, true)
        {
            healAmount = BASE_HEAL + source.GetSpellBonus();
        }

        protected override void hitUnit(IUnit unit, int distance)
        {
            unit.Heal(healAmount);
        }
    }
}
