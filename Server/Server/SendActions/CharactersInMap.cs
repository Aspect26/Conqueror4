﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class CharactersInMap : ISendAction
    {
        MapInstance mapInstance;

        public CharactersInMap(MapInstance mapInstance)
        {
            this.mapInstance = mapInstance;
        }

        public void Send()
        {
            StringBuilder msg = new StringBuilder(SendCommands.MSG_CHARACTERS_IN_MAP + ":");

            foreach(IUnit unit in mapInstance.GetUnits())
            {
                Location loc = unit.GetLocation();
                msg.Append(unit.GetName() + "|" + unit.GetId() + "|" + loc.X + "|" + loc.Y + ",");
            }
        }
    }
}
