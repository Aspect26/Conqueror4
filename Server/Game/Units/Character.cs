using Shared;
using System.Drawing;

namespace Server
{
    /// <summary>
    /// Class Character.
    /// </summary>
    /// <seealso cref="Server.GenericUnit" />
    public partial class Character : GenericUnit
    {
        /// <summary>
        /// Gets or sets the character's specialization.
        /// </summary>
        /// <value>The spec.</value>
        public int Spec { get { return UnitID; } set { this.UnitID = value; } }

        /// <summary>
        /// Gets or sets the experience.
        /// </summary>
        /// <value>The experience.</value>
        public int Experience { get; protected set; }

        /// <summary>
        /// Gets or sets the current quest.
        /// </summary>
        /// <value>The current quest.</value>
        public IQuest CurrentQuest { get; protected set; }

        /// <summary>
        /// Gets or sets the equip.
        /// </summary>
        /// <value>The equip.</value>
        public Equip Equip { get; protected set; }


        private long lastHealed;
        private long lastManaRegenerated;

        /// <summary>
        /// Initializes a new instance of the <see cref="Character"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="spec">The spec.</param>
        /// <param name="uid">The uid.</param>
        /// <param name="location">The location.</param>
        /// <param name="map">The map.</param>
        public Character(string name, int spec, int uid, Location location, MapInstance map) 
            : base(name, spec, uid, location, map, Data.GetCharacterBaseStats(spec, 1))
        {
            this.Level = 1;
            this.Experience = 0;
            this.CurrentQuest = Data.GetInitialQuest(spec);
            this.shootCooldown = 300;
            this.lastHealed = Extensions.GetCurrentMillis();
            this.lastManaRegenerated = Extensions.GetCurrentMillis();
            this.Equip = new Equip();
        }

        /// <summary>
        /// Plays one game cycle. This regenerates character's hit points as well
        /// as mana points.
        /// </summary>
        /// <param name="timeSpan">The time span.</param>
        public override void PlayCycle(int timeSpan)
        {
            // heal myself if not in combat
            if (IsDead)
                return;

            long now = Extensions.GetCurrentMillis();

            // not in combat
            if (InCombatWith.Count == 0)
            {
                // hp regen
                if (GetActualHitPoints() != GetMaxHitPoints()
                    && now - lastHealed >= Data.HPRegenInterval)
                {
                    regenHp(now);
                }
            }

            // in combat
            else
            {
                // mp regen
                if (GetActualManaPoints() != GetMaxManaPoints()
                    && now - lastManaRegenerated >= Data.MPRegenInterval)
                {
                    regenMp(now);
                }
            }
        }

        /// <summary>
        /// Sets the experience.
        /// </summary>
        /// <param name="xp">The xp.</param>
        public void SetExperience(int xp)
        {
            this.Experience = xp;
        }

        /// <summary>
        /// Sets the level.
        /// </summary>
        /// <param name="level">The level.</param>
        public void SetLevel(int level)
        {
            this.Level = level;
        }

        private void regenMp(long now)
        {
            lastManaRegenerated = now;
            int toRegen = 10;

            if (GetActualManaPoints() + toRegen > GetMaxManaPoints())
                ActualStats.ManaPoints = GetMaxManaPoints();
            else
                ActualStats.ManaPoints += toRegen;

            this.AddDifference(new ActualMPDifference(UniqueID, GetActualManaPoints()));
        }

        private void regenHp(long now)
        {
            lastHealed = now;
            int toRegen = GetMaxHitPoints() / 15;

            if (GetActualHitPoints() + toRegen > GetMaxHitPoints())
                ActualStats.HitPoints = GetMaxHitPoints();
            else
                ActualStats.HitPoints += toRegen;

            this.AddDifference(new ActualHPDifference(UniqueID, GetActualHitPoints()));
        }

        /// <summary>
        /// Gets the maximum hit points. These hit points are influenced by character's
        /// equip. If it is an NPC unit it returns the MaxStat.HitPoints value.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public override int GetMaxHitPoints()
        {
            return MaxStats.HitPoints + Equip.HitPoints;
        }

        /// <summary>
        /// Gets the maximum mana points. These mana points are influenced by character's
        /// equip. If it is an NPC unit it returns the MaxStat.ManaPoints value.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public override int GetMaxManaPoints()
        {
            return MaxStats.ManaPoints + Equip.ManaPoints;
        }

        /// <summary>
        /// Gets the damage. The return value is influenced bu character's equip.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public override int GetDamage()
        {
            return this.ActualStats.Damage + Equip.Damage;
        }

        /// <summary>
        /// Gets the armor. The return value is influenced bu character's equip.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public override int GetArmor()
        {
            return this.ActualStats.Armor + Equip.Armor;
        }

        /// <summary>
        /// Gets the spell bonus. The return value is influenced bu character's equip.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public override int GetSpellBonus()
        {
            return this.ActualStats.SpellBonus + Equip.SpellBonus;
        }

        /// <summary>
        /// Sets the map instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public void SetMapInstance(MapInstance instance)
        {
            this.MapInstance = instance;
        }

        /// <summary>
        /// Revives the specified location.
        /// </summary>
        /// <param name="location">The location.</param>
        public void Revive(Point location)
        {
            IsDead = false;

            // change location
            Location.X = location.X;
            Location.Y = location.Y;

            // reset q objectives
            CurrentQuest.Reset();

            // fill hp
            ResetStats();

            // set differences
            AddDifference(new UnitDiedDifference(this));
            AddDifference(new CharacterForceMovedAction(UniqueID, Location));
            AddDifference(new QuestObjectiveDifference(UniqueID, CurrentQuest));
            AddDifference(new ActualStatsDifference(this));
        }

        /// <summary>
        /// Leaves the combat with specified unit.
        /// </summary>
        /// <param name="unit">The unit to leave combat with.</param>
        public override void LeaveCombatWith(IUnit unit)
        {
            base.LeaveCombatWith(unit);
            this.lastHealed = Extensions.GetCurrentMillis();
        }

        /// <summary>
        /// Sets the quest.
        /// </summary>
        /// <param name="quest">The quest.</param>
        public void SetQuest(IQuest quest)
        {
            this.CurrentQuest = quest;
        }

        /// <summary>
        /// Sets the equip.
        /// </summary>
        /// <param name="equip">The equip.</param>
        public void SetEquip(Equip equip)
        {
            this.Equip = equip;
        }

        /// <summary>
        /// Adds experience to the character.
        /// </summary>
        /// <param name="xp">The xp.</param>
        public override void AddExperience(int xp)
        {
            this.Experience += xp;
            if(!(Experience >= Data.GetNextLevelXPRequired(Level)))
            {
                this.AddDifference(new ExperienceDifference(UniqueID, Experience));
            }
            else
            {
                this.Experience = Experience % Data.GetNextLevelXPRequired(Level);
                this.Level++;
                this.AddDifference(new ExperienceDifference(UniqueID, Experience));
                this.AddDifference(new LevelDifference(UniqueID, Level, Data.GetNextLevelXPRequired(Level)));
            }
        }

        /// <summary>
        /// Determines whether this unit is a player.
        /// </summary>
        /// <returns><c>true</c></returns>
        public override bool IsPlayer()
        {
            return true;
        }
    }
}
