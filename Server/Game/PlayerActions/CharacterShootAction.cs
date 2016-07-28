using System;

namespace Server
{
    public class CharacterShootAction : IPlayerAction
    {
        private Character character;
        private int x, y;

        public CharacterShootAction(Character character, int x, int y)
        {
            this.character = character;
            this.x = x;
            this.y = y;
        }

        public Character GetCharacter()
        {
            return character;
        }

        public ISendAction Process(long timeStamp)
        {
            character.Shoot(timeStamp, x,y);
            return null;
        }
    }
}
