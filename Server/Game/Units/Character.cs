using Shared;
using System.Drawing;

namespace Server
{
    public partial class Character : GenericUnit
    {
        public int Spec { get { return UnitID; } set { this.UnitID = value; } }
        public int Experience { get; protected set; }
        public IQuest CurrentQuest { get; protected set; }
        public Equip Equip { get; protected set; }

        private long lastHealed;
        private long lastManaRegenerated;

        public Character(string name, int spec, int uid, Location location, MapInstance map) 
            : base(name, spec, uid, location, map, Data.GetCharacterBaseStats(spec, 1))
        {
            this.Level = 1;
            this.Experience = 0;
            this.CurrentQuest = Data.GetInitialQuest(spec);
            this.shootCooldown = 300;
            this.lastHealed = Extensions.GetCurrentMillis();
            this.lastManaRegenerated = Extensions.GetCurrentMillis();

            // TODO: load from database
            this.Equip = new Equip();
        }

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

        public void SetExperience(int xp)
        {
            this.Experience = xp;
        }

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

        public override int GetMaxHitPoints()
        {
            return MaxStats.HitPoints + Equip.HitPoints;
        }

        public override int GetMaxManaPoints()
        {
            return MaxStats.ManaPoints + Equip.ManaPoints;
        }

        public override int GetDamage()
        {
            return this.ActualStats.Damage + Equip.Damage;
        }

        public override int GetArmor()
        {
            return this.ActualStats.Armor + Equip.Armor;
        }

        public override int GetSpellBonus()
        {
            return this.ActualStats.SpellBonus + Equip.SpellBonus;
        }

        public void SetMapInstance(MapInstance instance)
        {
            this.MapInstance = instance;
        }

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

        public override void LeaveCombatWith(IUnit unit)
        {
            base.LeaveCombatWith(unit);
            this.lastHealed = Extensions.GetCurrentMillis();
        }

        public void SetQuest(IQuest quest)
        {
            this.CurrentQuest = quest;
        }

        public void SetEquip(Equip equip)
        {
            this.Equip = equip;
        }

        public override void AddExperience(int xp)
        {
            this.Experience += xp;
            if(!(Experience >= SharedData.GetNextLevelXPRequired(Level)))
            {
                this.AddDifference(new ExperienceDifference(UniqueID, Experience));
            }
            else
            {
                this.Experience = Experience % SharedData.GetNextLevelXPRequired(Level);
                this.Level++;
                this.AddDifference(new ExperienceDifference(UniqueID, Experience));
            }
        }

        public override bool IsPlayer()
        {
            return true;
        }
    }
}
