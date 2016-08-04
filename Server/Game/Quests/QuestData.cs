namespace Server
{
    public class QuestData
    {
        public int NextQuestID;
        public IQuestObjective[] Objectives;
        public string Title;
        public string Description;

        public QuestData(IQuestObjective[] objectives, string title, string description, int nextQuestId)
        {
            this.Objectives = objectives;
            this.Title = title;
            this.Description = description;
            this.NextQuestID = nextQuestId;
        }
    }
}
