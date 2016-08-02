namespace Server
{
    public class ActualHPDifference : GenericDifference
    {
        private int actualHp;

        public ActualHPDifference(int uid, int actualHp) : base(uid)
        {
            this.actualHp = actualHp;
        }

        public override string GetString()
        {
            return "H&" + actualHp;
        }
    }
}
