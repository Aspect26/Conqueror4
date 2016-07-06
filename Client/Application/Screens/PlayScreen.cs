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

        private PlayerCharacter playerCharacter;
        private Game game;
        private ServerConnection server;

        public PlayScreen(Application application, ServerConnection server) : base(application, server)
        {
            this.server = server;

            playerCharacter = application.Account.PlayCharacter;
            application.server.LoadCharacter(playerCharacter);

            playerImage = GameData.GetCharacterImage(playerCharacter.Spec);
            game = new Game(application.Account.PlayCharacter);

            stopWatch = new Stopwatch();
            stopWatch.Start();
            lastCycle = stopWatch.ElapsedMilliseconds;
        }

        public override void Render(Graphics g)
        {
            base.Render(g);

            int elapsedMilis = (int)(stopWatch.ElapsedMilliseconds - lastCycle);
            lastCycle = stopWatch.ElapsedMilliseconds;

            string msg = server.ReadOneMessage();
            if (msg != null)
                game.ProcessServerMessage(msg);

            game.RunCycle(g, elapsedMilis);
        }

        // *************************************************
        // EVENTS
        // *************************************************
        int previousDownKey = 0;
        public override void OnKeyDown(int key)
        {
            if (key == previousDownKey)
                return;

            previousDownKey = key;

            if (key == 68)      // D
            {
                playerCharacter.StartMovingRight();
                Console.WriteLine("START RIGHT");
            }
            else if(key == 83)  // S
            {
                playerCharacter.StartMovingBottom();
                Console.WriteLine("START BOTTOM");
            }
            else if(key == 65)   // A
            {
                playerCharacter.StartMovingLeft();
                Console.WriteLine("START LEFT");
            }
            else if(key == 87)  // W
            {
                playerCharacter.StartMovingUp();
                Console.WriteLine("START UP");
            }
        }

        public override void OnKeyUp(int key)
        {
            if (key == previousDownKey)
                previousDownKey = 0;

            if (key == 68)      // D
            {
                playerCharacter.StopMovingRight();
                Console.WriteLine("STOP RIGHT");
            }
            else if (key == 83)  // S
            {
                playerCharacter.StopMovingBottom();
                Console.WriteLine("STOP BOTTOM");
            }
            else if (key == 65)   // A
            {
                playerCharacter.StopMovingLeft();
                Console.WriteLine("STOP LEFT");
            }
            else if (key == 87)  // W
            {
                playerCharacter.StopMovingUp();
                Console.WriteLine("STOP UP");
            }
        }
    }
}
