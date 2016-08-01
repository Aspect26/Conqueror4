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

        void TryHitByMissile(Missile missile);

        Location Location { get; set; }
    }
}
