﻿namespace Server
{
    public interface IQuestObjective
    {
        bool IsCompleted { get; }
        string GetCodedData();
    }
}