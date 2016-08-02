using Shared;
using System.Collections.Generic;
using System.Diagnostics;

namespace Server
{
    public class MapInstance
    {
        private List<StateObject> clientStates;
        private List<IUnit> units;
        private Queue<IPlayerAction> playerActions;
        private List<Missile> missiles;
        private int lastUniqueId = 1;

        private long lastTimeStamp;
        private const int PLAYERACTIONS_PROCESSED_IN_ONE_CYCLE = 5;

        private int mapId;

        public MapInstance(int mapId)
        {
            this.mapId = mapId;
            this.missiles = new List<Missile>();

            clientStates = new List<StateObject>();
            units = new List<IUnit>();
            playerActions = new Queue<IPlayerAction>();

            lastTimeStamp = Stopwatch.GetTimestamp();
        }

        public void SpawnNPC(int unitId, int x, int y)
        {
            this.units.Add(new GenericUnit(unitId, GetNextUniqueID(), new Location(x, y), this, Data.GetBaseStats(unitId)));
        }

        public int GetNextUniqueID()
        {
            return lastUniqueId++;
        }

        public List<IUnit> GetUnits()
        {
            return this.units;
        }

        public List<StateObject> GetClients()
        {
            return clientStates;
        }

        // THIS IS CALLED FROM GAME TASK
        public void PlayerShoot(Character character, long timeStamp, int x, int y)
        {
            Missile missile = character.Shoot(timeStamp, x, y);
            if(missile != null)
            {
                this.missiles.Add(missile);
            }
        }

        // THIS IS CALLED FROM RECEIVING TASK
        public string GetMessageCodedData()
        {
            string data = "";

            foreach(IUnit unit in units)
            {
                data += unit.UniqueID + "|" + unit.UnitID + "|" + unit.GetName() + "|" 
                    + unit.GetLocation().X + "|" + unit.GetLocation().Y + "|" 
                    + unit.MaxStats.HitPoints + "|" + unit.ActualStats.HitPoints + ",";
            }

            return data;
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

            if (timeSpan < 30)
                return;
            lastTimeStamp = now;

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

            // move all units
            foreach (IUnit unit in units)
            {
                unit.PlayCycle((int)timeSpan);
            }

            // move all missiles and check player collision
            foreach(Missile missile in missiles)
            {
                missile.PlayCycle(timeSpan);
                foreach (IUnit unit in units)
                    unit.TryHitByMissile(missile);
            }
            missiles.RemoveAll((Missile m) => m.IsDead);
        }
    }
}
