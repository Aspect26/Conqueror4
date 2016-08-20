namespace Shared
{
    public class BaseStats
    {
        public int HitPoints { get; set; }

        public BaseStats(int hitPoints)
        {
            this.HitPoints = hitPoints;
        }

        public BaseStats Copy()
        {
            return new BaseStats(HitPoints);
        }
    }
}
