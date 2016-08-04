using Shared;
using System;

namespace Server
{
    public class CharacterVisitedUnitAction : IPlayerAction
    {
        private Character character;
        private IUnit host;

        public CharacterVisitedUnitAction(Character character, IUnit host)
        {
            this.character = character;
            this.host = host;
        }

        public Character GetCharacter()
        {
            return character;
        }

        public void Process(long timeStamp)
        {
            Console.WriteLine(character.GetName() + " visited " + SharedData.GetUnitName(host.UnitID));
        }
    }
}
