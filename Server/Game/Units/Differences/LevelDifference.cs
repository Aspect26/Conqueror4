using System;

namespace Server
{
    public class LevelDifference : GenericDifference
    {
        private int level;
        private int xpRequired;

        public LevelDifference(int uid, int level, int xpRequired) : base(uid)
        {
            this.level = level;
            this.xpRequired = xpRequired;
        }

        public override string GetString()
        {
            return "LV&" + level + "&" + xpRequired;
        }
    }
}
