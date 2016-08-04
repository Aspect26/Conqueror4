using System;

namespace Server
{
    public class QuestObjectiveDifference : GenericDifference
    {
        private IQuest quest;

        public QuestObjectiveDifference(int uid, IQuest quest)
            :base(uid)
        {
            this.quest = quest;
        }

        public override string GetString()
        {
            return "QO" + quest.GetObjectivesCodedData();
        }
    }
}
