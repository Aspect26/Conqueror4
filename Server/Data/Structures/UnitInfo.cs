namespace Server
{
    public class UnitInfo
    {
        public UnitInfo(int ID, int X, int Y)
        {
            this.ID = ID;
            this.X = X;
            this.Y = Y;
        }

        public int ID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
