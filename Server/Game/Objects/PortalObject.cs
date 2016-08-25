using System.Drawing;

namespace Server
{
    public class PortalObject : GenericObject
    {
        private const int COLLISION_RANGE = 64;

        private int mapId;

        public PortalObject(int uid, Point location, int mapId)
            :base(uid, location, COLLISION_RANGE)
        {
            this.mapId = mapId;
        }

        public override string ServerMessageMark { get { return "P"; } }

        public override string GetCodedData()
        {
            return base.GetCodedData() + "|" + mapId;
        }
    }
}
