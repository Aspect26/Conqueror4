using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    pair.Value.PlayCycle();
                }
            }
        }

        public MapInstance AddPlayer(StateObject state, Character character)
        {
            mapInstances[character.Location.MapID].AddPlayer(state, character);
            return mapInstances[character.Location.MapID];
        }
    }
}
