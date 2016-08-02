using Shared;

namespace Server
{
    public class InitialData
    {
        public BaseStats MaxStats;
        public int Level;

        public InitialData(BaseStats baseStats, int level)
        {
            this.MaxStats = baseStats;
            this.Level = level;
        }
    }
}
