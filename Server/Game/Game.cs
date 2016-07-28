using System.Collections.Generic;

namespace Server
{
    public class Game
    {
        private bool running = false;

        private Dictionary<int, MapInstance> mapInstances;
        private Queue<ISendAction> sendActions;

        public void Initialize(Queue<ISendAction> sendActions)
        {
            mapInstances = new Dictionary<int, MapInstance>();

            Dictionary<int, string> maps = Data.GetMaps(); 
            foreach(int key in maps.Keys)
            {
                mapInstances.Add(key, new MapInstance(key, sendActions));
            }

            this.sendActions = sendActions;
        }

        public Dictionary<int, MapInstance> GetMapInstances()
        {
            return this.mapInstances;
        }

        public void AddPlayerAction(IPlayerAction action)
        {
            Character c = action.GetCharacter();
            mapInstances[c.Location.MapID].AddAction(action);
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
            mapInstances[character.Location.MapID].AddPlayer(state, character);
            return mapInstances[character.Location.MapID];
        }

        public MapInstance GetMapInstanceOfCharacter(Character character)
        {
            return mapInstances[character.Location.MapID];
        }
    }
}
