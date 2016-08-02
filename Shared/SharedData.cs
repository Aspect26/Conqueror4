
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
    }
}
