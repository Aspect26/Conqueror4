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

        public void Process(MapInstance instance, long timeStamp)
        {
            character.Location.X = x;
            character.Location.Y = y;
            character.Moved = true;

            if (character.CurrentQuest.MovedTo(x, y))
            {
                if (character.CurrentQuest.IsCompleted())
                {
                    character.SetQuest(Data.GetQuest(character.CurrentQuest.NextQuestID));
                    character.AddDifference(new NewQuestDifference(character.UniqueID, character.CurrentQuest));
                }
                else
                {
                    character.AddDifference(new QuestObjectiveDifference(character.UniqueID, character.CurrentQuest));
                }
            }
        }
    }
}
