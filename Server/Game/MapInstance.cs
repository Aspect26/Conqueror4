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

        private DateTime lastTime;
        private const int PLAYERACTIONS_PROCESSED_IN_ONE_CYCLE = 5;

        private int mapId;

        public MapInstance(int mapId, Queue<ISendAction> sendActions)
        {
            this.mapId = mapId;
            this.sendActions = sendActions;

            clientStates = new List<StateObject>();
            units = new List<IUnit>();
            playerActions = new Queue<IPlayerAction>();

            lastTime = DateTime.Now;
        }
        
        public List<IUnit> GetUnits()
        {
            return this.units;
        }

        public List<StateObject> GetClients()
        {
            return clientStates;
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
            // process some of the players' actions
            lock (playerActions)
            {
                while (playerActions.Count != 0)
                {
                    playerActions.Dequeue().Process();
                    /*lock (sendActions)
                    {
                        sendActions.Enqueue(playerActions.Dequeue().Process());
                    }*/
                }
            }

            // get timespan
            long timeSpan = (int)(DateTime.Now - lastTime).TotalMilliseconds;
            if (timeSpan < 30)
                return;

            lastTime = DateTime.Now;

            // move all units
            foreach (IUnit unit in units)
            {
                unit.PlayCycle((int)timeSpan);
            }
        }
    }
}
