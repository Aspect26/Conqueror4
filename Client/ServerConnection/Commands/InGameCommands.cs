using Shared;

namespace Client
{
    public partial class ServerConnection
    {
        public void StartMovingCharacter(MovingDirection direction)
        {
            trySend(CMD_STARTMOVING, new string[] { directionToString(direction) });
        }

        public void StopMovingCharacter(MovingDirection direction)
        {
            trySend(CMD_STOPMOVING, new string[] { directionToString(direction) });
        }

        private string directionToString(MovingDirection direction)
        {
            switch (direction)
            {
                case MovingDirection.Up:
                    return "1";
                case MovingDirection.Right:
                    return "2";
                case MovingDirection.Bottom:
                    return "3";
                case MovingDirection.Left:
                    return "4";
                default:
                    return "9";
            }
        }
    }
}
