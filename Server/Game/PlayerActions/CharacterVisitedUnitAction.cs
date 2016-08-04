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
            character.CurrentQuest.Visited(host.UnitID);
            if (character.CurrentQuest.IsCompleted())
            {
                character.SetQuest(Data.GetQuest(character.CurrentQuest.NextQuestID));
                character.AddDifference(new NewQuestDifference(character.UniqueID, character.CurrentQuest));
            }
        }
    }
}
