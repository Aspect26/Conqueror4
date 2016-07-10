using System.Net.Sockets;
using System.Text;
using Shared;

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
            msg.Append("\r\n");

            byte[] byteDate = Encoding.ASCII.GetBytes(msg.ToString());
            foreach(StateObject client in mapInstance.GetClients())
            {
                client.clientSocket.Send(byteDate, 0, byteDate.Length, SocketFlags.None);
            }
        }
    }
}
