using System.Text;

namespace Server
{
    /// <summary>
    /// A IQuest interface implementation.
    /// </summary>
    /// <seealso cref="Server.IQuest" />
    public class Quest : IQuest
    {
        /// <summary>
        /// Gets the quest identifier.
        /// </summary>
        /// <value>The quest identifier.</value>
        public int QuestID { get; protected set; }

        /// <summary>
        /// Gets the next quest identifier.
        /// </summary>
        /// <value>The next quest identifier.</value>
        public int NextQuestID { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the quest is completed.
        /// </summary>
        /// <value><c>true</c> if this instance is completed; otherwise, <c>false</c>.</value>
        public bool IsCompleted { get; private set; }

        private int questCompletionerId;
        private string title;
        private IQuestObjective[] objectives;
        private string description;

        /// <summary>
        /// Initializes a new instance of the <see cref="Quest"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="data">The data.</param>
        public Quest(int id, QuestData data)
        {
            this.QuestID = id;
            this.NextQuestID = data.NextQuestID;
            this.title = data.Title;
            this.objectives = data.Objectives;
            this.description = data.Description;
            this.questCompletionerId = data.QuestCompletionerId;
        }

        /// <summary>
        /// Checks if the quest objectives are completed. The quest objectives may
        /// be completed but the player still needs to turn the quest to the quest
        /// giver hence it still may not be completed.
        /// </summary>
        /// <returns><c>true</c> if they are, <c>false</c> otherwise.</returns>
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

        /// <summary>
        /// Calling this function informs the quest that a unit with specified
        /// unit identifier was visited.
        /// </summary>
        /// <param name="unitId">The unit identifier.</param>
        /// <returns><c>true</c> if any quest objecive was completed thanks to this
        /// event, <c>false</c> otherwise.</returns>
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

        /// <summary>
        /// Calling this function informs the quest that a specified location was
        /// visited.
        /// TODO mapId shall be included here.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns><c>true</c> if any quest objecive was completed thanks to this
        /// event, <c>false</c> otherwise.</returns>
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

        /// <summary>
        /// Calling this function informs the quest that a unit with specified
        /// unit identifier was killed by the unit with this quest.
        /// </summary>
        /// <param name="unitId">The unit identifier.</param>
        /// <returns><c>true</c> if any quest objecive was completed thanks to this
        /// event, <c>false</c> otherwise.</returns>
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

        /// <summary>
        /// Gets the coded data for a server message.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetCodedData()
        {
            StringBuilder msg = new StringBuilder();

            msg.Append("Q&");
            msg.Append(title + "&");
            msg.Append(description);
            msg.Append(GetObjectivesCodedData());

            return msg.ToString();
        }

        /// <summary>
        /// Gets the objectives coded data for a server message.
        /// </summary>
        /// <returns>System.String.</returns>
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

        /// <summary>
        /// Resets the quest objectives.
        /// </summary>
        public void Reset()
        {
            foreach(IQuestObjective objective in objectives)
            {
                objective.Reset();
            }
        }

        /// <summary>
        /// Performs a deep copy of this object.
        /// </summary>
        /// <returns>IQuest.</returns>
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
        // NO QUEST SINGLETON
        // *********************************
        private static Quest noQuest;

        /// <summary>
        /// Gets the no quest singleton.
        /// </summary>
        /// <value>The no quest singleton.</value>
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
