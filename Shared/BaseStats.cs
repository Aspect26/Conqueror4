namespace Shared
{
    public class BaseStats
    {
        public int HitPoints;

        public BaseStats(int hitPoints)
        {
            this.HitPoints = hitPoints;
        }

        public void ReduceHitPoints(int amount)
        {
            HitPoints -= amount;
        }

        public BaseStats Copy()
        {
            return new BaseStats(HitPoints);
        }
    }
}
