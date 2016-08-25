using System;
using System.Drawing;

namespace Server
{
    public abstract class GenericObject : IObject
    {
        public int UniqueID { get; }
        public Point Location { get; protected set; }
        public int CollisionRange { get; protected set; }

        public abstract string ServerMessageMark { get; }

        public GenericObject(int uid, Point location, int collisionRange)
        {
            this.UniqueID = uid;
            this.Location = location;
            this.CollisionRange = collisionRange;
        }

        public virtual string GetCodedData()
        {
            return ServerMessageMark + "|" + UniqueID + "|" + Location.X + "|" + Location.Y 
                + "|" + CollisionRange;
        }

        public static GenericObject Create(ObjectInfo info, int uid)
        {
            switch (info.Mark)
            {
                case 'P':
                    return new PortalObject(uid, new Point(info.X, info.Y),
                        Convert.ToInt32(info.SpecialArguments[0]));
                default:
                    throw new NotImplementedException("Tries to create an unknown object of " +
                        "type :" + info.Mark);
            }
        }
    }
}
