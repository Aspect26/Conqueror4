﻿using System.Collections.Generic;
using Shared;

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
        int Level { get; }

        int RespawnTime { get; }

        Location GetLocation();
        string GetName();
        bool IsPlayer();
        void PlayCycle(int timeSpan);

        void TryHitByMissile(Missile missile);
        void HitByMissile(Missile missile);
        bool IsDead { get; }

        void AddExperience(int xp);

        bool Updated { get; set; }
        List<IUnitDifference> Differences { get; }
        List<IUnit> HittedBy { get; }
        void AddDifference(IUnitDifference difference);
    }
}
