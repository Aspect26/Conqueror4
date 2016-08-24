using Shared;

namespace Client
{
    /// <summary>
    /// I have already written 2.5k lines of documentation today, and I think the 
    /// functions in this part of ServerConnection are self explanatory... :D
    /// See the other parts of the Commands folder.
    /// </summary>
    public partial class ServerConnection
    {
        public void SendPlayerShoot(int x, int y)
        {
            trySend(CMD_SHOOT, new string[] { x.ToString(), y.ToString() });
        }

        public void SendPlayerLocation(PlayedCharacter character)
        {
            trySend(CMD_CHANGELOCATION, new string[] { character.Location.X.ToString(), character.Location.Y.ToString() });
        }

        public void StartMovingCharacter(MovingDirection direction)
        {
            trySend(CMD_STARTMOVING, new string[] { directionToString(direction) });
        }

        public void StopMovingCharacter(MovingDirection direction)
        {
            trySend(CMD_STOPMOVING, new string[] { directionToString(direction) });
        }

        public void TakeItem(int uid)
        {
            trySend(CMD_TAKEITEM, new string[] { uid.ToString() });
        }

        public void TryUseAbility()
        {
            trySend(CMD_USEABILITY, new string[] { });
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
