using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Server
{
    public class MapInstance
    {
        private List<StateObject> clientStates;
        private List<IUnit> units;
        private Queue<IPlayerAction> playerActions;
        private List<IUnitDifference> mapGeneralDifferenes;
        private SortedList<long, List<ITimedAction>> timedActions;
        private List<Missile> missiles;
        private Point ReviveLocation;
        private int lastUniqueId = 1;

        private long lastTimeStamp;
        private const int PLAYERACTIONS_PROCESSED_IN_ONE_CYCLE = 5;

        private int mapId;

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

            lastTimeStamp = Stopwatch.GetTimestamp();
        }

        public void RemoveClient(StateObject client)
        {
            lock(clientStates)
                clientStates.Remove(client);

            lock(units)
                units.Remove(client.PlayingCharacter);

            lock(mapGeneralDifferenes)
                mapGeneralDifferenes.Add(new UnitDiedDifference(client.PlayingCharacter));
        }

        public IUnit SpawnNPC(int unitId, int x, int y)
        {
            IUnit newUnit = new GenericUnit(unitId, GetNextUniqueID(), new Location(x, y), 
                this, Data.GetInitialData(unitId));

            lock (units)
                units.Add(newUnit);

            return newUnit;
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

        public void AddMissile(Missile m)
        {
            this.missiles.Add(m);
        }

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

        // THIS IS CALLED FROM GAME TASK
        public void PlayerShoot(Character character, long timeStamp, int x, int y)
        {
            Missile missile = character.Shoot(timeStamp, x, y);
            if(missile != null)
            {
                this.missiles.Add(missile);
            }
        }

        // THIS IS CALLED FROM SENDING TASK
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

        // THIS IS CALLED FROM RECEIVING TASK
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

            return data;
        }

        // THIS IS CALLED FROM RECEIVING TASK
        public void AddPlayerAction(IPlayerAction action)
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
                    playerActions.Dequeue().Process((now*1000)/Stopwatch.Frequency);
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

                // remove killed units and create respawn action
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
