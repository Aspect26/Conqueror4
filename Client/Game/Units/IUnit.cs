﻿using Shared;
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
        int ManaPoints { get; }
        int MaxManaPoints { get; }
        bool IsDead { get; }

        void PlayCycle(int timeSpan);
        void DrawUnit(Graphics g);
        Image GetCurrentImage();
        void SetLocation(int x, int y);

        void TryHitByMissile(Missile missile);
        void Kill();

        Location Location { get; set; }
    }
}
