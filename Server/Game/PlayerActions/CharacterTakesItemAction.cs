namespace Server
{
    /// <summary>
    /// Represents a player's action of trying to take an item that dropped from some
    /// creature.
    /// </summary>
    /// <seealso cref="Server.IPlayerAction" />
    public class CharacterTakesItemAction : IPlayerAction
    {
        private Character character;
        private int itemUid;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterTakesItemAction"/> class.
        /// </summary>
        /// <param name="character">The character.</param>
        /// <param name="uid">The uid.</param>
        public CharacterTakesItemAction(Character character, int uid)
        {
            this.character = character;
            this.itemUid = uid;
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
        /// Gets the item from the map instance (by uid) and if it still exists,
        /// it equips it to the player's corresponding equip slot.
        /// </summary>
        /// <param name="mapInstance">The map instance in which it happened.</param>
        /// <param name="timeStamp">The time stamp.</param>
        public void Process(MapInstance mapInstance, long timeStamp)
        {
            IItem item = mapInstance.GetDroppedItem(itemUid);
            if (item != null)
            {
                character.Equip.Items[item.Slot] = item;
                character.AddDifference(new ItemEquipedDifference(character, item));
                character.AddDifference(new MaxStatsDifference(character));
            }
        }
    }
}
