using System;

namespace Server
{
    /// <summary>
    /// Represents an action of player moving (changing its location).
    /// IT SHALL BE CONTROLLED BY THE SERVER IF IT IS CORRECT MOVE BUT UNFORTUNATELY
    /// IT IS NOT :(
    /// </summary>
    /// <seealso cref="Server.IPlayerAction" />
    class CharacterMovedAction : IPlayerAction
    {
        private Character character;
        private int x;
        private int y;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterMovedAction"/> class.
        /// </summary>
        /// <param name="character">The character.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public CharacterMovedAction(Character character, int x, int y)
        {
            this.character = character;
            this.x = x;
            this.y = y;
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
        /// Moves the character and checks if the character didnt visited any unit
        /// (e.g.: for quest objective completition).
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="timeStamp">The time stamp.</param>
        public void Process(MapInstance instance, long timeStamp)
        {
            character.Location.X = x;
            character.Location.Y = y;
            character.Moved = true;

            if (character.CurrentQuest.MovedTo(x, y))
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
