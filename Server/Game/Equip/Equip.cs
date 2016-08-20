using System.Text;

namespace Server
{
    public class Equip
    {
        public IItem Weapon { get; set; }
        public IItem Chest { get; set; }
        public IItem Head { get; set; }
        public IItem Pants { get; set; }

        public string GetCodedData()
        {
            StringBuilder str = new StringBuilder("E+");

            bool addPlus = false;
            if (Weapon != null)
            {
                str.Append("W").Append(Weapon.GetCodedData());
                addPlus = true;
            }
            if(Chest != null)
            {
                if (addPlus)
                    str.Append("+");

                str.Append("C").Append(Chest.GetCodedData());
                addPlus = true;
            }
            if (Head != null)
            {
                if (addPlus)
                    str.Append("+");

                str.Append("H").Append(Head.GetCodedData());
                addPlus = true;
            }
            if (Pants != null)
            {
                if (addPlus)
                    str.Append("+");

                str.Append("P").Append(Pants.GetCodedData());
                addPlus = true;
            }

            return str.ToString();
        }
    }
}
