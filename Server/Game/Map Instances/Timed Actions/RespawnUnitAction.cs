namespace Server
{
    /// <summary>
    /// Every NPC Unit that is killed in the game shall be respawned after a specific 
    /// time. This timed action takes care of this.
    /// </summary>
    /// <seealso cref="Server.ITimedAction" />
    public class RespawnUnitAction : ITimedAction
    {
        private int unitId;
        private int xLoc;
        private int yLoc;

        /// <summary>
        /// Initializes a new instance of the <see cref="RespawnUnitAction"/> class.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public RespawnUnitAction(IUnit unit)
        {
            this.unitId = unit.UnitID;

            this.xLoc = unit.SpawnPosition.X;
            this.yLoc = unit.SpawnPosition.Y;
        }

        /// <summary>
        /// Respawn the died NPC unit.
        /// </summary>
        /// <param name="map">The map instance it belongs to.</param>
        public void Process(MapInstance map)
        {
            IUnit newUnit = map.SpawnNPC(unitId, xLoc, yLoc);
            newUnit.AddDifference(new UnitSpawnedDifference(newUnit));
        }
    }
}
