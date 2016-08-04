using System;

namespace Server
{
    class CharacterMovedAction : IPlayerAction
    {
        private Character character;
        private int x;
        private int y;

        public CharacterMovedAction(Character character, int x, int y)
        {
            this.character = character;
            this.x = x;
            this.y = y;
        }

        public Character GetCharacter()
        {
            return character;
        }

        public void Process(long timeStamp)
        {
            character.Location.X = x;
            character.Location.Y = y;
            character.Moved = true;
        }
    }
}
