using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class CharacterStopMovingAction : IPlayerAction
    {
        private Game game;
        private Character character;
        private MovingDirection direction;

        public CharacterStopMovingAction(Game game, Character character, MovingDirection direction)
        {
            this.game = game;
            this.character = character;
            this.direction = direction;
        }

        public Character GetCharacter()
        {
            return character;
        }

        public ISendAction Process()
        {
            switch (direction)
            {
                case MovingDirection.Bottom:
                    character.StopMovingBottom(); break;
                case MovingDirection.Right:
                    character.StopMovingRight(); break;
                case MovingDirection.Up:
                    character.StopMovingUp(); break;
                case MovingDirection.Left:
                    character.StopMovingLeft(); break;
            }

            return new CharactersInMap(game.GetMapInstanceOfCharacter(character));
        }
    }
}
