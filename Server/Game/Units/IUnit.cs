using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public interface IUnit
    {
        int GetId();
        Location GetLocation();
        string GetName();

        void PlayCycle(int timeSpan);
        bool IsPlayer();
    }
}
