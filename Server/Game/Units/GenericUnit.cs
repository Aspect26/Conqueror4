using Shared;
using System.Collections.Generic;
using System.Drawing;
using System;

namespace Server
{
    public class GenericUnit : IUnit
    {
        public int UnitID { get; protected set; }
        public int UniqueID { get; protected set; }

        public BaseStats MaxStats { get; protected set; }
        public BaseStats ActualStats { get; protected set; }
        public int Level { get; protected set; }

        // TODO: respawn time
        public int RespawnTime { get { return 120; } }
        public int Fraction { get; }

        public int HitRange { get; protected set; }
        public List<IUnit> HittedBy { get; protected set; }
        public bool IsDead { get; protected set; }

        public MapInstance MapInstance { get; protected set; }
        public Location Location { get; set; }
        public MovingDirection Direction { get; private set; }
        public bool Updated { get; set; }
        public List<IUnitDifference> Differences { get; protected set; }

        protected int movingSpeed;
        protected const int SLOWING_CONSTANT = 5;

        public string Name { get; set; }

        public GenericUnit(int unitID, int uniqueId, Location location, MapInstance mapInstance, InitialData data)
        {
            this.Location = location;
            this.UnitID = unitID;
            this.UniqueID = uniqueId;
            this.Name = "Unknown";
            this.MapInstance = mapInstance;
            this.HitRange = 30;
            this.Differences = new List<IUnitDifference>();
            this.HittedBy = new List<IUnit>();
            this.IsDead = false;
            this.MaxStats = data.MaxStats.Copy();
            this.ActualStats = data.MaxStats.Copy();
            this.Level = data.Level;
            this.Fraction = data.Fraction;

            Direction = MovingDirection.None;
            movingSpeed = 1;
        }

        public GenericUnit(string name, int unitId, int uniqueId, Location location, MapInstance mapInstance, 
            InitialData data) 
            : this(unitId, uniqueId, location, mapInstance, data)
        {
            this.Name = name;
        }

        public void SetUniqueID(int uniqueId)
        {
            this.UniqueID = uniqueId;
        }

        public virtual void PlayCycle(int timeSpan)
        {
        }

        private int ShootCooldown = 400;
        private long lastShoot = long.MinValue;

        public Missile Shoot(long timeStamp, int x, int y)
        {
            if (timeStamp > lastShoot + ShootCooldown)
            {
                this.Differences.Add(new PlayerShootDifference(UniqueID, x, y));
                return new Missile(this, new Point(Location.X, Location.Y), new Point(x, y));
            }

            return null;
        }

        public Location GetLocation()
        {
            return Location;
        }

        public virtual string GetName()
        {
            return Name;
        }

        public virtual bool IsPlayer()
        {
            return false;
        }

        public void TryHitByMissile(Missile missile)
        {
            if (missile.Source == this)
                return;

            if (missile.Source.Fraction == this.Fraction)
                return;

            Point missilePoint = missile.GetLocation();
            Point myPoint = new Point(this.Location.X, this.Location.Y);
            int distance = myPoint.DistanceFrom(missilePoint);

            if (distance <= HitRange)
            {
                missile.HitUnit(this);
            }
        }

        public void HitByMissile(Missile missile)
        {
            this.ActualStats.HitPoints -= missile.Damage;
            if (ActualStats.HitPoints <= 0)
                IsDead = true;

            if(!HittedBy.Contains(missile.Source))
                HittedBy.Add(missile.Source);

            this.Differences.Add(new ActualHPDifference(UniqueID, ActualStats.HitPoints));
        }

        public virtual void AddExperience(int xp) { }

        public void AddDifference(IUnitDifference difference)
        {
            this.Differences.Add(difference);
        }
    }
}
