using Shared;
using System;

namespace Client
{
    public class Equip
    {
        public IItem[] Items { get; protected set; }

        public Equip()
        {
            this.Items = new IItem[SharedData.ITEM_SLOTS];
        }

        public void SetWeapon(ItemStats stats)
        {
            Items[SharedData.ITEM_SLOT_WEAPON] = new Item(stats, ItemType.WEAPON);
        }

        public void SetChest(ItemStats stats)
        {
            Items[SharedData.ITEM_SLOT_CHEST] = new Item(stats, ItemType.CHEST);
        }

        public void SetHead(ItemStats stats)
        {
            Items[SharedData.ITEM_SLOT_HEAD] = new Item(stats, ItemType.HEAD);
        }

        public void SetPants(ItemStats stats)
        {
            Items[SharedData.ITEM_SLOT_PANTS] = new Item(stats, ItemType.PANTS);
        }

        public static Equip ParseEquip(string data)
        {
            string[] itemsData = data.Split('+');
            if (itemsData[0] != "E")
                return null;

            Equip equip = new Equip();
            for(int i=1; i<itemsData.Length; i++)
            {
                string[] itemParts = itemsData[i].Split('&');
                int slot = Convert.ToInt32(itemParts[0]);
                switch (slot)
                {
                    case SharedData.ITEM_SLOT_WEAPON:
                        equip.SetWeapon(ParseStats(itemParts)); break;
                    case SharedData.ITEM_SLOT_CHEST:
                        equip.SetChest(ParseStats(itemParts)); break;
                    case SharedData.ITEM_SLOT_HEAD:
                        equip.SetHead(ParseStats(itemParts)); break;
                    case SharedData.ITEM_SLOT_PANTS:
                        equip.SetPants(ParseStats(itemParts)); break;
                    default: throw new NotImplementedException("Unknown item type from server: " + itemParts[0] + ".");
                }
            }

            return equip;
        }

        // TODO: this code is duplicit -> it is also in item parsing!
        public static ItemStats ParseStats(string[] data)
        {
            ItemStats stats = new ItemStats();

            for(int i = 1; i < data.Length; i++)
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
