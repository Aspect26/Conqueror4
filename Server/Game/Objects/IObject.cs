using System.Drawing;

namespace Server
{ 
    public interface IObject
    {
        int UniqueID { get; }
        Point Location { get; }
        int CollisionRange { get; }

        string ServerMessageMark { get; }
        string GetCodedData();
    }
}
