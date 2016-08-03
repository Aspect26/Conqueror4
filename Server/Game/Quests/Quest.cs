using System.Text;

namespace Server
{
    public class Quest : IQuest
    {
        public int QuestID { get; protected set; }

        private string title;
        private IQuestObjective[] objectives;
        private string description;

        public Quest(QuestData data)
        {
            this.title = data.Title;
            this.objectives = data.Objectives;
            this.description = data.Description;
        }

        public bool IsCompleted()
        {
            foreach(IQuestObjective objective in objectives)
            {
                if (!objective.IsCompleted)
                    return false;
            }

            return true;
        }

        public string GetCodedData()
        {
            StringBuilder msg = new StringBuilder();

            msg.Append("Q&");
            msg.Append(title + "&");
            msg.Append(description);
            foreach (IQuestObjective objective in objectives)
                msg.Append("&" + objective.GetCodedData());

            return msg.ToString();
        }
    }
}
