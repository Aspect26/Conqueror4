using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{ 
    public class CharacterEnterAction : IPlayerAction
    {
        Character character;

        public CharacterEnterAction(Character character)
        {
            this.character = character;
        }

        public ISendAction Process(List<IUnit> units)
        {
            units.Add(character);

            return new CharactersInMap();
        }
    }
}
