﻿using System;
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

        private long lastTimeStamp;
        private const int PLAYERACTIONS_PROCESSED_IN_ONE_CYCLE = 5;

        private int mapId;

        public MapInstance(int mapId, Queue<ISendAction> sendActions)
        {
            this.mapId = mapId;
            this.sendActions = sendActions;

            clientStates = new List<StateObject>();
            units = new List<IUnit>();
            playerActions = new Queue<IPlayerAction>();

            lastTimeStamp = Stopwatch.GetTimestamp();
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
            long now = Stopwatch.GetTimestamp();
            long timeSpan = (1000*(Stopwatch.GetTimestamp() - lastTimeStamp) / Stopwatch.Frequency);

            // process all players' actions
            lock (playerActions)
            {
                while (playerActions.Count != 0)
                {
                    playerActions.Dequeue().Process(now);
                    /*lock (sendActions)
                    {
                        sendActions.Enqueue(playerActions.Dequeue().Process());
                    }*/
                }
            }

            lastTimeStamp = now;
            if (timeSpan < 30)
                return;

            // move all units
            foreach (IUnit unit in units)
            {
                unit.PlayCycle((int)timeSpan);
            }
        }
    }
}
