using System;

namespace Server
{
    public class LevelDifference : GenericDifference
    {
        private int level;

        public LevelDifference(int uid, int level) : base(uid)
        {
            this.level = level;
        }

        public override string GetString()
        {
            return "LV&" + level;
        }
    }
}
