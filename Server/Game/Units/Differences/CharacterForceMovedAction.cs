using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class CharacterForceMovedAction : IUnitDifference
    {
        private Location location;

        public int UnitUID { get; protected set; }

        public CharacterForceMovedAction(int uid, Location location)
        {
            this.UnitUID = uid;
            this.location = location;
        }

        public string GetString()
        {
            return "FM&" + location.MapID + "&" + location.X + "&" + location.Y;
        }
    }
}
