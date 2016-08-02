namespace Server
{
    public class ActualHPDifference : IUnitDifference
    {
        private int actualHp;

        public ActualHPDifference(int actualHp)
        {
            this.actualHp = actualHp;
        }

        public string GetString()
        {
            return "H&" + actualHp;
        }
    }
}
