using System.Collections.Generic;

namespace Server
{
    public class Game
    {
        private bool running = false;
        private Dictionary<int, MapInstance> mapInstances;

        public Game()
        {
            mapInstances = Data.GetMapInstances();
        }

        public void RemoveClient(StateObject client)
        {
            MapInstance map = client.PlayingCharacter.MapInstance;
            map.RemoveClient(client);
        }

        public MapInstance GetMapInstance(int mapId)
        {
            return mapInstances[mapId];
        }

        public Dictionary<int, MapInstance> GetMapInstances()
        {
            return this.mapInstances;
        }

        public void AddPlayerAction(IPlayerAction action)
        {
            Character c = action.GetCharacter();
            mapInstances[c.Location.MapID].AddPlayerAction(action);
        }

        public void Start()
        {
            running = true;
            while (running)
            {
                foreach(KeyValuePair<int, MapInstance> pair in mapInstances)
                {
                    lock (pair.Value)
                    {
                        pair.Value.PlayCycle();
                    }
                }
            }
        }

        public MapInstance AddPlayer(StateObject state, Character character)
        {
            int mapId = character.Location.MapID;
            int uniqueId = -1;

            lock (mapInstances[mapId]) { 
                character.SetMapInstance(mapInstances[mapId]);
                uniqueId = mapInstances[mapId].GetNextUniqueID();
                character.SetUniqueID(uniqueId);
                character.AddDifference(new UnitSpawnedDifference(character));
                mapInstances[mapId].AddPlayer(state, character);
            }

            return mapInstances[mapId];
        }
    }
}
