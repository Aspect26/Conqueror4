namespace Server
{
    public class PlayerShootDifference : IUnitDifference
    {
        private int xDir;
        private int yDir;

        public PlayerShootDifference(int x, int y)
        {
            this.xDir = x;
            this.yDir = y;
        }

        public string GetString()
        {
            return "S&" + xDir + "&" + yDir;
        }
    }
}
