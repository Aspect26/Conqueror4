using System.Collections.Generic;

namespace Server
{
    /// <summary>
    /// Represents the actual game. The game is running in a separate thread.
    /// It contains a map instances for each map and on every game cycle it
    /// sequentially locks one instance after another and tells the instance to
    /// play its game cycle. The map instances compute the cycle's time span
    /// individually.
    /// </summary>
    public class Game
    {
        private bool running = false;
        private Dictionary<int, MapInstance> mapInstances;

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// Gets the map instances from the game data.
        /// </summary>
        public Game()
        {
            mapInstances = Data.GetMapInstances();
        }

        /// <summary>
        /// Removes a client from the game and it's map instance.
        /// </summary>
        /// <param name="client">The client.</param>
        public void RemoveClient(StateObject client)
        {
            MapInstance map = client.PlayingCharacter.MapInstance;
            map.RemoveClient(client);
        }

        /// <summary>
        /// Gets a map instance by its identifier.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        /// <returns>The map instance.</returns>
        public MapInstance GetMapInstance(int mapId)
        {
            return mapInstances[mapId];
        }

        /// <summary>
        /// Gets all the map instances.
        /// </summary>
        /// <returns>Dictionary&lt;System.Int32, MapInstance&gt;.</returns>
        public Dictionary<int, MapInstance> GetMapInstances()
        {
            return this.mapInstances;
        }

        /// <summary>
        /// Adds a player action to its corresponding instance. This function is called
        /// in the receiving thread when it receives a message from a client that
        /// some event with it's character happend (e.g.: it moved).
        /// </summary>
        /// <param name="action">The action.</param>
        public void AddPlayerAction(IPlayerAction action)
        {
            Character c = action.GetCharacter();
            mapInstances[c.Location.MapID].AddPlayerAction(action);
        }

        /// <summary>
        /// Starts the infinite game cycle.
        /// </summary>
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

        /// <summary>
        /// Adds a character to map instance. The map identifier shall be already
        /// defined in the character instance.
        /// </summary>
        /// <param name="state">The client's state object.</param>
        /// <param name="character">The character (This is possibly not needed
        /// because the StateObject contains information about the playing
        /// character of a client.</param>
        /// <returns>The map instance the character was added to.</returns>
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
