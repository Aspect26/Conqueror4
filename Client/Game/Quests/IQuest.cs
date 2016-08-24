namespace Client
{
    /// <summary>
    /// A simple interface for game quests.
    /// A quests consists of its title, description and objectives.
    /// </summary>
    public interface IQuest
    {
        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; }

        /// <summary>
        /// Gets the objectives.
        /// </summary>
        /// <value>The objectives.</value>
        QuestObjective[] Objectives { get; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        string Description { get; }

        /// <summary>
        /// Updates the objectives.
        /// </summary>
        /// <param name="objectives">The objectives.</param>
        void UpdateObjectives(QuestObjective[] objectives);

        /// <summary>
        /// Resets the quest. This happens when the player dies.
        /// </summary>
        void Reset();
    }
}
