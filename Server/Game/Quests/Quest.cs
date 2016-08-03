namespace Server
{
    public class Quest : IQuest
    {
        public int QuestID { get; protected set; }

        private IQuestObjective[] objectives;

        public Quest(IQuestObjective[] objectives)
        {
            this.objectives = objectives;
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
    }
}
