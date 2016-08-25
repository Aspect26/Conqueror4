using Shared;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Client
{
    /// <summary>
    /// Represents the whole game. The other part is in the ServerMessage.cs.
    /// </summary>
    public partial class Game
    {
        /// <summary>
        /// Gets or sets the map.
        /// </summary>
        /// <value>The map.</value>
        public Map Map { get; set; }

        /// <summary>
        /// Gets or sets the player's currently plaeyd character.
        /// </summary>
        /// <value>The character.</value>
        public PlayedCharacter Character { get; set; }

        private ServerConnection server;
        private Dictionary<int, IUnit> units;
        private List<Missile> missiles;
        private List<IGroundObject> objects;
        private List<IGroundObject> collidingObjects;
        private IItem droppedItem;
        private List<ISpecialEffect> specialEffects;

        /// <summary>
        /// Delegate for NewQuestAcquired event. It gets the quest as a parameter.
        /// </summary>
        /// <param name="quest">The quest.</param>
        public delegate void NewQuestDelegate (IQuest quest);

        /// <summary>
        /// Occurs when player acquired a new quest.
        /// </summary>
        public event NewQuestDelegate NewQuestAcquired;

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// Initializes the variables to their default values and create the map.
        /// </summary>
        /// <param name="server">The server connection.</param>
        /// <param name="character">The playedd character.</param>
        public Game(ServerConnection server, PlayedCharacter character)
        {
            this.Character = character;
            this.server = server;
            this.units = new Dictionary<int, IUnit>();
            this.missiles = new List<Missile>();
            this.objects = new List<IGroundObject>();
            this.collidingObjects = new List<IGroundObject>();
            this.specialEffects = new List<ISpecialEffect>();
        }

        public  void CreateMap()
        {
            if(Map == null)
                Map = new Map();

            Map.Create(GameData.GetMapFilePath(Character.Location.MapID));
        }

        // TODO: this function should be split into smaller ones...
        /// <summary>
        /// Runs one game cycle.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        /// <param name="timeSpan">The time span from last cycle.</param>
        public void RunCycle(Graphics g, int timeSpan)
        {
            // played character
            Character.PlayCycle(timeSpan);

            // ground objects - collide
            foreach (IGroundObject obj in objects)
            {
                if(new Point(Character.Location.X, Character.Location.Y).DistanceFrom(obj.Location) <= 
                    obj.GetCollisionDistance())
                {
                    if (!collidingObjects.Contains(obj))
                    {
                        obj.Collide();
                        collidingObjects.Add(obj);
                    }
                }
            }

            // ground objects - leave
            List<IGroundObject> leftObjects = new List<IGroundObject>();
            foreach(IGroundObject collidingObject in collidingObjects)
            {
                if (new Point(Character.Location.X, Character.Location.Y).DistanceFrom(collidingObject.Location) >
                    collidingObject.GetCollisionDistance())
                {
                    leftObjects.Add(collidingObject);
                }
            }

            foreach(IGroundObject leftObject in leftObjects)
            {
                leftObject.Leave();
                collidingObjects.Remove(leftObject);
            }

            // missiles
            foreach (Missile missile in missiles)
            {
                missile.PlayCycle(timeSpan);
                foreach (KeyValuePair<int, IUnit> pair in units)
                {
                    pair.Value.TryHitByMissile(missile);
                }
                Character.TryHitByMissile(missile);
            }

            // special effects
            foreach (ISpecialEffect effect in specialEffects)
            {
                effect.PlayCycle(timeSpan);
            }
            specialEffects.RemoveAll(e => e.IsDead);

            // units
            var killedUnits = new List<IUnit>();
            foreach (KeyValuePair<int, IUnit> pair in units)
            {
                if (pair.Value.IsDead)
                    killedUnits.Add(pair.Value);
            }
            foreach (var killedUnit in killedUnits)
            {
                units.Remove(killedUnit.UniqueID);
            }

            missiles.RemoveAll((Missile m) => m.IsDead);

            RenderAll(g);
        }

        private void RenderAll(Graphics g)
        {
            // map
            Map.Render(g, Character.Location);

            // objects
            foreach (IGroundObject obj in objects)
                obj.Render(g);

            // other units
            foreach (KeyValuePair<int, IUnit> unit in units)
                unit.Value.DrawUnit(g);

            // player
            Character.DrawUnit(g);

            // missiles
            lock (missiles)
            {
                foreach (Missile missile in missiles)
                    missile.Render(g);
            }

            // special effects
            foreach (ISpecialEffect effect in specialEffects)
                effect.Render(g);
        }

        /// <summary>
        /// Updates a unit's actual hit points.
        /// </summary>
        /// <param name="uniqueId">The unit's unique identifier.</param>
        /// <param name="hitPoints">The hit points.</param>
        public void UpdateUnitActualHitPoints(int uniqueId, int hitPoints)
        {
            if (units.ContainsKey(uniqueId))
            {
                units[uniqueId].ActualStats.HitPoints = hitPoints;
            } 
            else if(uniqueId == Character.UniqueID)
            {
                Character.ActualStats.HitPoints = hitPoints;
            }
        }

        /// <summary>
        /// Updates a unit's location.
        /// </summary>
        /// <param name="uniqueId">The unit's unique identifier.</param>
        /// <param name="x">The x location.</param>
        /// <param name="y">The y location.</param>
        public void UpdateUnitLocation(int uniqueId, int x, int y)
        {
            if(uniqueId == Character.UniqueID)
                return;

            if (units.ContainsKey(uniqueId))
            {
                units[uniqueId].SetLocation(x, y);
            }
        }

        /// <summary>
        /// Updates a unit's location.
        /// </summary>
        /// <param name="uid">The unit's unique identifier.</param>
        /// <param name="l">The unit's new location.</param>
        public void UpdateUnitLocation(int uid, Location l)
        {
            if(uid == Character.UniqueID)
            {
                Character.ChangeLocation(l);
            }
            else
            {
                if(l.MapID != Character.Location.MapID)
                {
                    KillUnit(uid);
                }
                else
                {
                    UpdateUnitLocation(uid, l.X, l.Y);
                }
            }
        }

        /// <summary>
        /// Updates the player's quest.
        /// </summary>
        /// <param name="quest">The new quest.</param>
        public void UpdateQuest(IQuest quest)
        {
            Character.SetCurrentQuest(quest);
            NewQuestAcquired(quest);
        }

        /// <summary>
        /// Updates the player's quest objectives.
        /// </summary>
        /// <param name="objectives">The new objectives.</param>
        public void UpdateQuestObjectives(QuestObjective[] objectives)
        {
            Character.Quest.UpdateObjectives(objectives);
        }

        /// <summary>
        /// Adds a new unit to the game unit.
        /// </summary>
        /// <param name="name">The unit's name.</param>
        /// <param name="unitId">The unit's identifier.</param>
        /// <param name="uniqueId">The unit's unique identifier.</param>
        /// <param name="xLoc">The unit's x loccation.</param>
        /// <param name="yLoc">The unit's y location.</param>
        /// <param name="maxStats">The unit's maximum stats.</param>
        /// <param name="actualStats">The unit's actual stats.</param>
        /// <param name="fraction">The unit's fraction.</param>
        public void AddUnit(string name, int unitId, int uniqueId, int xLoc, int yLoc, BaseStats maxStats, 
            BaseStats actualStats, int fraction)
        {
            if (uniqueId == Character.UniqueID)
                return;

            if (GameData.IsPlayerUnit(unitId))
                units.Add(uniqueId, new PlayerUnit(this, name, unitId, uniqueId, xLoc, yLoc, maxStats, actualStats,
                    fraction));
            else
                units.Add(uniqueId, new GenericUnit(this, unitId, uniqueId, new Location(xLoc, yLoc), maxStats,
                    actualStats, fraction));
        }

        /// <summary>
        /// Adds a missile to the game.
        /// </summary>
        /// <param name="uniqueId">The unit's (which shot it, to determine the image
        /// which shall be used) unique identifier.</param>
        /// <param name="dirX">The direction vector's x part.</param>
        /// <param name="dirY">The direction vector's y part.</param>
        public void AddMissile(int uniqueId, int dirX, int dirY)
        {
            IUnit unit;
            if (uniqueId == Character.UniqueID)
                unit = Character;
            else
                unit = units[uniqueId];

            Point position = new Point(unit.Location.X, unit.Location.Y);
            lock (missiles)
            {
                Image img = GameData.GetMissileImage(unit.UnitID);
                img = Extensions.RotateImage(img, ((float)Math.Atan2(dirY, dirX))*(180.0f / (float)Math.PI));
                this.missiles.Add(new Missile(this, unit, img, position, new Point(dirX, dirY)));
            }
        }

        /// <summary>
        /// Adds a missile to the game.
        /// </summary>
        /// <param name="missile">The new missile.</param>
        public void AddMissile(Missile missile)
        {
            lock(missiles)
                this.missiles.Add(missile);
        }

        /// <summary>
        /// Converts a position on the map to a position on the screen.
        /// </summary>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <returns>The screen position.</returns>
        public Point MapPositionToScreenPosition(int x, int y)
        {
            int newX = Application.WIDTH / 2 - (int)Character.Location.X + x;
            int newY = Application.HEIGHT / 2 - (int)Character.Location.Y + y;

            return new Point(newX, newY); 
        }

        /// <summary>
        /// Converts a position on the map to a position on the screen.
        /// </summary>
        /// <param name="p">The position.</param>
        /// <returns>The screen position.</returns>
        public Point MapPositionToScreenPosition(Point p)
        {
            return MapPositionToScreenPosition(p.X, p.Y);
        }

        /// <summary>
        /// Changes the player's experience.
        /// </summary>
        /// <param name="xp">The new xp.</param>
        public void ChangePlayerXp(int xp)
        {
            Character.Experience = xp;
        }

        /// <summary>
        /// Changes the player's level.
        /// </summary>
        /// <param name="level">The new level.</param>
        /// <param name="newXpRequirement">The new xp requirement.</param>
        public void ChangePlayerLevel(int level, int newXpRequirement)
        {
            Character.Level = level;
            Character.ExperienceRequired = newXpRequirement;
        }

        /// <summary>
        /// Kills a unit with specified unique identifier.
        /// </summary>
        /// <param name="uid">The unit's unique iddentifier.</param>
        public void KillUnit(int uid)
        {
            if(uid == Character.UniqueID)
            {
                Character.Quest.Reset();
                return;
            }

            if (!units[uid].IsPlayer())
            {
                units[uid].Kill();
            }
        }

        /// <summary>
        /// Updates a unit's actual stats.
        /// </summary>
        /// <param name="uid">The unit's unique identifier.</param>
        /// <param name="stats">The new actual stats.</param>
        public void UpdateActualStats(int uid, BaseStats stats)
        {
            IUnit updatingUnit = (uid == Character.UniqueID) ? Character : units[uid];
            updatingUnit.UpdateActualStats(stats);
        }

        /// <summary>
        /// Updates a unit's actual stats.
        /// </summary>
        /// <param name="uid">The unit's unique identifier.</param>
        /// <param name="stats">The new max stats.</param>
        public void UpdateMaxStats(int uid, BaseStats stats)
        {
            IUnit updatingUnit = (uid == Character.UniqueID) ? Character : units[uid];
            updatingUnit.UpdateMaxStats(stats);
        }

        /// <summary>
        /// Creates a new chest object on the ground. The chest will contain the
        /// specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="location">The chest's location on the ground.</param>
        public void CreateItem(IItem item, Point location)
        {
            this.objects.Add(new ChestObject(this, item, location));
        }

        /// <summary>
        /// Adds a new object to the game. The object is parsed from server message.
        /// </summary>
        /// <param name="dataParts">The splitted server message.</param>
        public void AddObject(string[] dataParts)
        {
            this.objects.Add(GroundObject.CreateObject(this, dataParts));
        }

        /// <summary>
        /// Sets the dropped item meaning the item that it displayed in the Dropped item
        /// slot in the game's right side overlay.
        /// </summary>
        /// <param name="droppedItem">The dropped item.</param>
        public void SetDroppedItem(IItem droppedItem)
        {
            if(this.droppedItem == null)
            {
                this.droppedItem = droppedItem;
            }
        }

        /// <summary>
        /// Tries to remove the dropped item from the rigt side's overlay's dropped item
        /// slot. Removes it if and only if the item passed in the argument is the item
        /// that is currently displayed in the slot.
        /// </summary>
        /// <param name="leftItem">The item that tries to be removed from the slot.</param>
        public void TryRemoveDroppedItem(IItem leftItem)
        {
            if (droppedItem == null)
                return;

            if(leftItem.UniqueID == droppedItem.UniqueID)
            {
                this.droppedItem = null;
            }
        }

        /// <summary>
        /// Gets the item that is currently displayed in the game's right sied overlay in
        /// the dropped item slot.
        /// </summary>
        /// <returns>The item.</returns>
        public IItem GetDroppedItem()
        {
            return this.droppedItem;
        }

        /// <summary>
        /// Tries to take the item that is displayed in the game's right side's overlay's
        /// dropped item slot (and equip it in the correspondding item slot). It actually 
        /// only sends the server a message that the player tries to take the item. If 
        /// the item is still possible, the server response with a message that the item 
        /// is removed from the ground and update character's equip.
        /// </summary>
        public void TryTakeDroppedItem()
        {
            if (droppedItem == null)
                return;

            server.TakeItem(droppedItem.UniqueID);
        }

        /// <summary>
        /// Send the server a message that the player is trying to use his special ability.
        /// </summary>
        public void TryUseAbility()
        {
            server.TryUseAbility();
        }

        /// <summary>
        /// Sends server a message that the player wants to change the map.
        /// </summary>
        /// <param name="mapId">The map identifier.</param>
        public void TryChangeMap(int mapId)
        {
            server.TryChangeMap(mapId);
        }

        /// <summary>
        /// Removes a game object with specified unique id.
        /// </summary>
        /// <param name="uid">The uid.</param>
        public void RemoveGameObject(int uid)
        {
            this.objects.RemoveAll(obj => obj.UniqueID == uid);

            var colliding = this.collidingObjects.FindAll(obj => obj.UniqueID == uid);
            foreach(IGroundObject collidingObj in colliding)
            {
                collidingObj.Leave();
                this.objects.Remove(collidingObj);
            }
        }

        /// <summary>
        /// Equips the specified item in the correspondding slot.
        /// </summary>
        /// <param name="item">The item.</param>
        public void EquipItem(IItem item)
        {
            this.Character.Equip.Items[item.Slot] = item;
        }

        /// <summary>
        /// Adds a special effect into the game.
        /// </summary>
        /// <param name="specialEffect">The special effect.</param>
        public void AddSpecialEffect(ISpecialEffect specialEffect)
        {
            this.specialEffects.Add(specialEffect);
        }
    }
}