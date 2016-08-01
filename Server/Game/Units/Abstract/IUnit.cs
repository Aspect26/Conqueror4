using System.Collections.Generic;
using Shared;

namespace Server
{
    public interface IUnit
    {
        int UnitID { get; }
        int UniqueID { get; }
        Location GetLocation();
        string GetName();
        bool IsPlayer();
        void PlayCycle(int timeSpan);

        bool Updated { get; set; }
        List<string> Differences { get; set; }
    }
}
