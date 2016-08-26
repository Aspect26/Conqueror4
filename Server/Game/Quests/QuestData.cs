namespace Server
{
    /// <summary>
    /// A simple class representing a quest data.
    /// </summary>
    public class QuestData
    {
        public int NextQuestID;
        public IQuestObjective[] Objectives;
        public string Title;
        public string Description;
        public int QuestCompletionerId;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestData"/> class.
        /// </summary>
        /// <param name="objectives">The objectives.</param>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <param name="nextQuestId">The next quest identifier.</param>
        /// <param name="questCompletionerId">The quest completioner identifier.</param>
        public QuestData(IQuestObjective[] objectives, string title, string description, int nextQuestId,
            int questCompletionerId)
        {
            this.Objectives = objectives;
            this.Title = title;
            this.Description = description;
            this.NextQuestID = nextQuestId;
            this.QuestCompletionerId = questCompletionerId;
        }
    }
}
