using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class PlayScreen : EmptyScreen
    {
        private Image playerImage;
        private Stopwatch stopWatch;
        private long lastCycle;
        private const int playerSize = 50;

        private Game game;

        public PlayScreen(Application application): base(application)
        {
            Character character = application.Account.PlayCharacter;
            application.server.LoadCharacter(character);

            playerImage = GameData.GetCharacterImage(character.Spec);
            game = new Game(application.Account.PlayCharacter);

            stopWatch.Start();
            lastCycle = stopWatch.ElapsedMilliseconds;
        }

        public override void Render(Graphics g)
        {
            base.Render(g);

            int elapsedMilis = (int)(stopWatch.ElapsedMilliseconds - lastCycle);
            lastCycle = stopWatch.ElapsedMilliseconds;

            game.RunCycle(g, elapsedMilis);
        }

        // *************************************************
        // EVENTS
        // *************************************************
        public override void OnKeyDown(int key)
        {
            if (key == 68)      // D
            {

            }
            else if(key == 83)  // S
            {

            }
            else if(key == 65)   // A
            {

            }
            else if(key == 87)  // W
            {

            }
        }
    }
}
