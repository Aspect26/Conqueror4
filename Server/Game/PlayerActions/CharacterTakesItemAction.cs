using Shared;
using System;

namespace Server
{
    public class CharacterTakesItemAction : IPlayerAction
    {
        private Character character;
        private int itemUid;

        public CharacterTakesItemAction(Character character, int uid)
        {
            this.character = character;
            this.itemUid = uid;
        }

        public Character GetCharacter()
        {
            return character;
        }

        public void Process(MapInstance mapInstance, long timeStamp)
        {
            IItem item = mapInstance.GetDroppedItem(itemUid);
            if (item != null)
            {
                // TODO: remove item type totally .......... from server at least
                character.Equip.Items[item.Slot] = item;
                character.AddDifference(new ItemEquipedDifference(character, item));
                character.AddDifference(new MaxStatsDifference(character));
            }
        }
    }
}
