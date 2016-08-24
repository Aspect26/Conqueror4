using Shared;
using System;

namespace Client
{
    /// <summary>
    /// Represents a unit's equip. It currently consists of 4 items: Weapon, Chest, Head and Pants.
    /// </summary>
    public class Equip
    {
        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        public IItem[] Items { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Equip"/> class.
        /// </summary>
        public Equip()
        {
            this.Items = new IItem[SharedData.ITEM_SLOTS];
        }

        /// <summary>
        /// Sets the weapon slot.
        /// </summary>
        /// <param name="stats">The stats.</param>
        /// <param name="uid">The uid.</param>
        public void SetWeapon(ItemStats stats, int uid)
        {
            Items[SharedData.ITEM_SLOT_WEAPON] = new Item(stats, SharedData.ITEM_SLOT_WEAPON, uid);
        }

        /// <summary>
        /// Sets the chest slot.
        /// </summary>
        /// <param name="stats">The stats.</param>
        /// <param name="uid">The uid.</param>
        public void SetChest(ItemStats stats, int uid)
        {
            Items[SharedData.ITEM_SLOT_CHEST] = new Item(stats, SharedData.ITEM_SLOT_CHEST, uid);
        }

        /// <summary>
        /// Sets the head slot.
        /// </summary>
        /// <param name="stats">The stats.</param>
        /// <param name="uid">The uid.</param>
        public void SetHead(ItemStats stats, int uid)
        {
            Items[SharedData.ITEM_SLOT_HEAD] = new Item(stats, SharedData.ITEM_SLOT_HEAD, uid);
        }

        /// <summary>
        /// Sets the pants slot.
        /// </summary>
        /// <param name="stats">The stats.</param>
        /// <param name="uid">The uid.</param>
        public void SetPants(ItemStats stats, int uid)
        {
            Items[SharedData.ITEM_SLOT_PANTS] = new Item(stats, SharedData.ITEM_SLOT_PANTS, uid);
        }

        /// <summary>
        /// Parses the equip from server message.
        /// </summary>
        /// <param name="data">The server message.</param>
        /// <returns>Parsed equip.</returns>
        /// <exception cref="NotImplementedException">Unknown item type from server.</exception>
        public static Equip ParseEquip(string data)
        {
            string[] itemsData = data.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
            if (itemsData[0] != "E")
                return null;

            Equip equip = new Equip();
            for(int i=1; i<itemsData.Length; i++)
            {
                string[] itemParts = itemsData[i].Split('&');
                int slot = Convert.ToInt32(itemParts[0]);
                int uid = Convert.ToInt32(itemParts[1]);
                switch (slot)
                {
                    case SharedData.ITEM_SLOT_WEAPON:
                        equip.SetWeapon(ParseStats(itemParts), uid); break;
                    case SharedData.ITEM_SLOT_CHEST:
                        equip.SetChest(ParseStats(itemParts), uid); break;
                    case SharedData.ITEM_SLOT_HEAD:
                        equip.SetHead(ParseStats(itemParts), uid); break;
                    case SharedData.ITEM_SLOT_PANTS:
                        equip.SetPants(ParseStats(itemParts), uid); break;
                    default: throw new NotImplementedException("Unknown item type from server: " + itemParts[0] + ".");
                }
            }

            return equip;
        }

        // TODO: this code is duplicit -> it is also in item parsing!
        /// <summary>
        /// Parses the item stats from server message.
        /// </summary>
        /// <param name="data">The splitted server message.</param>
        /// <returns>ItemStats.</returns>
        public static ItemStats ParseStats(string[] data)
        {
            ItemStats stats = new ItemStats();

            for(int i = 2; i < data.Length; i++)
            {
                string[] datum = data[i].Split('^');

                int amount = Convert.ToInt32(datum[1]);
                switch (datum[0])
                {
                    case "H":
                        stats.HitPoints = amount; break;
                    case "M":
                        stats.ManaPoints = amount; break;
                    case "A":
                        stats.Armor = amount; break;
                    case "D":
                        stats.Damage = amount; break;
                    case "S":
                        stats.SpellBonus = amount; break;
                }
            }

            return stats;
        }
    }
}
