using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public partial class Server
    {
        private const int SEND_UPDATE_INTERVAL = 30;
        private void SendClients()
        {

            Console.WriteLine("Task for sending messages to clients started...");
            Console.WriteLine("-------------------------");

            long lastTimeStamp = Stopwatch.GetTimestamp();
            while (true)
            {
                long timeSpan = (1000 * (Stopwatch.GetTimestamp() - lastTimeStamp)) / Stopwatch.Frequency;
                if (timeSpan < SEND_UPDATE_INTERVAL)
                    continue;

                lastTimeStamp = Stopwatch.GetTimestamp();
                foreach (KeyValuePair<int, MapInstance> key in game.GetMapInstances())
                {
                    sendUpdate(key.Value);
                }
            }
        }

        private void sendUpdate(MapInstance mapInstance)
        {
            lock (mapInstance)
            {
                StringBuilder msg = new StringBuilder(SendCommands.MSG_CHARACTERS_IN_MAP + ":");

                foreach (IUnit unit in mapInstance.GetUnits())
                {
                    Location loc = unit.GetLocation();
                    msg.Append(unit.GetName() + "|" + unit.GetId() + "|" + loc.X + "|" + loc.Y + ",");
                }
                msg.Append("\r\n");

                byte[] byteDate = Encoding.ASCII.GetBytes(msg.ToString());
                foreach (StateObject client in mapInstance.GetClients())
                {
                    try
                    {
                        Console.WriteLine("Sendding: " + msg);
                        client.clientSocket.Send(byteDate, 0, byteDate.Length, SocketFlags.None);
                    }
                    catch (SocketException)
                    {
                        DisconnectClient(client);
                    }
                }
            }
        }
    }
}
