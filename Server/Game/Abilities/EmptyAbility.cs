namespace Server
{
    public class EmptyAbility : Ability
    {
        public EmptyAbility(IUnit source)
            :base(source)
        {
            this.ManaCost = 25;
        }
    }
}
