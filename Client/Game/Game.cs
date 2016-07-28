using System.Collections.Generic;
using System.Drawing;
using Shared;

namespace Client
{
    public partial class Game
    {
        public Map Map { get; set; }
        public PlayedCharacter Character { get; set; }
        private Dictionary<string, IUnit> units;

        public Game(PlayedCharacter character)
        {
            this.Character = character;
            this.CreateMap();

            this.units = new Dictionary<string, IUnit>();
        }

        private  void CreateMap()
        {
            Map = new Map();
            Map.Create(GameData.GetMapFilePath(Character.Location.MapID));
        }

        public void RunCycle(Graphics g, int timeSpan)
        {
            RenderAll(g);
            Character.PlayCycle(timeSpan);
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

        public Point MapPositionToScreenPosition(int x, int y)
        {
            int newX = Application.WIDTH / 2 - (int)Character.Location.X + x;
            int newY = Application.HEIGHT / 2 - (int)Character.Location.Y + y;

            return new Point(newX, newY); 
        }
    }
}
