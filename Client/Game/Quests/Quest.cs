using System;
using System.Collections.Generic;

namespace Client
{
    public class Quest : IQuest
    {
        public string Description { get; protected set; }
        public QuestObjective[] Objectives { get; protected set; }
        public string Title { get; protected set; }

        public Quest(QuestObjective[] objectives, string title, string description)
        {
            this.Objectives = objectives;
            this.Title = title;
            this.Description = description;
        }

        public void UpdateObjectives(QuestObjective[] objectives)
        {
            this.Objectives = objectives;
        }

        public static IQuest CreateQuest(string questData)
        {
            string[] parts = questData.Split('&');

            if (parts.Length < 4 || parts[0] != "Q")
                return null;

            string title = parts[1];
            string description = parts[2];
            QuestObjective[] objectives = CreateObjectives(parts, 3);

            return new Quest(objectives, title, description);
        }

        public static QuestObjective[] CreateObjectives(string[] data, int startIndex)
        {
            var objectives = new List<QuestObjective>();
            for (int i = startIndex; i < data.Length; i++)
            {
                objectives.Add(new QuestObjective(data[i]));
            }

            return objectives.ToArray();
        }

        public void Reset()
        {
            foreach(QuestObjective objective in Objectives)
            {
                objective.Reset();
            }
        }
    }
}
