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

            // TODO: initial location instead of dying location
            this.xLoc = unit.GetLocation().X;
            this.yLoc = unit.GetLocation().Y;
        }

        public void Process(MapInstance map)
        {
            IUnit newUnit = map.SpawnNPC(unitId, xLoc, yLoc);
            newUnit.AddDifference(new UnitSpawnedDifference(newUnit));
        }
    }
}
