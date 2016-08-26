using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Server
{
    /// <summary>
    /// Base implementation of the IUnit interface. The implementation is sufficient 
    /// enough for basic NPC units.
    /// </summary>
    /// <seealso cref="Server.IUnit" />
    public class GenericUnit : IUnit
    {
        /// <summary>
        /// Gets the unit identifier.
        /// This identifier identifies which type of a unit this is. For character
        /// players it determines its specialization.
        /// </summary>
        /// <value>The unit identifier.</value>
        public int UnitID { get; protected set; }

        /// <summary>
        /// Gets the unique identifier.
        /// This is a unique iddentifier for a unit. These identifiers are uniue in the
        /// map instance scope.
        /// </summary>
        /// <value>The unique identifier.</value>
        public int UniqueID { get; protected set; }

        /// <summary>
        /// Gets the maximum base stats stats.
        /// </summary>
        /// <value>The maximum stats.</value>
        public BaseStats MaxStats { get; protected set; }

        /// <summary>
        /// Gets the actual stats of a unit.
        /// </summary>
        /// <value>The actual stats.</value>
        public BaseStats ActualStats { get; protected set; }

        /// <summary>
        /// Gets the level of the unit.
        /// </summary>
        /// <value>The level.</value>
        public int Level { get; protected set; }

        /// <summary>
        /// Gets the respawn time.
        /// </summary>
        /// <value>The respawn time.</value>
        public int RespawnTime { get { return 120; } }

        /// <summary>
        /// Gets the fraction.
        /// </summary>
        /// <value>The fraction.</value>
        public int Fraction { get; }

        /// <summary>
        /// Gets the hit range.
        /// </summary>
        /// <value>The hit range.</value>
        public int HitRange { get; protected set; }

        /// <summary>
        /// Gets the list of units that hitted this unit in the current combat.
        /// </summary>
        /// <value>The list of units.</value>
        public List<IUnit> HittedBy { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether this unit is dead.
        /// </summary>
        /// <value><c>true</c> if this unit is dead; otherwise, <c>false</c>.</value>
        public bool IsDead { get; protected set; }

        /// <summary>
        /// Gets the map instance in which this unit is located.
        /// </summary>
        /// <value>The map instance.</value>
        public MapInstance MapInstance { get; protected set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        public Location Location { get; set; }

        /// <summary>
        /// Gets the unit's respawn position. It is not used for player's characters.
        /// </summary>
        /// <value>The spawn position.</value>
        public Point SpawnPosition { get; protected set; }

        /// <summary>
        /// Gets the moving direction of this unit.
        /// </summary>
        /// <value>The direction.</value>
        public MovingDirection Direction { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IUnit" /> moved
        /// since the last send update.
        /// </summary>
        /// <value><c>true</c> if moved; otherwise, <c>false</c>.</value>
        public bool Moved { get; set; }

        /// <summary>
        /// Gets the unit's differences between now and the last send update.
        /// </summary>
        /// <value>The differences.</value>
        public List<IUnitDifference> Differences { get; protected set; }

        /// <summary>
        /// Gets the list of units this unit is currently 'visiting' (are close enough).
        /// </summary>
        /// <value>The currently visited.</value>
        public List<IUnit> CurrentlyVisited { get; protected set; }

        /// <summary>
        /// Gets the list of units this unit is in combat with.
        /// </summary>
        /// <value>The list of units.</value>
        public List<IUnit> InCombatWith { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether something about this unit changed
        /// between now and last save to SQL database.
        /// </summary>
        /// <value><c>true</c> if [SQL difference]; otherwise, <c>false</c>.</value>
        public bool SQLDifference { get; set; }

        protected int movingSpeed;
        protected int shootCooldown;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericUnit"/> class.
        /// </summary>
        /// <param name="unitID">The unit identifier.</param>
        /// <param name="uniqueId">The unique identifier.</param>
        /// <param name="location">The location.</param>
        /// <param name="mapInstance">The map instance.</param>
        /// <param name="data">The data.</param>
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
            this.SQLDifference = false;

            Direction = MovingDirection.None;
            movingSpeed = 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericUnit"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="unitId">The unit identifier.</param>
        /// <param name="uniqueId">The unique identifier.</param>
        /// <param name="location">The location.</param>
        /// <param name="mapInstance">The map instance.</param>
        /// <param name="data">The data.</param>
        public GenericUnit(string name, int unitId, int uniqueId, Location location, MapInstance mapInstance, 
            InitialData data) 
            : this(unitId, uniqueId, location, mapInstance, data)
        {
            this.Name = name;
        }

        /// <summary>
        /// Resets the actual stats so they have value of the maximal possible stats.
        /// </summary>
        public void ResetStats()
        {
            ActualStats.HitPoints = GetMaxHitPoints();
        }

        /// <summary>
        /// Sets the unique identifier.
        /// </summary>
        /// <param name="uniqueId">The unique identifier.</param>
        public void SetUniqueID(int uniqueId)
        {
            this.UniqueID = uniqueId;
        }

        /// <summary>
        /// Leaves the combat with specified unit.
        /// </summary>
        /// <param name="unit">The unit to leave combat with.</param>
        public virtual void LeaveCombatWith(IUnit unit)
        {
            InCombatWith.Remove(unit);
        }

        /// <summary>
        /// Enters the combat with a spcified unit.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public virtual void EnterCombatWith(IUnit unit)
        {
            InCombatWith.Add(unit);
        }

        /// <summary>
        /// Plays one game cycle. 
        /// </summary>
        /// <param name="timeSpan">The time span.</param>
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
                        this.AddDifference(new ActualHPDifference(UniqueID, GetActualHitPoints()));
                    }
                }
            }
        }

        private long lastShoot = long.MinValue;

        /// <summary>
        /// Shoots the specified time stamp.
        /// </summary>
        /// <param name="timeStamp">The time stamp.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>Missile.</returns>
        public Missile Shoot(long timeStamp, int x, int y)
        {
            if (timeStamp > lastShoot + shootCooldown)
            {
                lastShoot = timeStamp;
                this.AddDifference(new PlayerShootDifference(UniqueID, x, y));
                return new Missile(this, new Point(Location.X, Location.Y), new Point(x, y));
            }

            return null;
        }

        /// <summary>
        /// Gets the unit's current location.
        /// </summary>
        /// <returns>Location.</returns>
        public Location GetLocation()
        {
            return Location;
        }

        /// <summary>
        /// Gets the unit's name.
        /// </summary>
        /// <returns>System.String.</returns>
        public virtual string GetName()
        {
            return Name;
        }

        /// <summary>
        /// Determines whether this unit is a player.
        /// </summary>
        /// <returns><c>true</c> if this unit is aa player; otherwise, <c>false</c>.</returns>
        public virtual bool IsPlayer()
        {
            return false;
        }

        /// <summary>
        /// Tries the hit the unit by missile. Checks whether this unit is close
        /// enough to the specified missile so it can be hit (it's not like the unit
        /// wants that but it must be done :) ).
        /// </summary>
        /// <param name="missile">The missile.</param>
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

        /// <summary>
        /// Hits the unit by a missile.
        /// </summary>
        /// <param name="missile">The missile.</param>
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

            this.AddDifference(new ActualHPDifference(UniqueID, GetActualHitPoints()));
        }

        /// <summary>
        /// Adds experience to the unit (only applied to player characters).
        /// </summary>
        /// <param name="xp">The xp.</param>
        public virtual void AddExperience(int xp) { }

        /// <summary>
        /// Adds a new difference between last send update and now.
        /// </summary>
        /// <param name="difference">The difference.</param>
        public void AddDifference(IUnitDifference difference)
        {
            this.SQLDifference = true;
            this.Differences.Add(difference);
        }

        /// <summary>
        /// This function is called when this unit dies (and it must be a non player
        /// unit). It generates a item that this unit droppes after its death.
        /// </summary>
        /// <returns>The item.</returns>
        public IItem GenerateDroppedItem()
        {
            return Data.GenerateItemDropped(this);
        }

        /// <summary>
        /// Gets the actual hit points.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public virtual int GetActualHitPoints()
        {
            return ActualStats.HitPoints;
        }

        /// <summary>
        /// Gets the maximum hit points. These hit points are influenced by character's
        /// equip. If it is an NPC unit it returns the MaxStat.HitPoints value.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public virtual int GetMaxHitPoints()
        {
            return MaxStats.HitPoints;
        }

        /// <summary>
        /// Gets the damage. The return value is influenced bu character's equip.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public virtual int GetDamage()
        {
            return this.ActualStats.Damage;
        }

        /// <summary>
        /// Gets the maximum mana points. These mana points are influenced by character's
        /// equip. If it is an NPC unit it returns the MaxStat.ManaPoints value.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public virtual int GetMaxManaPoints()
        {
            return MaxStats.ManaPoints;
        }

        /// <summary>
        /// Gets the actual mana points.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public virtual int GetActualManaPoints()
        {
            return ActualStats.ManaPoints;
        }

        /// <summary>
        /// Gets the armor. The return value is influenced bu character's equip.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public virtual int GetArmor()
        {
            return ActualStats.Armor;
        }

        /// <summary>
        /// Gets the spell bonus. The return value is influenced bu character's equip.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public virtual int GetSpellBonus()
        {
            return ActualStats.SpellBonus;
        }

        /// <summary>
        /// Heals the unit by specified amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public virtual void Heal(int amount)
        {
            int actual = this.GetActualHitPoints();
            int max = this.GetMaxHitPoints();
            if (actual == max)
                return;

            amount = (max - actual < amount) ? max - actual : amount;
            this.ActualStats.HitPoints += amount;
            this.AddDifference(new ActualHPDifference(this.UniqueID, GetActualHitPoints()));
        }

        /// <summary>
        /// Decreases the actual mana points by specified value.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public void DecreaseActualManaPoints(int amount)
        {
            this.ActualStats.ManaPoints -= amount;
            this.AddDifference(new ActualMPDifference(this.UniqueID, GetActualManaPoints()));
        }
    }
}
