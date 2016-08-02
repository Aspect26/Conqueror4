namespace Server
{
    public class ExperienceDifference : IUnitDifference
    {
        private int xp;

        public ExperienceDifference(int xp)
        {
            this.xp = xp;
        }

        public string GetString()
        {
            return "X&" + xp;
        }
    }
}
