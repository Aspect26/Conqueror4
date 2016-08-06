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
            if (objectives.Length == 0)
                return false;

            foreach(IQuestObjective objective in objectives)
            {
                if (!objective.IsCompleted)
                    return false;
            }

            return true;
        }

        public bool Visited(int unitId)
        {
            bool visited = false;
            foreach(IQuestObjective objective in objectives)
            {
                if (objective.Visited(unitId))
                    visited = true;
            }

            return visited;
        }

        public bool MovedTo(int x, int y)
        {
            bool moved = false;
            foreach(IQuestObjective objective in objectives)
            {
                if(objective.MovedTo(x, y))
                    moved = true;
            }

            return moved;
        }

        public bool Killed(int unitId)
        {
            bool killed = false;
            foreach (IQuestObjective objective in objectives)
            {
                if (objective.Killed(unitId))
                    killed = true;
            }

            return killed;
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
            {
                data.Append("&" + objective.GetCodedData());
                if (objective.IsCompleted)
                    data.Append("^CMP");
            }

            return data.ToString();
        }

        // *********************************
        // no quest singleton
        // *********************************
        private static Quest noQuest;

        public static Quest NoQuestSingleton {
            get
            {
                if (noQuest == null)
                    noQuest = new Quest(new QuestData(
                        new IQuestObjective[] { }, 
                        "No Quest", 
                        "You have completed all your quests!",
                        -1));

                return noQuest;
            }
        }
    }
}
