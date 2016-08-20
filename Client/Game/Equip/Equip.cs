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

        public void SetWeapon(ItemStats stats, int uid)
        {
            Items[SharedData.ITEM_SLOT_WEAPON] = new Item(stats, SharedData.ITEM_SLOT_WEAPON, uid);
        }

        public void SetChest(ItemStats stats, int uid)
        {
            Items[SharedData.ITEM_SLOT_CHEST] = new Item(stats, SharedData.ITEM_SLOT_CHEST, uid);
        }

        public void SetHead(ItemStats stats, int uid)
        {
            Items[SharedData.ITEM_SLOT_HEAD] = new Item(stats, SharedData.ITEM_SLOT_HEAD, uid);
        }

        public void SetPants(ItemStats stats, int uid)
        {
            Items[SharedData.ITEM_SLOT_PANTS] = new Item(stats, SharedData.ITEM_SLOT_PANTS, uid);
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
