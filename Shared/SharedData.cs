﻿
namespace Shared
{
    public static class SharedData
    {
        public static int GetNextLevelXPRequired(int level)
        {
            switch (level)
            {
                case 1: return 250;
                case 2: return 360;
                default: return 2000000;
            }
        }

        public static string GetUnitName(int unitId)
        {
            switch (unitId)
            {
                case UNIT_WOLF: return "Wolf";
                case UNIT_WOLF_PACK_LEADER: return "Wolf Pack Leader";
                case UNIT_LIEUTENANT_LANDAX: return "Lieutenant Landax";

                default: return "Unknown";
            }
        }
        /*****************************/
        /* IDs                       */
        /*****************************/
        // fractions
        public const int FRACTION_HUMAN_REALM = 1;
        public const int FRACTION_DEMON_KINGDOM = 2;
        public const int FRACTION_HOSTILE_UNITS = 3;

        // maps
        public const int MAP_KINGDOM = 0;
        public const int MAP_FORTRESS = 1;

        // quests
        public const int QUEST_CALL_TO_ARMS = 1;
        public const int QUEST_WOLFPACK = 2;
        public const int QUEST_SLAY_THEIR_LEADER = 3;

        // units
        public const int UNIT_DEMONHUNTER = 1;
        public const int UNIT_MAGE = 2;
        public const int UNIT_PRIEST = 3;
        public const int UNIT_WARLOCK = 4;
        public const int UNIT_UNKHERO1 = 5;
        public const int UNIT_UNKHERO2 = 6;

        public const int UNIT_WOLF = 7;
        public const int UNIT_WOLF_PACK_LEADER = 8;
        public const int UNIT_LIEUTENANT_LANDAX = 9;
    }
}
