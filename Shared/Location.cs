namespace Shared
{
    public class Location
    {
        public int MapID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Location(int mapId, int x, int y)
        {
            this.MapID = mapId;
            this.X = x;
            this.Y = y;
        }

        public Location(int x, int y) 
            :this(-1, x, y)
        {
        }

        public Location()
            :this(-1, -1, -1)
        {
        }
    }
}
