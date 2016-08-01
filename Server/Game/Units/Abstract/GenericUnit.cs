using Shared;
using System.Collections.Generic;

namespace Server
{
    public class GenericUnit : IUnit
    {
        public int UnitID { get; protected set; }
        public int UniqueID { get; protected set; }

        public Location Location { get; set; }
        public MovingDirection Direction { get; private set; }
        public bool Updated { get; set; }
        public List<string> Differences { get; set; }

        protected int movingSpeed;
        protected const int SLOWING_CONSTANT = 5;

        public string Name { get; set; }

        public GenericUnit(int unitID, int uniqueId, Location location)
        {
            this.Location = location;
            this.UnitID = unitID;
            this.UniqueID = uniqueId;
            this.Name = "Unknown";
            this.Differences = new List<string>();

            Direction = MovingDirection.None;
            movingSpeed = 1;
        }

        public GenericUnit(string name, int unitId, int uniqueId, Location location) : this(unitId, uniqueId, location)
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
        public void Shoot(long timeSpan, int x, int y)
        {
            if (timeSpan > lastShoot + ShootCooldown)
            {
                this.Differences.Add("S&" + x + "&" + y);
            }
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
    }
}
