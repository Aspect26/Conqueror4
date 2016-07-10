using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class CharacterStartMovingAction : IPlayerAction
    {
        private Game game;
        private Character character;
        private MovingDirection direction;

        public CharacterStartMovingAction(Game game, Character character, MovingDirection direction)
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
                    character.StartMovingBottom(); break;
                case MovingDirection.Right:
                    character.StartMovingRight(); break;
                case MovingDirection.Up:
                    character.StartMovingUp(); break;
                case MovingDirection.Left:
                    character.StartMovingLeft(); break;
            }

            return new CharactersInMap(game.GetMapInstanceOfCharacter(character));
        }
    }
}
