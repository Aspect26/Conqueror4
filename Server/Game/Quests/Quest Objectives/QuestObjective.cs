namespace Server
{
    public abstract class QuestObjective : IQuestObjective
    {
        public abstract bool IsCompleted { get; protected set; }

        public abstract void Reset();
        public abstract IQuestObjective Copy();

        public abstract string GetCodedData();

        public virtual bool Killed(int unitId) { return false; }

        public virtual bool MovedTo(int x, int y) { return false; }

        public virtual bool Visited(int unitId) { return false; }
    }
}
