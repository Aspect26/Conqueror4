using System;

namespace Server
{
    public class EmptyAbility : Ability
    {
        public override int ID { get { return -1; } }

        public EmptyAbility(IUnit source)
            :base(source, 15)
        {
        }

        public override string GetCodedData()
        {
            return "";
        }
    }
}
