using System.Text;

namespace Server
{
    public class Quest : IQuest
    {
        public int QuestID { get; protected set; }

        private IQuestObjective[] objectives;
        private string description;

        public Quest(IQuestObjective[] objectives, string description)
        {
            this.objectives = objectives;
            this.description = description;
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
            msg.Append(description);
            foreach (IQuestObjective objective in objectives)
                msg.Append("&" + objective.GetCodedData());

            return msg.ToString();
        }
    }
}
