using Shared;
using System.Drawing;

namespace Client
{
    public interface IUnit
    {
        // IDs
        int UnitID { get; }
        int UniqueID { get; }

        // STATUS
        int HitPoints { get; }
        int MaxHitPoints { get; }
        int ManaPoints { get; }
        int MaxManaPoints { get; }

        void PlayCycle(int timeSpan);
        void DrawUnit(Graphics g);
        Image GetCurrentImage();
        void SetLocation(int x, int y);

        Location Location { get; set; }

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
