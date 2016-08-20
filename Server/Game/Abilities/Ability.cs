using System;

namespace Server
{
    public abstract class Ability : IAbility
    {
        public int ManaCost { get; protected set; }

        protected IUnit source;

        public Ability(IUnit source)
        {
            this.source = source;
        }

        public void Process(MapInstance mapInstance)
        {
            source.DecreaseActualManaPoints(ManaCost);
        }
    }
}
