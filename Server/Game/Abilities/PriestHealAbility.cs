using Shared;

namespace Server
{
    /// <summary>
    /// Represents the Priest specilization special ability. When used, it heals
    /// the priest and nearby friendly units (PC and NPC).
    /// This class cannot be inherited.
    /// </summary>
    /// <seealso cref="Server.CentralAOEAbility" />
    public sealed class PriestHealAbility : CentralAOEAbility
    {
        /// <summary>
        /// Gets the identifier of the ability.
        /// </summary>
        /// <value>The identifier.</value>
        public override int ID { get { return SharedData.ABILITY_PRIEST_HEAL; } }

        private const int MANA_COST = 20;
        private const int RANGE = 300;
        private const int BASE_HEAL = 20;
        private readonly int healAmount;

        /// <summary>
        /// Initializes a new instance of the <see cref="PriestHealAbility"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
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
