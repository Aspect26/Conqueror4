namespace Server
{
    /// <summary>
    /// Represents a characters action of shooting. 
    /// </summary>
    /// <seealso cref="Server.IPlayerAction" />
    public class CharacterShootAction : IPlayerAction
    {
        private Character character;
        private int x, y;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterShootAction"/> class.
        /// </summary>
        /// <param name="character">The character.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public CharacterShootAction(Character character, int x, int y)
        {
            this.character = character;
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Gets the character this action is associated with.
        /// </summary>
        /// <returns>Character.</returns>
        public Character GetCharacter()
        {
            return character;
        }

        /// <summary>
        /// Informs the map instance that the player tries to shoot.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="timeStamp">The time stamp.</param>
        public void Process(MapInstance instance, long timeStamp)
        {
            lock (character.MapInstance) 
                character.MapInstance.PlayerShoot(character, timeStamp, x, y);
        }
    }
}
