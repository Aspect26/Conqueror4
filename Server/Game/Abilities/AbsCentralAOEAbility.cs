using System.Drawing;

namespace Server
{
    /// <summary>
    /// Base abstract class for every AOE (Area of Effect) type ability. It takes care
    /// of deciding which unit shall be hitted. Abilities that inherit from this class
    /// only need to specify the center of the AOE (by the unit that used the ability)
    /// and the radius (so it can only be a circular area). The new ability only needs 
    /// to implement the protected hitUnit function which defines what shall happen to 
    /// a unit that is hitted by the ability.
    /// </summary>
    /// <seealso cref="Server.Ability" />
    public abstract class CentralAOEAbility : Ability
    {
        protected int range;
        protected bool hitFriendly;
        protected Point center;

        /// <summary>
        /// Initializes a new instance of the <see cref="CentralAOEAbility"/> class.
        /// </summary>
        /// <param name="source">The source unit.</param>
        /// <param name="manaCost">The mana cost.</param>
        /// <param name="range">The range of the ability's effect.</param>
        /// <param name="hitFriendly">if set to <c>true</c> it hits friendly units,
        /// otherwise it hits enemy units.</param>
        public CentralAOEAbility(IUnit source, int manaCost, int range, bool hitFriendly = false)
            :base(source, manaCost)
        {
            this.range = range;
            this.hitFriendly = hitFriendly;
            this.center = new Point(source.GetLocation().X, source.GetLocation().Y);
        }

        /// <summary>
        /// Processes the ability. It determines (very unefectively :() which units in
        /// the map instance shall be hitted by the spell. For each unit that shall
        /// be unit the function then call the abstract hitUnit function.
        /// </summary>
        /// <param name="mapInstance">The map instance in which the ability
        /// was used.</param>
        public override void Process(MapInstance mapInstance)
        {
            base.Process(mapInstance);

            foreach(IUnit unit in mapInstance.GetUnits())
            {
                Point unitPoint = new Point(unit.GetLocation().X, unit.GetLocation().Y);
                int distance = unitPoint.DistanceFrom(center);
                if (distance > range)
                    continue;

                if(unit.Fraction != Source.Fraction && !hitFriendly)
                {
                    hitUnit(unit, distance);
                }
                else if (unit.Fraction == Source.Fraction && hitFriendly)
                {
                    hitUnit(unit, distance);
                }
            }
        }

        protected abstract void hitUnit(IUnit unit, int distance);

        /// <summary>
        /// Gets coded data for a server message.
        /// </summary>
        /// <returns>System.String.</returns>
        public override string GetCodedData()
        {
            return Source.GetLocation().X + "&" + Source.GetLocation().Y;
        }
    }
}
