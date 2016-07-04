using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
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
    }
}
