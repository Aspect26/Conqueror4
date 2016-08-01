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

        public int HitRange { get; protected set; }
        public MapInstance MapInstance { get; protected set; }
        public Location Location { get; set; }
        public MovingDirection Direction { get; private set; }
        public bool Updated { get; set; }
        public List<IUnitDifference> Differences { get; protected set; }

        protected int movingSpeed;
        protected const int SLOWING_CONSTANT = 5;

        public string Name { get; set; }

        public GenericUnit(int unitID, int uniqueId, Location location, MapInstance mapInstance)
        {
            this.Location = location;
            this.UnitID = unitID;
            this.UniqueID = uniqueId;
            this.Name = "Unknown";
            this.MapInstance = mapInstance;
            this.HitRange = 30;
            this.Differences = new List<IUnitDifference>();

            Direction = MovingDirection.None;
            movingSpeed = 1;
        }

        public GenericUnit(string name, int unitId, int uniqueId, Location location, MapInstance mapInstance) 
            : this(unitId, uniqueId, location, mapInstance)
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
                this.Differences.Add(new PlayerShootDifference(x, y));
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
            if (missile.source == this)
                return;

            Point missilePoint = missile.GetLocation();
            Point myPoint = new Point(this.Location.X, this.Location.Y);
            int distance = myPoint.DistanceFrom(missilePoint);

            if (distance <= HitRange)
            {
                missile.HitUnit(this);
            }
        }
    }
}
