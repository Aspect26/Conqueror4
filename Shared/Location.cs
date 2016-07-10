namespace Shared
{
    public class Location
    {
        public int MapID { get; set; }
        public float X { get; set; }
        public float Y { get; set; }

        public Location(int mapId, float x, float y)
        {
            this.MapID = mapId;
            this.X = x;
            this.Y = y;
        }

        public Location(float x, float y) 
            :this(-1, x, y)
        {
        }

        public Location()
            :this(-1, -1f, -1f)
        {
        }
    }
}
