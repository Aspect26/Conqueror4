using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Game
    {
        public Map Map { get; set; }
        public PlayerCharacter Character { get; set; }

        public Game(PlayerCharacter character)
        {
            this.Character = character;
            this.CreateMap();
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

            // player
            Character.DrawUnit(g);
        }
    }
}
