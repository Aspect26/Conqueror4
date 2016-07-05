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
        public Character Character { get; set; }

        public Game(Character character)
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
        }

        private void RenderAll(Graphics g)
        {
            // player
            /*g.DrawImage(playerImage, Application.WIDTH / 2 - playerSize / 2, Application.HEIGHT / 2 - playerSize / 2,
                playerSize, playerSize);

            // map
            map.Render(g, character.Location);*/
        }
    }
}
