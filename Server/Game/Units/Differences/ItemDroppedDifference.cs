﻿using Shared;
using System;
using System.Text;

namespace Server
{
    public class ItemDroppedDifference : GenericDifference
    {
        private Location location;
        private IItem item;

        public ItemDroppedDifference(IItem item, IUnit unit) :
            base(unit.UniqueID)
        {
            this.location = unit.GetLocation();
            this.item = item;
        }

        public override string GetString()
        {
            StringBuilder msg = new StringBuilder();
            msg.Append("I&");
            msg.Append(location.X);
            msg.Append("&");
            msg.Append(location.Y);
            msg.Append("&");
            msg.Append(getTypeLetter(item.Type));

            msg.Append(item.GetCodedData());
            
            return msg.ToString();
        }

        private string getTypeLetter(ItemType type)
        {
            switch(type)
            {
                case ItemType.CHEST:
                    return "C";
                case ItemType.WEAPON:
                    return "W";
                case ItemType.PANTS:
                    return "P";
                case ItemType.HEAD:
                    return "H";
                default:
                    throw new NotImplementedException("Unimplemented ItemType -> String conversion for type: " +
                        type.ToString() + ".");
            }
        }
    }
}