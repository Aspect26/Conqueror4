using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Server
{
    /// <summary>
    /// Represents one map instance. Every map in the game is represented by one 
    /// MapInstance object. This way the game is divided into isolated smaller parts.
    /// The map instance takes care of its game cycles and handles unit's (and player's)
    /// actions.
    /// </summary>
    public class MapInstance
    {
        private List<StateObject> clientStates;
        private List<IUnit> units;
        private Queue<IPlayerAction> playerActions;
        private List<IUnitDifference> mapGeneralDifferenes;
        private List<IItem> droppedItems;
        private List<IObject> objects;
        private SortedList<long, List<ITimedAction>> timedActions;
        private List<Missile> missiles;
        private Point ReviveLocation;
        private int lastUniqueId = 1;

        private long lastTimeStamp;
        private const int PLAYERACTIONS_PROCESSED_IN_ONE_CYCLE = 5;

        private int mapId;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapInstance"/> class.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        public MapInstance(int mapId)
        {
            this.mapId = mapId;
            this.missiles = new List<Missile>();
            this.mapGeneralDifferenes = new List<IUnitDifference>();
            this.clientStates = new List<StateObject>();
            this.units = new List<IUnit>();
            this.playerActions = new Queue<IPlayerAction>();
            this.timedActions = new SortedList<long, List<ITimedAction>>();
            this.ReviveLocation = Data.GetReviveLocation(mapId);
            this.droppedItems = new List<IItem>();
            this.objects = new List<IObject>();

            lastTimeStamp = Stopwatch.GetTimestamp();
        }

        /// <summary>
        /// Removes a client from the map.
        /// </summary>
        /// <param name="client">The client.</param>
        public void RemoveClient(StateObject client)
        {
            lock(clientStates)
                clientStates.Remove(client);

            lock(units)
                units.Remove(client.PlayingCharacter);

            lock(mapGeneralDifferenes)
                mapGeneralDifferenes.Add(new UnitDiedDifference(client.PlayingCharacter));
        }

        /// <summary>
        /// Spawns an NPC unit.
        /// </summary>
        /// <param name="unitId">The unit identifier.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>IUnit.</returns>
        public IUnit SpawnNPC(int unitId, int x, int y)
        {
            IUnit newUnit = new GenericUnit(unitId, GetNextUniqueID(), new Location(x, y), 
                this, Data.GetInitialData(unitId));

            lock (units)
                units.Add(newUnit);

            return newUnit;
        }

        /// <summary>
        /// Spawns a game object.
        /// </summary>
        /// <param name="objInfo">The object information.</param>
        public void SpawnObject(ObjectInfo objInfo)
        {
            lock (objects) {
                objects.Add(GenericObject.Create(objInfo, GetNextUniqueID()));
            }
        }

        /// <summary>
        /// Gets the next available unique identifier. This function is not really accurate
        /// but it relies on the fact that the game will never be played by many players at
        /// a time :'(.
        /// </summary>
        /// <returns>The uid.</returns>
        public int GetNextUniqueID()
        {
            return lastUniqueId++;
        }

        /// <summary>
        /// Gets the list of all units in the map (including players).
        /// </summary>
        /// <returns>List&lt;IUnit&gt;.</returns>
        public List<IUnit> GetUnits()
        {
            return this.units;
        }

        /// <summary>
        /// Gets all clients's state objects.
        /// </summary>
        /// <returns>List&lt;StateObject&gt;.</returns>
        public List<StateObject> GetClients()
        {
            return clientStates;
        }

        /// <summary>
        /// Adds a missile to the map.
        /// </summary>
        /// <param name="m">The missile.</param>
        public void AddMissile(Missile m)
        {
            this.missiles.Add(m);
        }

        /// <summary>
        /// Adds a timed action to the map instance..
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="afterSeconds">The time (in seconds) after it shall be 
        /// processed.</param>
        public void AddTimedAction(ITimedAction action, int afterSeconds)
        {
            lock (timedActions)
            {
                long key = Extensions.GetCurrentMillis() + afterSeconds * 1000;
                if (!timedActions.Keys.Contains(key))
                    timedActions.Add(key, new List<ITimedAction>());

                timedActions[key].Add(action);
            }
        }

        /// <summary>
        /// Adds a general difference. A general difference is a difference that cannot
        /// be held in a unt's object (e.g.: if a unit dies it is removed from the game
        /// so when the server next time sends list of all differences that happened it
        /// would not get the information about unit dying).
        /// </summary>
        /// <param name="difference">The difference.</param>
        /// <seealso cref="IUnitDifference"/>
        public void AddGeneralDifference(IUnitDifference difference)
        {
            this.mapGeneralDifferenes.Add(difference);
        }

        // THIS IS CALLED FROM GAME TASK
        /// <summary>
        /// Handles player's command to shoot. 
        /// </summary>
        /// <param name="character">The character.</param>
        /// <param name="timeStamp">The time stamp.</param>
        /// <param name="x">The x coordinate of the direction vector.</param>
        /// <param name="y">The y coordinate of the direction vector.</param>
        public void PlayerShoot(Character character, long timeStamp, int x, int y)
        {
            Missile missile = character.Shoot(timeStamp, x, y);
            if(missile != null)
            {
                this.missiles.Add(missile);
            }
        }

        // THIS IS CALLED FROM SENDING TASK
        /// <summary>
        /// Gets the general differences and resets them.
        /// </summary>
        /// <returns>List&lt;IUnitDifference&gt;.</returns>
        public List<IUnitDifference> GetGeneralDifferencesAndReset()
        {
            List<IUnitDifference> diffs = null;
            lock (mapGeneralDifferenes)
            {
                diffs = mapGeneralDifferenes;
                mapGeneralDifferenes = new List<IUnitDifference>();
            }

            return diffs;
        }

        // THIS IS CALLED FROM GAME TASK
        /// <summary>
        /// Gets a dropped item.
        /// </summary>
        /// <param name="uid">The uid.</param>
        /// <returns>The item if there really is a dropped item (on the ground...)
        /// with the specified uid or null otherise.</returns>
        public IItem GetDroppedItem(int uid)
        {
            IItem item = droppedItems.Find(i => i.UniqueID == uid);
            if(item != null)
            {
                droppedItems.Remove(item);
                mapGeneralDifferenes.Add(new ItemRemovedDifference(item));
            }

            return item;
        }

        // THIS IS CALLED FROM RECEIVING TASK
        /// <summary>
        /// Gets the coded data of the whole instance for the server message.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetMessageCodedData()
        {
            string data = "";

            lock (units)
            {
                foreach (IUnit unit in units)
                {
                    data += unit.UniqueID + "|" + unit.UnitID + "|" + unit.GetName() + "|"
                        + unit.GetLocation().X + "|" + unit.GetLocation().Y + "|"
                        + unit.MaxStats.HitPoints + "|" + unit.ActualStats.HitPoints + "|"
                        + unit.Fraction + ",";
                }
            }
            
            // objects are only sended so they does not need to be locked
            foreach(IObject obj in objects)
            {
                data += obj.GetCodedData() + ",";
            }

            return data;
        }

        // THIS IS CALLED FROM RECEIVING TASK
        /// <summary>
        /// Adds a player action to the actions queue.
        /// </summary>
        /// <param name="action">The action.</param>
        public void AddPlayerAction(IPlayerAction action)
        {
            lock (playerActions)
            {
                playerActions.Enqueue(action);
            }
        }

        // THIS IS CALLED FROM GAME TASK
        /// <summary>
        /// Adds a player to the map instance.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="character">The character.</param>
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
        /// <summary>
        /// Plays one game cycle. This function is a real hell.
        /// Dear maintainer:
        /// 
        /// Once you are done trying to 'optimize' this routine,
        /// and have realized what a terrible mistake that was,
        /// please increment the following counter as a warning
        /// to the next guy:
        /// 
        /// total_hours_wasted_here = 95
        /// 
        /// hahahahahahahahahahahahahahah
        /// </summary>
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
                    playerActions.Dequeue().Process(this, (now*1000)/Stopwatch.Frequency);
                }
            }

            // process timed actions
            lock (timedActions)
            {
                while (true)
                {
                    if (timedActions.Count == 0)
                        break;

                    long firstKey = timedActions.Keys[0];
                    if (firstKey > (now * 1000) / Stopwatch.Frequency)
                        break;

                    // OUCH THIS :'(
                    lock (this)
                    {
                        foreach (ITimedAction action in timedActions[firstKey])
                            action.Process(this);

                        timedActions.Remove(firstKey);
                    }
                }
            }

            // move all units
            lock (units)
            {
                foreach (IUnit unit in units)
                {
                    unit.PlayCycle((int)timeSpan);
                    if (unit.IsDead)
                    {
                        foreach (IUnit hittedBy in unit.HittedBy)
                        {
                            hittedBy.AddExperience(Data.GetXPReward(unit.Level));
                            if (hittedBy is Character)
                            {
                                Character c = (Character)hittedBy;
                                if (c.CurrentQuest.Killed(unit.UnitID))
                                    c.AddDifference(new QuestObjectiveDifference(c.UniqueID, c.CurrentQuest));
                            }
                        }
                    }
                }

                // remove killed units, create respawn action, drop items
                units.RemoveAll(
                    (IUnit u) =>
                    {
                        if (u.IsDead)
                        {
                            lock (mapGeneralDifferenes)
                            {
                                if (!u.IsPlayer())
                                {
                                    mapGeneralDifferenes.Add(new UnitDiedDifference(u));
                                    AddTimedAction(new RespawnUnitAction(u), u.RespawnTime);
                                    IItem item = u.GenerateDroppedItem();
                                    if(item != null)
                                    {
                                        droppedItems.Add(item);
                                        mapGeneralDifferenes.Add(new ItemDroppedDifference(item, u));
                                        AddTimedAction(new RemoveItemAction(item.UniqueID), 45);
                                    }
                                }
                                else
                                {
                                    Character character = (Character)u;
                                    character.Revive(this.ReviveLocation);
                                }
                            }
                        }

                        return u.IsDead;
                    });

                // move all missiles and check player collision
                foreach (Missile missile in missiles)
                {
                    missile.PlayCycle(timeSpan);
                    foreach (IUnit unit in units)
                        unit.TryHitByMissile(missile);
                }
                missiles.RemoveAll((Missile m) => m.IsDead);

                // check visited ranges
                foreach (IUnit unit in units)
                {
                    Point unitPoint = new Point(unit.GetLocation().X, unit.GetLocation().Y);
                    foreach (IUnit host in units)
                    {
                        if (unit == host || !unit.IsPlayer())
                            continue;

                        Point hostPoint = new Point(host.GetLocation().X, host.GetLocation().Y);
                        // visit -> for quests
                        bool isVisited = unit.CurrentlyVisited.Contains(host);

                        if (isVisited && !(unitPoint.DistanceFrom(hostPoint) < Data.VisitDistance))
                            unit.CurrentlyVisited.Remove(host);

                        if (!isVisited && (unitPoint.DistanceFrom(hostPoint) < Data.VisitDistance))
                        {
                            unit.CurrentlyVisited.Add(host);
                            AddPlayerAction(new CharacterVisitedUnitAction((Character)unit, host));
                        }

                        // enter combat range 
                        if (host.IsPlayer())
                            continue;

                        if (host.Fraction == unit.Fraction)
                            continue;

                        bool isInCombat = host.InCombatWith.Contains(unit);
                        if (isInCombat && unitPoint.DistanceFrom(host.SpawnPosition) > Data.LeaveCombatDistance)
                        {
                            host.LeaveCombatWith(unit);
                            unit.LeaveCombatWith(host);
                            Console.WriteLine(host.GetName() + " left combat with " + unit.GetName());
                        }
                        if (!isInCombat && unitPoint.DistanceFrom(host.SpawnPosition) < Data.EnterCombatDistance)
                        {
                            host.EnterCombatWith(unit);
                            unit.EnterCombatWith(host);
                            Console.WriteLine(host.GetName() + " entered combat with " + unit.GetName());
                        }
                    }
                }
            }
        }
    }
}
