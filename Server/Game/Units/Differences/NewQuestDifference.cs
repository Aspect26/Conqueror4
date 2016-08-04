using System;

namespace Server
{ 
    public class NewQuestDifference : GenericDifference
    {
        private IQuest newQuest;

        public NewQuestDifference(int uid, IQuest newQuest)
            :base(uid)
        {
            this.newQuest = newQuest;
        }

        public override string GetString()
        {
            return newQuest.GetCodedData();
        }
    }
}
