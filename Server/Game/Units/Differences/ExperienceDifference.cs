namespace Server
{
    public class ExperienceDifference : GenericDifference
    {
        private int xp;

        public ExperienceDifference(int uid, int xp) : base(uid)
        {
            this.xp = xp;
        }

        public override string GetString()
        {
            return "X&" + xp;
        }
    }
}
