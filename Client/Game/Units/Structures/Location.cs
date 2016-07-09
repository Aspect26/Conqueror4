using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Location
    {
        public int MapID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Location()
        {
            MapID = X = Y -1;
        }

        public Location(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.MapID = -1;
        }
    }
}
