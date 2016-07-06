using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class MapInstance
    {
        private List<StateObject> clientStates;
        private List<IUnit> units;
        private Queue<IPlayerAction> playerActions;
        private Queue<ISendAction> sendActions;

        private Stopwatch stopwatch;
        private const int PLAYERACTIONS_PROCESSED_IN_ONE_CYCLE = 5;

        private int mapId;
        private long lastMiliseconds;

        public MapInstance(int mapId, Queue<ISendAction> sendActions)
        {
            this.mapId = mapId;
            this.sendActions = sendActions;

            clientStates = new List<StateObject>();
            units = new List<IUnit>();
            playerActions = new Queue<IPlayerAction>();

            stopwatch = new Stopwatch();
            stopwatch.Start();
            lastMiliseconds = stopwatch.ElapsedMilliseconds;
        }
        
        public List<IUnit> GetUnits()
        {
            return this.units;
        }

        // THIS IS CALLED FROM RECEIVING TASK
        public void AddAction(IPlayerAction action)
        {
            lock (playerActions)
            {
                playerActions.Enqueue(action);
            }
        }

        // THIS IS CALLED FROM GAME TASK
        public void AddPlayer(StateObject state, Character character)
        {
            lock (clientStates)
            {
                clientStates.Add(state);
            }

            lock (units)
            {
                units.Add(character);
            }
        }

        // THIS IS CALLED FROM GAME TASK
        public void PlayCycle()
        {
            // process some of the plyers' actions
            for (int i = 0; i < PLAYERACTIONS_PROCESSED_IN_ONE_CYCLE; i++)
            {
                lock (playerActions)
                {
                    if (playerActions.Count != 0)
                    {
                        lock (sendActions)
                        {
                            sendActions.Enqueue(playerActions.Dequeue().Process());
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            // get timespan
            long timeSpan = stopwatch.ElapsedMilliseconds - lastMiliseconds;
            lastMiliseconds = stopwatch.ElapsedMilliseconds;

            // move all units
            foreach (IUnit unit in units)
            {
                unit.PlayCycle((int)timeSpan);
            }
        }
    }
}
