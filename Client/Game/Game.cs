using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public partial class Game
    {
        public Map Map { get; set; }
        public MainPlayerCharacter Character { get; set; }
        private List<IUnit> units;

        public Game(MainPlayerCharacter character)
        {
            this.Character = character;
            this.CreateMap();

            this.units = new List<IUnit>();
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
            foreach (IUnit unit in units)
                unit.DrawUnit(g);

            // player
            Character.DrawUnit(g);
        }

        public void AddUnit(string name, int id, int x, int y)
        {
            units.Add(new PlayerUnit(this, name, id, x, y));
        }

        public Point MapPositionToScreenPosition(int x, int y)
        {
            int newX = Application.WIDTH / 2 - Character.Location.X + x;
            int newY = Application.HEIGHT / 2 - Character.Location.Y + y;

            return new Point(newX, newY); 
        }
    }
}
