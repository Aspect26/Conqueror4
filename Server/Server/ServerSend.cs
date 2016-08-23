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
        private long lastSQLSave = Extensions.GetCurrentMillis();

        public const int MSG_CHARACTERS_IN_MAP = 5;


        private void SendClients()
        {

            Console.WriteLine("Task for sending messages to clients started...");

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

                // check for sql save
                if(Extensions.GetCurrentMillis() > lastSQLSave + Data.SQLSaveInterval)
                {
                    Data.Save();
                    lastSQLSave = Extensions.GetCurrentMillis();
                }
            }
        }

        private void sendUpdate(MapInstance mapInstance, long delta)
        {
            lock (mapInstance)
            {
                StringBuilder msg = new StringBuilder(MSG_CHARACTERS_IN_MAP + ":");
                bool needSend = false;

                // add unit differences
                List<IUnit> units = mapInstance.GetUnits();
                lock (units)
                {
                    foreach (IUnit unit in units)
                    {
                        if (unit.Moved || unit.Differences.Count != 0)
                        {
                            msg.Append(unit.UniqueID);

                            if (unit.Moved)
                            {
                                // location TODO: move this to differences
                                Location loc = unit.GetLocation();
                                msg.Append("|L&" + loc.X + "&" + loc.Y);
                            }

                            // other actions
                            foreach (IUnitDifference diff in unit.Differences)
                            {
                                msg.Append("|" + diff.GetString());
                            }
                            // TODO: unit differences this way is bullshit
                            if (unit.Differences.Count != 0)
                            {
                                unit.Differences.Clear();
                            }

                            // finish
                            msg.Append(",");
                            unit.Moved = false;
                            needSend = true;
                        }
                    }

                    // add map general differences
                    List<IUnitDifference> unitDiffs = mapInstance.GetGeneralDifferencesAndReset();
                    lock (unitDiffs)
                    {
                        foreach (IUnitDifference diff in unitDiffs)
                        {
                            int uid = diff.UnitUID;
                            msg.Append(uid + "|" + diff.GetString());
                            msg.Append(",");
                            needSend = true;
                        }
                    }

                    msg.Append("\r\n");

                    if (!needSend)
                        return;

                    byte[] byteDate = Encoding.ASCII.GetBytes(msg.ToString());
                    var clients = mapInstance.GetClients();
                    lock (clients)
                    {
                        foreach (StateObject client in clients)
                        {
                            try
                            {
                                Console.WriteLine("Sending: " + msg);
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
    }
}
