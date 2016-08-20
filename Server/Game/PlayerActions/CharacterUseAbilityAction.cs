using System;

namespace Server
{
    public class CharacterUseAbilityAction : IPlayerAction
    {
        private Character character;

        public CharacterUseAbilityAction(Character character)
        {
            this.character = character;
        }

        public Character GetCharacter()
        {
            return character;
        }

        public void Process(MapInstance mapInstance, long timeStamp)
        {
            IAbility ability = Data.GetCharacterAbility(character);
            if (ability.ManaCost > character.GetActualManaPoints())
                return;

            ability.Process(mapInstance);
        }
    }
}
