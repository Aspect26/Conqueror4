using Shared;
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

        public Game(PlayedCharacter character)
        {
            this.Character = character;
            this.CreateMap();

            this.units = new Dictionary<int, IUnit>();
            this.missiles = new List<Missile>();
        }

        private  void CreateMap()
        {
            Map = new Map();
            Map.Create(GameData.GetMapFilePath(Character.Location.MapID));
        }

        public void RunCycle(Graphics g, int timeSpan)
        {
            Character.PlayCycle(timeSpan);
            foreach (Missile missile in missiles)
            {
                missile.PlayCycle(timeSpan);
                foreach (KeyValuePair<int, IUnit> pair in units)
                    pair.Value.TryHitByMissile(missile);
            }

            missiles.RemoveAll((Missile m) => m.IsDead);

            RenderAll(g);
        }

        private void RenderAll(Graphics g)
        {
            // map
            Map.Render(g, Character.Location);

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
        }

        public void AddOrUpdateUnit(string name, int unitId, int uniqueId, int x, int y, BaseStats maxStats,
            BaseStats actualStats)
        {
            if(uniqueId == Character.UniqueID)
            {
                Character.SetMaxStats(maxStats);
                Character.SetActualStats(actualStats);
            }
            else if (units.ContainsKey(uniqueId))
            {
                units[uniqueId].SetLocation(x, y);
            }
            else
            {
                if(GameData.IsPlayerUnit(unitId))
                    units.Add(uniqueId, new PlayerUnit(this, name, unitId, uniqueId, x, y, maxStats, actualStats));
                else
                    units.Add(uniqueId, new GenericUnit(this, unitId, uniqueId, new Location(x, y), maxStats,
                        actualStats));
            }
        }

        public void AddMissile(int uniqueId, int dirX, int dirY)
        {
            if (uniqueId == Character.UniqueID)
                return;


            IUnit unit = units[uniqueId];
            Point position = new Point(unit.Location.X, unit.Location.Y);
            lock (missiles)
                this.missiles.Add(new Missile(this, unit, GameData.GetMissileImage(unit.UnitID), position, 
                    new Point(dirX, dirY)));
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
    }
}
