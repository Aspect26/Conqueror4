using System;
using System.Text;

namespace Server
{
    public class Quest : IQuest
    {
        public int QuestID { get; protected set; }
        public int NextQuestID { get; protected set; }
        public bool IsCompleted { get; private set; }

        private int questCompletionerId;
        private string title;
        private IQuestObjective[] objectives;
        private string description;

        public Quest(int id, QuestData data)
        {
            this.QuestID = id;
            this.NextQuestID = data.NextQuestID;
            this.title = data.Title;
            this.objectives = data.Objectives;
            this.description = data.Description;
            this.questCompletionerId = data.QuestCompletionerId;
        }

        public bool RequirementsCompleted()
        {
            // because of NO_QUEST
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

            if(unitId == questCompletionerId && RequirementsCompleted())
            {
                IsCompleted = true;
                return true;
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

        public void Reset()
        {
            foreach(IQuestObjective objective in objectives)
            {
                objective.Reset();
            }
        }

        public IQuest Copy()
        {
            IQuestObjective[] objectivesCopy = new IQuestObjective[objectives.Length];
            for(int i = 0; i < objectives.Length; i++)
            {
                objectivesCopy[i] = objectives[i].Copy();
            }
            return new Quest(QuestID, new QuestData(objectivesCopy, title, description, NextQuestID, questCompletionerId));
        }

        // *********************************
        // no quest singleton
        // *********************************
        private static Quest noQuest;

        public static Quest NoQuestSingleton {
            get
            {
                if (noQuest == null)
                    noQuest = new Quest(0,new QuestData(
                        new IQuestObjective[] { }, 
                        "No Quest", 
                        "You have completed all your quests!",
                        -1, -1));

                return noQuest;
            }
        }
    }
}
