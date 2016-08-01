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
            units.Add(new UnitInfo(WOLF, 3 * 50, 46 * 50));
            units.Add(new UnitInfo(WOLF, 10 * 50, 46 * 50));
            units.Add(new UnitInfo(WOLF, 12 * 50, 44 * 50));
            units.Add(new UnitInfo(WOLF, 7 * 50, 44 * 50));
            units.Add(new UnitInfo(WOLF, 5 * 50, 42 * 50));
            units.Add(new UnitInfo(WOLF, 10 * 50, 41 * 50));
            units.Add(new UnitInfo(WOLF, 3 * 50, 38 * 50));
            units.Add(new UnitInfo(WOLF, 7 * 50, 39 * 50));
            units.Add(new UnitInfo(WOLF, 10 * 50, 35 * 50));
            units.Add(new UnitInfo(WOLF, 6 * 50, 35 * 50));
            units.Add(new UnitInfo(WOLF, 3 * 50, 33 * 50));
            units.Add(new UnitInfo(WOLF, 8 * 50, 32 * 50));
            units.Add(new UnitInfo(WOLF, 6 * 50, 30 * 50));
            units.Add(new UnitInfo(WOLF, 10 * 50, 28  * 50));
            units.Add(new UnitInfo(WOLF, 3 * 50, 27 * 50));
            units.Add(new UnitInfo(WOLF, 5 * 50, 23 * 50));
            units.Add(new UnitInfo(WOLF, 7 * 50, 23 * 50));
            units.Add(new UnitInfo(WOLF_PACK_LEADER, 6 * 50, 22 * 50));
            unitsData.Add(Data.MAP_KINGDOM, units);
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

        // UNIT IDS
        private const int WOLF = 7;
        private const int WOLF_PACK_LEADER = 8;
    }
}
