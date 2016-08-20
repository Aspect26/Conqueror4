using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Server
{
    public class GenericUnit : IUnit
    {
        public int UnitID { get; protected set; }
        public int UniqueID { get; protected set; }

        public BaseStats MaxStats { get; protected set; }
        public BaseStats ActualStats { get; protected set; }
        public int Level { get; protected set; }

        public int RespawnTime { get { return 120; } }
        public int Fraction { get; }

        public int HitRange { get; protected set; }
        public List<IUnit> HittedBy { get; protected set; }
        public bool IsDead { get; protected set; }

        public MapInstance MapInstance { get; protected set; }
        public Location Location { get; set; }
        public Point SpawnPosition { get; protected set; }
        public MovingDirection Direction { get; private set; }
        public bool Moved { get; set; }
        public List<IUnitDifference> Differences { get; protected set; }
        public List<IUnit> CurrentlyVisited { get; protected set; }
        public List<IUnit> InCombatWith { get; protected set; }

        protected int movingSpeed;
        protected int shootCooldown;

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
            this.CurrentlyVisited = new List<IUnit>();
            this.InCombatWith = new List<IUnit>();
            this.IsDead = false;
            this.MaxStats = data.MaxStats.Copy();
            this.ActualStats = data.MaxStats.Copy();
            this.Level = data.Level;
            this.Fraction = data.Fraction;
            this.shootCooldown = 800;
            this.SpawnPosition = new Point(location.X, location.Y);

            Direction = MovingDirection.None;
            movingSpeed = 1;
        }

        public GenericUnit(string name, int unitId, int uniqueId, Location location, MapInstance mapInstance, 
            InitialData data) 
            : this(unitId, uniqueId, location, mapInstance, data)
        {
            this.Name = name;
        }

        public void ResetStats()
        {
            ActualStats.HitPoints = GetMaxHitPoints();
        }

        public void SetUniqueID(int uniqueId)
        {
            this.UniqueID = uniqueId;
        }

        public virtual void LeaveCombatWith(IUnit unit)
        {
            InCombatWith.Remove(unit);
        }

        public virtual void EnterCombatWith(IUnit unit)
        {
            InCombatWith.Add(unit);
        }

        public virtual void PlayCycle(int timeSpan)
        {
            if(InCombatWith.Count > 0)
            {
                // shoot at combat target
                Point targetLocation = new Point(InCombatWith[0].GetLocation().X, InCombatWith[0].GetLocation().Y);
                int x = targetLocation.X - Location.X;
                int y = targetLocation.Y - Location.Y;
                double length = Math.Sqrt(x * x + y * y);

                int dirX = (int)((x / length) * 100);
                int dirY = (int)((y / length) * 100);

                Missile m = Shoot((Stopwatch.GetTimestamp() * 1000) / Stopwatch.Frequency, dirX, dirY);
                if (m != null)
                    MapInstance.AddMissile(m);

                // check if move to combat target
                int movePoints = timeSpan / SharedData.SlowingConstant;
                Point myPoint = new Point(Location.X, Location.Y);
                if (myPoint.DistanceFrom(targetLocation) < Data.CombatRange)
                    return;

                int moveX = (int)(movePoints * (x / length));
                int moveY = (int)(movePoints * (y / length));

                // move to combat target
                if ( myPoint.DistanceFrom(targetLocation) > Data.CombatRange)
                {
                    GetLocation().X += moveX;
                    GetLocation().Y += moveY;

                    Moved = true;
                }
            }
            else
            {
                // return to spawn location if needed
                Point myPoint = new Point(Location.X, Location.Y);
                if (myPoint.DistanceFrom(SpawnPosition) > 10)
                {
                    int x = SpawnPosition.X - Location.X;
                    int y = SpawnPosition.Y - Location.Y;
                    double length = Math.Sqrt(x * x + y * y);

                    int dirX = (int)((x / length) * 100);
                    int dirY = (int)((y / length) * 100);

                    int movePoints = timeSpan / SharedData.SlowingConstant;
                    int moveX = (int)(movePoints * (x / length));
                    int moveY = (int)(movePoints * (y / length));

                    GetLocation().X += moveX;
                    GetLocation().Y += moveY;
                    myPoint = new Point(Location.X, Location.Y);

                    Moved = true;

                    if (myPoint.DistanceFrom(SpawnPosition) <= 10)
                    {
                        this.ActualStats.HitPoints = GetMaxHitPoints();
                        this.Differences.Add(new ActualHPDifference(UniqueID, GetActualHitPoints()));
                    }
                }
            }
        }

        private long lastShoot = long.MinValue;

        public Missile Shoot(long timeStamp, int x, int y)
        {
            if (timeStamp > lastShoot + shootCooldown)
            {
                lastShoot = timeStamp;
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

            if (missile.IsDead)
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
            int damage = missile.Damage - GetArmor();
            damage = (damage < 0) ? 0 : damage;

            this.ActualStats.HitPoints -= damage;
            if (GetActualHitPoints() <= 0)
            {
                missile.Source.InCombatWith.Remove(this);
                this.IsDead = true;
            }

            if(!HittedBy.Contains(missile.Source))
                HittedBy.Add(missile.Source);

            this.Differences.Add(new ActualHPDifference(UniqueID, GetActualHitPoints()));
        }

        public virtual void AddExperience(int xp) { }

        public void AddDifference(IUnitDifference difference)
        {
            this.Differences.Add(difference);
        }

        public IItem GetDroppedItem()
        {
            return Data.GenerateItemDropped(this);
        }

        public virtual int GetActualHitPoints()
        {
            return ActualStats.HitPoints;
        }

        public virtual int GetMaxHitPoints()
        {
            return MaxStats.HitPoints;
        }

        public virtual int GetDamage()
        {
            return this.ActualStats.Damage;
        }

        public virtual int GetMaxManaPoints()
        {
            return MaxStats.ManaPoints;
        }

        public virtual int GetActualManaPoints()
        {
            return ActualStats.ManaPoints;
        }

        public virtual int GetArmor()
        {
            return ActualStats.Armor;
        }
    }
}
