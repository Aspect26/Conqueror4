using Shared;
using System.Collections.Generic;

namespace Server
{
    public static class MapInstanceGen
    {
        private class UnitInfo
        {
            public UnitInfo(int ID, int X, int Y)
            {
                this.ID = ID;
                this.X = X;
                this.Y = Y;
            }

            public int ID { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
        }

        private static Dictionary<int, List<UnitInfo>> unitsData;

        static MapInstanceGen()
        {
            unitsData = new Dictionary<int, List<UnitInfo>>();

            // kingdom
            var units = new List<UnitInfo>();
            units.Add(new UnitInfo(SharedData.UNIT_WOLF, 3 * 50, 46 * 50));
            units.Add(new UnitInfo(SharedData.UNIT_WOLF, 10 * 50, 46 * 50));
            units.Add(new UnitInfo(SharedData.UNIT_WOLF, 12 * 50, 44 * 50));
            units.Add(new UnitInfo(SharedData.UNIT_WOLF, 7 * 50, 44 * 50));
            units.Add(new UnitInfo(SharedData.UNIT_WOLF, 5 * 50, 42 * 50));
            units.Add(new UnitInfo(SharedData.UNIT_WOLF, 10 * 50, 41 * 50));
            units.Add(new UnitInfo(SharedData.UNIT_WOLF, 3 * 50, 38 * 50));
            units.Add(new UnitInfo(SharedData.UNIT_WOLF, 7 * 50, 39 * 50));
            units.Add(new UnitInfo(SharedData.UNIT_WOLF, 10 * 50, 35 * 50));
            units.Add(new UnitInfo(SharedData.UNIT_WOLF, 6 * 50, 35 * 50));
            units.Add(new UnitInfo(SharedData.UNIT_WOLF, 3 * 50, 33 * 50));
            units.Add(new UnitInfo(SharedData.UNIT_WOLF, 8 * 50, 32 * 50));
            units.Add(new UnitInfo(SharedData.UNIT_WOLF, 6 * 50, 30 * 50));
            units.Add(new UnitInfo(SharedData.UNIT_WOLF, 10 * 50, 28  * 50));
            units.Add(new UnitInfo(SharedData.UNIT_WOLF, 3 * 50, 27 * 50));
            units.Add(new UnitInfo(SharedData.UNIT_WOLF, 5 * 50, 23 * 50));
            units.Add(new UnitInfo(SharedData.UNIT_WOLF, 7 * 50, 23 * 50));
            units.Add(new UnitInfo(SharedData.UNIT_WOLF_PACK_LEADER, 6 * 50, 22 * 50));
            units.Add(new UnitInfo(SharedData.UNIT_LIEUTENANT_LANDAX, 5 * 50, 57 * 50));

            units.Add(new UnitInfo(SharedData.UNIT_WARLOCK_SPAWNER, 7 * 50, 7 * 50));
            units.Add(new UnitInfo(SharedData.UNIT_WARLOCK_SPAWNER, 14 * 50, 3 * 50));
            units.Add(new UnitInfo(SharedData.UNIT_WARLOCK_SPAWNER, 15 * 50, 10 * 50));
            units.Add(new UnitInfo(SharedData.UNIT_WARLOCK_SPAWNER, 22 * 50, 6 * 50));
            units.Add(new UnitInfo(SharedData.UNIT_WARLOCK_SPAWNER, 27 * 50, 2 * 50));
            units.Add(new UnitInfo(SharedData.UNIT_WARLOCK_SPAWNER, 3 * 50, 2 * 50));

            units.Add(new UnitInfo(SharedData.UNIT_BERLOC_PYRESTEEL, 40 * 50, 58 * 50));
            units.Add(new UnitInfo(SharedData.UNIT_AWAKENED_SOUL, 42 * 50, 44 * 50));
            units.Add(new UnitInfo(SharedData.UNIT_AWAKENED_SOUL, 38 * 50, 38 * 50));
            units.Add(new UnitInfo(SharedData.UNIT_AWAKENED_SOUL, 45 * 50, 36 * 50));
            units.Add(new UnitInfo(SharedData.UNIT_AWAKENED_SOUL, 53 * 50, 42 * 50));
            units.Add(new UnitInfo(SharedData.UNIT_AWAKENED_SOUL, 43 * 50, 29 * 50));
            units.Add(new UnitInfo(SharedData.UNIT_AWAKENED_SOUL, 49 * 50, 31 * 50));
            units.Add(new UnitInfo(SharedData.UNIT_AWAKENED_SOUL, 55 * 50, 27 * 50));
            units.Add(new UnitInfo(SharedData.UNIT_AWAKENED_SOUL, 54 * 50, 37 * 50));
            units.Add(new UnitInfo(SharedData.UNIT_AWAKENED_SOUL, 49 * 50, 48 * 50));
            units.Add(new UnitInfo(SharedData.UNIT_AWAKENED_SOUL, 60 * 50, 33 * 50));
            units.Add(new UnitInfo(SharedData.UNIT_AWAKENED_SOUL, 59 * 50, 40 * 50));
            units.Add(new UnitInfo(SharedData.UNIT_AWAKENED_SOUL, 58 * 50, 47 * 50));

            unitsData.Add(SharedData.MAP_KINGDOM, units);
        }

        public static void GenerateUnits(MapInstance map, int mapId)
        {
            List<UnitInfo> units;
            unitsData.TryGetValue(mapId, out units);
            if(units != null)
            {
                foreach(UnitInfo unit in units)
                {
                    map.SpawnNPC(unit.ID, unit.X, unit.Y);
                }
            }
        }
    }
}
