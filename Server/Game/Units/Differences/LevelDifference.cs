using System;

namespace Server
{
    public class LevelDifference : IUnitDifference
    {
        private int level;

        public LevelDifference(int level)
        {
            this.level = level;
        }

        public string GetString()
        {
            return "LV&" + level;
        }
    }
}
