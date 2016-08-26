namespace Server
{
    /// <summary>
    /// Represents a quest. Quests develop the storyline of the game. There are two 
    /// qeustlines one for each fraction. Neither of the is currently finnished. There
    /// are curently simething like 9 quests for the human alliance fraction and non for
    /// the demons fracion (although I like their storyline more :D)
    /// A player has one active quest at every time in the game. When the player completes
    /// its current quest it automatically acquires a new one. If the player dies the quest
    /// objectives are reseted. 
    /// Actual state of the quest objectives of player is not stored in the SQL database
    /// meaning that if the server crashes (or is simply resterted / turned off), the 
    /// quest objectives for all players are reseted to their initial state.
    /// </summary>
    public interface IQuest
    {
        /// <summary>
        /// Gets the quest identifier.
        /// </summary>
        /// <value>The quest identifier.</value>
        int QuestID { get; }

        /// <summary>
        /// Gets the next quest identifier.
        /// </summary>
        /// <value>The next quest identifier.</value>
        int NextQuestID { get; }

        /// <summary>
        /// Checks if the quest objectives are completed. The quest objectives may
        /// be completed but the player still needs to turn the quest to the quest 
        /// giver hence it still may not be completed.
        /// </summary>
        /// <returns><c>true</c> if they are, <c>false</c> otherwise.</returns>
        bool RequirementsCompleted();

        /// <summary>
        /// Gets a value indicating whether the quest is completed.
        /// </summary>
        /// <value><c>true</c> if this instance is completed; otherwise, <c>false</c>.</value>
        bool IsCompleted { get; }

        /// <summary>
        /// Resets the quest objectives.
        /// </summary>
        void Reset();

        /// <summary>
        /// Gets the coded data for a server message.
        /// </summary>
        /// <returns>System.String.</returns>
        string GetCodedData();

        /// <summary>
        /// Gets the objectives coded data for a server message.
        /// </summary>
        /// <returns>System.String.</returns>
        string GetObjectivesCodedData();

        /// <summary>
        /// Calling this function informs the quest that a unit with specified
        /// unit identifier was visited.
        /// </summary>
        /// <param name="unitId">The unit identifier.</param>
        /// <returns><c>true</c> if any quest objecive was completed thanks to this
        /// event, <c>false</c> otherwise.</returns>
        bool Visited(int unitId);

        /// <summary>
        /// Calling this function informs the quest that a specified location was 
        /// visited.
        /// TODO mapId shall be included here.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns><c>true</c> if any quest objecive was completed thanks to this
        /// event, <c>false</c> otherwise.</returns>
        bool MovedTo(int x, int y);

        /// <summary>
        /// Calling this function informs the quest that a unit with specified
        /// unit identifier was killed by the unit with this quest.
        /// </summary>
        /// <param name="unitId">The unit identifier.</param>
        /// <returns><c>true</c> if any quest objecive was completed thanks to this
        /// event, <c>false</c> otherwise.</returns>
        bool Killed(int unitId);

        /// <summary>
        /// Performs a deep copy of this object. 
        /// </summary>
        /// <returns>IQuest.</returns>
        IQuest Copy();
    }
}
