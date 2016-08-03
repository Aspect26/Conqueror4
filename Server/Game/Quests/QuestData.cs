namespace Server
{
    public class QuestData
    {
        public IQuestObjective[] Objectives;
        public string Title;
        public string Description;

        public QuestData(IQuestObjective[] objectives, string title, string description)
        {
            this.Objectives = objectives;
            this.Title = title;
            this.Description = description;
        }
    }
}
