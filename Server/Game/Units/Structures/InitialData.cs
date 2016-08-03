using Shared;

namespace Server
{
    public class InitialData
    {
        public BaseStats MaxStats;
        public int Level;
        public int Fraction;

        public InitialData(BaseStats baseStats, int level, int fraction)
        {
            this.MaxStats = baseStats;
            this.Level = level;
            this.Fraction = fraction;
        }
    }

    public class CharacterInitialData
    {
        public BaseStats[] maxStats;
        public int Fraction;

        public CharacterInitialData(int fractions, BaseStats[] maxStats)
        {
            this.Fraction = fractions;
            this.maxStats = maxStats;
        }

        public BaseStats GetData(int level)
        {
            return maxStats[level];
        }
    }
}
