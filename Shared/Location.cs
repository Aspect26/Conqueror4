namespace Shared
{
    /// <summary>
    /// Represents a unit's actual location. Contains map identifier and position
    /// on the map in form of X and Y coordinates.
    /// </summary>
    public class Location
    {
        public int MapID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Location"/> class.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public Location(int mapId, int x, int y)
        {
            this.MapID = mapId;
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Location"/> class with
        /// invalid map identifier.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public Location(int x, int y) 
            :this(-1, x, y)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Location"/> class with
        /// indalid values.
        /// </summary>
        public Location()
            :this(-1, -1, -1)
        {
        }
    }
}
