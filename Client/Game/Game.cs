using System.Collections.Generic;
using System.Drawing;

namespace Client
{
    public partial class Game
    {
        public Map Map { get; set; }
        public PlayedCharacter Character { get; set; }
        private Dictionary<string, IUnit> units;
        private List<Missile> missiles;

        public Game(PlayedCharacter character)
        {
            this.Character = character;
            this.CreateMap();

            this.units = new Dictionary<string, IUnit>();
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
                missile.PlayCycle(timeSpan);

            missiles.RemoveAll((Missile m) => m.IsDead);

            RenderAll(g);
        }

        private void RenderAll(Graphics g)
        {
            // map
            Map.Render(g, Character.Location);

            // other units
            foreach (KeyValuePair<string, IUnit> unit in units)
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

        public void AddOrUpdateUnit(string name, int id, int x, int y)
        {
            if (units.ContainsKey(name))
            {
                units[name].SetLocation(x, y);
            }
            else
            {
                units.Add(name, new PlayerUnit(this, name, id, x, y));
            }
        }

        public void AddMissile(string unitName, int dirX, int dirY)
        {
            IUnit unit = units[unitName];
            Point position = new Point(unit.Location.X, unit.Location.Y);
            lock (missiles)
                this.missiles.Add(new Missile(this, GameData.GetDefaultMissileImage(), position, new Point(dirX, dirY)));
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
    }
}
