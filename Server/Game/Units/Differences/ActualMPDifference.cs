namespace Server
{
    public class ActualMPDifference : GenericDifference
    {
        private int actualMp;

        public ActualMPDifference(int uid, int actualMp) : base(uid)
        {
            this.actualMp = actualMp;
        }

        public override string GetString()
        {
            return "MN&" + actualMp;
        }
    }
}
