namespace Server
{
    public class PlayerShootDifference : GenericDifference
    {
        private int xDir;
        private int yDir;

        public PlayerShootDifference(int uid, int x, int y) : base(uid)
        {
            this.xDir = x;
            this.yDir = y;
        }

        public override string GetString()
        {
            return "S&" + xDir + "&" + yDir;
        }
    }
}
