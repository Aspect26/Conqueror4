using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public partial class Character : IUnit
    {
        public int GetId()
        {
            return Spec;
        }

        public Location GetLocation()
        {
            return Location;
        }

        public string GetName()
        {
            return Name;
        }

        public void PlayCycle(int timeSpan)
        {
            // TODO: do player action
        }

        public bool IsPlayer()
        {
            return true;
        }
    }
}
