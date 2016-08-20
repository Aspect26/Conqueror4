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
        BaseStats MaxStats { get; }
        BaseStats ActualStats { get; }

        int GetActualHitPoints();
        int GetActualManaPoints();
        int GetMaxHitPoints();
        int GetMaxManaPoints();

        bool IsDead { get; }
        bool Isplayer();

        void UpdateActualStats(BaseStats stats);
        void UpdateMaxStats(BaseStats stats);

        int Fraction { get; }

        void PlayCycle(int timeSpan);
        void DrawUnit(Graphics g);
        Image GetCurrentImage();
        void SetLocation(int x, int y);

        void TryHitByMissile(Missile missile);
        void Kill();

        Location Location { get; set; }
    }
}
