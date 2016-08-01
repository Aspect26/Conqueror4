using Shared;

namespace Server
{
    public partial class Character : GenericUnit
    {
        public int Spec { get { return UnitID; } set { this.UnitID = value; } }
        public int Level { get; set; }

        public Character(string name, int spec, int uid, Location location, MapInstance map) 
            : base(name, spec, uid, location, map)
        {
            this.Level = 1;
        }

        public override bool IsPlayer()
        {
            return true;
        }
    }
}
