﻿using System.Text;

namespace Server
{
    public class Quest : IQuest
    {
        public int QuestID { get; protected set; }
        public int NextQuestID { get; protected set; }

        private string title;
        private IQuestObjective[] objectives;
        private string description;

        public Quest(QuestData data)
        {
            this.NextQuestID = data.NextQuestID;
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

        public void Visited(int unitId)
        {
            foreach(IQuestObjective objective in objectives)
            {
                objective.Visited(unitId);
            }
        }

        public string GetCodedData()
        {
            StringBuilder msg = new StringBuilder();

            msg.Append("Q&");
            msg.Append(title + "&");
            msg.Append(description);
            msg.Append(GetObjectivesCodedData());

            return msg.ToString();
        }

        public string GetObjectivesCodedData()
        {
            StringBuilder data = new StringBuilder();
            foreach (IQuestObjective objective in objectives)
                data.Append("&" + objective.GetCodedData());

            return data.ToString();
        }
    }
}
