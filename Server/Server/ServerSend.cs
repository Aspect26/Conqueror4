using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

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
                    sendUpdate(key.Value, timeSpan);
                }
            }
        }

        private void sendUpdate(MapInstance mapInstance, long delta)
        {
            lock (mapInstance)
            {
                StringBuilder msg = new StringBuilder(SendCommands.MSG_CHARACTERS_IN_MAP + ":");
                bool needSend = false;

                foreach (IUnit unit in mapInstance.GetUnits())
                {
                    if (unit.Updated || unit.Differences.Count != 0)
                    {
                        // location
                        Location loc = unit.GetLocation();
                        msg.Append(unit.GetName() + "|" + unit.GetId());

                        if (unit.Updated)
                        {
                            msg.Append("|L&" + loc.X + "&" + loc.Y);
                        }

                        // other actions
                        foreach(string action in unit.Differences)
                        {
                            msg.Append("|" + action);
                        }
                        // TODO: unit differences this way is bullshit
                        if (unit.Differences.Count != 0)
                        {
                            unit.Differences.Clear();
                        }
                        
                        // finish
                        msg.Append(",");
                        unit.Updated = false;
                        needSend = true;
                    }
                }
                msg.Append("\r\n");

                if (!needSend)
                    return;

                byte[] byteDate = Encoding.ASCII.GetBytes(msg.ToString());
                foreach (StateObject client in mapInstance.GetClients())
                {
                    try
                    {
                        Console.WriteLine("Sending: " + msg + ", " + delta);
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
