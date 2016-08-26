namespace Server
{
    /// <summary>
    /// An ability implementation that does nothing only reduces caster's mana points. 
    /// It is only used for character's specializations whose abilities are not 
    /// implemented yet.
    /// </summary>
    /// <seealso cref="Server.Ability" />
    public class EmptyAbility : Ability
    {
        /// <summary>
        /// Gets the identifier of the ability.
        /// </summary>
        /// <value>The identifier.</value>
        public override int ID { get { return -1; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyAbility"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        public EmptyAbility(IUnit source)
            :base(source, 15)
        {
        }

        /// <summary>
        /// Gets coded data for a server message.
        /// </summary>
        /// <returns>System.String.</returns>
        public override string GetCodedData()
        {
            return "";
        }
    }
}
