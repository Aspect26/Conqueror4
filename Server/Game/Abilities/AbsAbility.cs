using System;

namespace Server
{
    public abstract class Ability : IAbility
    {
        public int ManaCost { get; }
        public abstract int ID { get; }

        public IUnit Source { get; }

        public Ability(IUnit source, int manaCost)
        {
            this.Source = source;
            this.ManaCost = manaCost;
        }

        public virtual void Process(MapInstance mapInstance)
        {
            Source.DecreaseActualManaPoints(ManaCost);
        }

        public abstract string GetCodedData();
    }
}
