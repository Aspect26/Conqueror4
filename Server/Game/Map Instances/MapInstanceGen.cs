namespace Server
{
    public static class MapInstanceGen
    {
        public static void GenerateUnits(MapInstance map, int mapId)
        {
            switch (mapId)
            {
                default:
                    Map1(map); break;
            }
        }

        /*****************************************************************/
        // SPECIFIC MAPS
        /*****************************************************************/
        private static void Map1(MapInstance map)
        {
            map.CreateNPC(1, 120, 120);
        }
    }
}
