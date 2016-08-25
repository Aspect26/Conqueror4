namespace Server
{
    public class ObjectInfo
    {
        public ObjectInfo(char mark, int X, int Y, string[] specialArguments)
        {
            this.Mark = mark;
            this.X = X;
            this.Y = Y;
            this.SpecialArguments = specialArguments;
        }

        public char Mark { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string[] SpecialArguments { get; set; }
    }
}
