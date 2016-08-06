using Shared;

namespace Server
{
    public partial class Character : GenericUnit
    {
        public int Spec { get { return UnitID; } set { this.UnitID = value; } }
        public int Experience { get; protected set; }
        public IQuest CurrentQuest { get; protected set; }

        public Character(string name, int spec, int uid, Location location, MapInstance map) 
            : base(name, spec, uid, location, map, Data.GetCharacterBaseStats(spec, 1))
        {
            this.Level = 1;
            this.Experience = 0;
            this.CurrentQuest = Data.GetInitialQuest(spec);
        }

        public override void PlayCycle(int timeSpan)
        {
        }

        public void SetQuest(IQuest quest)
        {
            this.CurrentQuest = quest;
        }

        public override void AddExperience(int xp)
        {
            this.Experience += xp;
            if(!(Experience >= SharedData.GetNextLevelXPRequired(Level)))
            {
                this.Differences.Add(new ExperienceDifference(UniqueID, Experience));
            }
            else
            {
                this.Experience = Experience % SharedData.GetNextLevelXPRequired(Level);
                this.Level++;
                this.Differences.Add(new ExperienceDifference(UniqueID, Experience));
            }
        }

        public override bool IsPlayer()
        {
            return true;
        }
    }
}
