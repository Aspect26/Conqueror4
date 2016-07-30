using Shared;

namespace Server
{
    public partial class Character : GenericUnit
    {
        public int Spec { get { return id; } set { this.id = value; } }
        public int Level { get; set; }

        public Character(string name, int spec, Location location) : base(name, spec, location)
        {
            this.Level = 1;
        }

        public override bool IsPlayer()
        {
            return true;
        }
    }
}
