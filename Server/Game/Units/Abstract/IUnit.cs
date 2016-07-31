using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        void StartMovingUp();
        void StartMovingRight();
        void StartMovingBottom();
        void StartMovingLeft();

        void StopMovingUp();
        void StopMovingRight();
        void StopMovingBottom();
        void StopMovingLeft();
    }
}
