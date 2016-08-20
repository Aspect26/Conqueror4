using Shared;
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
