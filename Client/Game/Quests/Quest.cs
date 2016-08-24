using System.Collections.Generic;

namespace Client
{
    /// <summary>
    /// An implementation of IQuest interface.
    /// </summary>
    /// <seealso cref="Client.IQuest" />
    public class Quest : IQuest
    {
        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; protected set; }

        /// <summary>
        /// Gets the objectives.
        /// </summary>
        /// <value>The objectives.</value>
        public QuestObjective[] Objectives { get; protected set; }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Quest"/> class.
        /// </summary>
        /// <param name="objectives">The objectives.</param>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        public Quest(QuestObjective[] objectives, string title, string description)
        {
            this.Objectives = objectives;
            this.Title = title;
            this.Description = description;
        }

        /// <summary>
        /// Updates the objectives.
        /// </summary>
        /// <param name="objectives">The objectives.</param>
        public void UpdateObjectives(QuestObjective[] objectives)
        {
            this.Objectives = objectives;
        }

        /// <summary>
        /// A static function that creates a quest from the specified server message
        /// </summary>
        /// <param name="questData">The quest data part.</param>
        /// <param name="characterName">Name of the player's character (it is needed so the #NAME can be converted into
        /// the character's name).</param>
        /// <returns>The created quest.</returns>
        public static IQuest CreateQuest(string questData, string characterName)
        {
            string[] parts = questData.Split('&');

            if (parts.Length < 3 || parts[0] != "Q")
                return null;

            string title = parts[1];
            string description = parts[2];
            description.Replace("#NAME", characterName);
            QuestObjective[] objectives = CreateObjectives(parts, 3);

            return new Quest(objectives, title, description);
        }

        /// <summary>
        /// A static function that creates a quest objectives from the specified server message
        /// </summary>
        /// <param name="data">The server message.</param>
        /// <param name="startIndex">The start index (the server message comes in different forms (for update quest
        /// objectives and loading a quest) so the index on which the quest objectives data starts may vary.</param>
        /// <returns>QuestObjective[].</returns>
        public static QuestObjective[] CreateObjectives(string[] data, int startIndex)
        {
            var objectives = new List<QuestObjective>();
            for (int i = startIndex; i < data.Length; i++)
            {
                objectives.Add(new QuestObjective(data[i]));
            }

            return objectives.ToArray();
        }

        /// <summary>
        /// Resets the quest. This happens when the player dies.
        /// </summary>
        public void Reset()
        {
            foreach(QuestObjective objective in Objectives)
            {
                objective.Reset();
            }
        }
    }
}
