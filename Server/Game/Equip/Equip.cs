using Shared;
using System.Linq;
using System.Text;

namespace Server
{
    /// <summary>
    /// Represents an  equip (list of items that some specific character is curently 
    /// wearing).
    /// </summary>
    public class Equip
    {
        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        public IItem[] Items { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Equip"/> class with no items.
        /// </summary>
        public Equip()
        {
            Items = new IItem[SharedData.ITEM_SLOTS];
        }

        /// <summary>
        /// Sets a new item in the specified slot.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="slot">The slot.</param>
        public void SetItem(IItem item, int slot)
        {
            Items[slot] = item;
        }

        /// <summary>
        /// Gets the hit points bonus provided by all the worn items.
        /// </summary>
        /// <value>The hit points.</value>
        public int HitPoints
        {
            get { return Items.Sum(i => (i != null) ? i.Stats.HitPoints : 0 ); }
        }

        /// <summary>
        /// Gets the mana points bonus provided by all the worn items.
        /// </summary>
        /// <value>The mana points.</value>
        public int ManaPoints
        {
            get { return Items.Sum(i => (i != null) ? i.Stats.ManaPoints : 0); }
        }

        /// <summary>
        /// Gets the armor bonus provided by all the worn items.
        /// </summary>
        /// <value>The armor.</value>
        public int Armor
        {
            get { return Items.Sum(i => (i != null) ? i.Stats.Armor : 0); }
        }

        /// <summary>
        /// Gets the damage bonus provided by all the worn items.
        /// </summary>
        /// <value>The damage.</value>
        public int Damage
        {
            get { return Items.Sum(i => (i != null) ? i.Stats.Damage : 0); }
        }

        /// <summary>
        /// Gets the spell bonus bonus provided by all the worn items.
        /// </summary>
        /// <value>The spell bonus.</value>
        public int SpellBonus
        {
            get { return Items.Sum(i => (i != null) ? i.Stats.SpellBonus : 0); }
        }

        /// <summary>
        /// Gets the coded data for a server message.
        /// </summary>
        /// <returns>System.String.</returns>
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
