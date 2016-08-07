namespace Client
{
    public interface IQuest
    {
        string Title { get; }
        QuestObjective[] Objectives { get; }
        string Description { get; }

        void UpdateObjectives(QuestObjective[] objectives);
        void Reset();
    }
}
