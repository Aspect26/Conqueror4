using System.Drawing;
using Shared;

namespace Client
{
    public class PlayedCharacter : SimpleUnit
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int Spec { get { return UnitID; }  }
        private ServerConnection server;

        // TODO !!!!!! remove this -> mock data
        public int HitPoints { get { return 68; } }
        public int MaxHitPoints { get { return 125; } }
        public int ManaPoints { get { return 25; } }
        public int MaxManaPoints { get { return 100; } }
        public int Experience { get { return 145; } }

        public PlayedCharacter(ServerConnection server, string name, int level, int spec, int uniqueId) 
            : base(null, spec, uniqueId, new Location())
        {
            this.Name = name;
            this.Level = level;
            this.server = server;
            this.animation = new CentreUnitAnimation(this, GameData.GetCharacterBasePath(spec));
        }

        // RENDERING
        private int playerSize = 50;
        public override void DrawUnit(Graphics g)
        {
            base.DrawUnit(g);

            g.DrawString(Name, GameData.GetFont(8), Brushes.Black,
                Application.WIDTH / 2 - playerSize / 2, Application.HEIGHT / 2 - playerSize / 2 - 20);
        }

        public override void PlayCycle(int timeSpan)
        {
            base.PlayCycle(timeSpan);
            if (moved)
            {
                server.SendPlayerLocation(this);
                moved = false;
            }
        }

        // MOVING COMMANDS
        // Start
        public override void StartMovingBottom()
        {
            base.StartMovingBottom();
        }

        public override void StartMovingRight()
        {
            base.StartMovingRight();
        }

        public override void StartMovingUp()
        {
            base.StartMovingUp();
        }

        public override void StartMovingLeft()
        {
            base.StartMovingLeft();
        }

        // Stop
        public override void StopMovingBottom()
        {
            base.StopMovingBottom();
        }

        public override void StopMovingLeft()
        {
            base.StopMovingLeft();
        }

        public override void StopMovingUp()
        {
            base.StopMovingUp();
        }

        public override void StopMovingRight()
        {
            base.StopMovingRight();
        }
    }
}
