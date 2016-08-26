namespace Server
{
    /// <summary>
    /// Represents a character's action of gettin near another unit.
    /// </summary>
    /// <seealso cref="Server.IPlayerAction" />
    public class CharacterVisitedUnitAction : IPlayerAction
    {
        private Character character;
        private IUnit host;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterVisitedUnitAction"/> class.
        /// </summary>
        /// <param name="character">The character.</param>
        /// <param name="host">The host.</param>
        public CharacterVisitedUnitAction(Character character, IUnit host)
        {
            this.character = character;
            this.host = host;
        }

        /// <summary>
        /// Gets the character this action is associated with.
        /// </summary>
        /// <returns>Character.</returns>
        public Character GetCharacter()
        {
            return character;
        }

        /// <summary>
        /// Processes the actions (check if it didn't completed a quest objective).
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="timeStamp">The time stamp.</param>
        public void Process(MapInstance instance, long timeStamp)
        {
            if (character.CurrentQuest.Visited(host.UnitID))
            {
                if (character.CurrentQuest.IsCompleted)
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
