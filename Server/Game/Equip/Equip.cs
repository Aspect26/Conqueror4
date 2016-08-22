using Shared;
using System.Linq;
using System.Text;

namespace Server
{
    public class Equip
    {
        public IItem[] Items { get; set; }

        public Equip()
        {
            Items = new IItem[SharedData.ITEM_SLOTS];
        }

        public void SetItem(IItem item, int slot)
        {
            Items[slot] = item;
        }

        public int HitPoints
        {
            get { return Items.Sum(i => (i != null) ? i.Stats.HitPoints : 0 ); }
        }

        public int ManaPoints
        {
            get { return Items.Sum(i => (i != null) ? i.Stats.ManaPoints : 0); }
        }

        public int Armor
        {
            get { return Items.Sum(i => (i != null) ? i.Stats.Armor : 0); }
        }

        public int Damage
        {
            get { return Items.Sum(i => (i != null) ? i.Stats.Damage : 0); }
        }

        public int SpellBonus
        {
            get { return Items.Sum(i => (i != null) ? i.Stats.SpellBonus : 0); }
        }

        public string GetCodedData()
        {
            StringBuilder str = new StringBuilder("E+");

            bool addPlus = false;
            for(int i = 0; i < SharedData.ITEM_SLOTS; i++)
            {
                if(Items[i] != null)
                {
                    if (addPlus)
                        str.Append("+");

                    str.Append(Items[i].GetCodedData());
                    addPlus = true;
                }
            }

            return str.ToString();
        }
    }
}
