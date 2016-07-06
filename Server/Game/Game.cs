using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Game
    {
        private bool running = false;

        private List<IUnit> units;
        private Queue<IPlayerAction> playerActions;
        private Queue<ISendAction> sendActions;

        private Stopwatch stopwatch;

        private const int PLAYERACTIONS_PROCESSED_IN_ONE_CYCLE = 5;

        public void Initialize(Queue<IPlayerAction> playerActions, Queue<ISendAction> sendActions)
        {
            units = new List<IUnit>();
            this.playerActions = playerActions;
            this.sendActions = sendActions;

            stopwatch = new Stopwatch();
            stopwatch.Start();
            running = true;
        }

        public void Start()
        {
            long lastMiliseconds = stopwatch.ElapsedMilliseconds;

            while (running)
            {
                // process some player actions from queue
                lock (playerActions)
                {
                    lock (sendActions)
                    {
                        for (int i = 0; i < PLAYERACTIONS_PROCESSED_IN_ONE_CYCLE; i++)
                        {
                            if (playerActions.Count != 0)
                                sendActions.Enqueue(playerActions.Dequeue().Process(units));
                            else
                                break;
                        }
                    }
                }

                // get timespan
                long timeSpan = stopwatch.ElapsedMilliseconds - lastMiliseconds;
                lastMiliseconds = stopwatch.ElapsedMilliseconds;

                // move all units
                foreach (IUnit unit in units)
                {
                    unit.PlayCycle((int)timeSpan);
                }
            }
        }
    }
}
