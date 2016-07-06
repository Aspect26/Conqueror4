using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{ 
    public class CharacterEnterLocationAction : IPlayerAction
    {
        Character character;
        Game game;
        StateObject clientState;

        public CharacterEnterLocationAction(Game game, StateObject clientState, Character character)
        {
            this.character = character;
            this.game = game;
            this.clientState = clientState;
        }

        public Character GetCharacter()
        {
            return character;
        }

        public ISendAction Process()
        {
            MapInstance mapInstance = game.AddPlayer(clientState, character);
            return new CharactersInMap(mapInstance);
        }
    }
}
