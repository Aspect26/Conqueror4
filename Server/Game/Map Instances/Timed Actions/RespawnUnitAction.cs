namespace Server
{
    public class RespawnUnitAction : ITimedAction
    {
        private int unitId;
        private int xLoc;
        private int yLoc;

        public RespawnUnitAction(IUnit unit)
        {
            this.unitId = unit.UnitID;

            this.xLoc = unit.SpawnPosition.X;
            this.yLoc = unit.SpawnPosition.Y;
        }

        public void Process(MapInstance map)
        {
            IUnit newUnit = map.SpawnNPC(unitId, xLoc, yLoc);
            newUnit.AddDifference(new UnitSpawnedDifference(newUnit));
        }
    }
}
