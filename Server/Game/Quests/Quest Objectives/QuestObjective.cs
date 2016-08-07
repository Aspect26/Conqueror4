using System;
using System.Collections.Generic;

namespace Server
{
    public abstract class QuestObjective : IQuestObjective
    {
        public abstract bool IsCompleted { get; protected set; }

        public IQuestObjective[] PreRequiredObjecties { get; }

        public QuestObjective(IQuestObjective[] preRequisities)
        {
            this.PreRequiredObjecties = preRequisities;
        }

        protected bool checkRequisities()
        {
            if (PreRequiredObjecties == null)
                return true;

            foreach(IQuestObjective requisity in PreRequiredObjecties)
            {
                if (!requisity.IsCompleted)
                {
                    return false;
                }
            }

            return true;
        }

        public abstract void Reset();

        public abstract string GetCodedData();

        public virtual bool Killed(int unitId) { return false; }

        public virtual bool MovedTo(int x, int y) { return false; }

        public virtual bool Visited(int unitId) { return false; }
    }
}
