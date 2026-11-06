using Incision.Room.Runtime;

namespace Incision.Room.Services.Interfaces
{
    public interface IRoomGraphGenerator
    {
        RoomGraph Generate(int roomCount);
    }
}
