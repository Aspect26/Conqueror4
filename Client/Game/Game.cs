using Shared;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Client
{
    public partial class Game
    {
        public Map Map { get; set; }
        public PlayedCharacter Character { get; set; }
        private Dictionary<int, IUnit> units;
        private List<Missile> missiles;
        private List<IGroundObject> objects;

        // EVENTS
        public delegate void NewQuestDelegate (IQuest qiest);
        public event NewQuestDelegate NewQuestAcquired;

        public Game(PlayedCharacter character)
        {
            this.Character = character;
            this.CreateMap(character.Location.MapID);

            this.units = new Dictionary<int, IUnit>();
            this.missiles = new List<Missile>();
            this.objects = new List<IGroundObject>();

            character.MapChanged += CreateMap;
        }

        private  void CreateMap(int mapId)
        {
            if(Map == null)
                Map = new Map();

            Map.Create(GameData.GetMapFilePath(mapId));
        }

        public void RunCycle(Graphics g, int timeSpan)
        {
            // played character
            Character.PlayCycle(timeSpan);

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
        }

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

        public void UpdateUnitLocation(int uniqueId, int x, int y)
        {
            if(uniqueId == Character.UniqueID)
                return;

            if (units.ContainsKey(uniqueId))
            {
                units[uniqueId].SetLocation(x, y);
            }
        }

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

        public void UpdateQuest(IQuest quest)
        {
            Character.SetCurrentQuest(quest);
            NewQuestAcquired(quest);
        }

        public void UpdateQuestObjectives(QuestObjective[] objectives)
        {
            Character.Quest.UpdateObjectives(objectives);
        }

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

        public void AddMissile(Missile missile)
        {
            lock(missiles)
                this.missiles.Add(missile);
        }

        public Point MapPositionToScreenPosition(int x, int y)
        {
            int newX = Application.WIDTH / 2 - (int)Character.Location.X + x;
            int newY = Application.HEIGHT / 2 - (int)Character.Location.Y + y;

            return new Point(newX, newY); 
        }

        public Point MapPositionToScreenPosition(Point p)
        {
            return MapPositionToScreenPosition(p.X, p.Y);
        }

        public void ChangePlayerXp(int xp)
        {
            Character.Experience = xp;
        }

        public void ChangePlayerLevel(int level)
        {
            Character.Level = level;
        }

        public void KillUnit(int uid)
        {
            if(uid == Character.UniqueID)
            {
                Character.Quest.Reset();
                return;
            }

            if (!units[uid].Isplayer())
            {
                units[uid].Kill();
            }
        }

        public void UpdateActualStats(int uid, BaseStats stats)
        {
            IUnit updatingUnit = (uid == Character.UniqueID) ? Character : units[uid];
            updatingUnit.UpdateActualStats(stats);
        }

        public void CreateItem(IItem item, Point location)
        {
            this.objects.Add(new ChestObject(this, item, location));
        }
    }
}