using System;

namespace Server
{
    /// <summary>
    /// Represents a character's action of tryinf to use its special ability.
    /// </summary>
    /// <seealso cref="Server.IPlayerAction" />
    public class CharacterUseAbilityAction : IPlayerAction
    {
        private Character character;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterUseAbilityAction"/> class.
        /// </summary>
        /// <param name="character">The character.</param>
        public CharacterUseAbilityAction(Character character)
        {
            this.character = character;
        }

        /// <summary>
        /// Gets the character this action is associated with.
        /// </summary>
        /// <returns>Character.</returns>
        public Character GetCharacter()
        {
            return character;
        }

        /// <summary>
        /// If the unit has enogh mana, it uses the ability.
        /// NOTE that there is no cooldown for abilities currently implemented.
        /// </summary>
        /// <param name="mapInstance">The map instance in which it happened.</param>
        /// <param name="timeStamp">The time stamp.</param>
        public void Process(MapInstance mapInstance, long timeStamp)
        {
            IAbility ability = Data.GetCharacterAbility(character);
            if (ability.ManaCost > character.GetActualManaPoints())
                return;

            ability.Process(mapInstance);
            mapInstance.AddGeneralDifference(new UnitUsedAbilityDifference(character, ability));
        }
    }
}
