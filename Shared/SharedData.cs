
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

        public const int FRACTION_HUMAN_REALM = 1;
        public const int FRACTION_DEMON_KINGDOM = 2;
        public const int FRACTION_HOSTILE_UNITS = 3;
    }
}
