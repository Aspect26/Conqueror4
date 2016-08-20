using System.Collections.Generic;
using Shared;
using System.Drawing;

namespace Server
{
    public interface IUnit
    {
        int UnitID { get; }
        int UniqueID { get; }
        MapInstance MapInstance { get; }
        int HitRange { get; }

        BaseStats MaxStats { get; }
        BaseStats ActualStats { get; }
        void ResetStats();
        int Level { get; }

        int GetActualHitPoints();
        int GetActualManaPoints();
        int GetMaxHitPoints();
        int GetMaxManaPoints();
        int GetDamage();
        int GetArmor();

        int Fraction { get; }
        int RespawnTime { get; }

        Location GetLocation();
        Point SpawnPosition { get; }
        string GetName();
        bool IsPlayer();
        void PlayCycle(int timeSpan);

        void TryHitByMissile(Missile missile);
        void HitByMissile(Missile missile);
        bool IsDead { get; }

        void LeaveCombatWith(IUnit unit);
        void EnterCombatWith(IUnit unit);

        void AddExperience(int xp);

        bool Moved { get; set; }
        List<IUnitDifference> Differences { get; }
        List<IUnit> HittedBy { get; }
        List<IUnit> CurrentlyVisited { get; }
        List<IUnit> InCombatWith { get; }
        IItem GetDroppedItem();
        void AddDifference(IUnitDifference difference);
    }
}
