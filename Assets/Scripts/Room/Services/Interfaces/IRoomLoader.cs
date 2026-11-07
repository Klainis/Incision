using System.Threading.Tasks;
using Incision.Room.Runtime;

namespace Incision.Room.Services.Interfaces
{
    public interface IRoomLoader
    {
        Task LoadAsync(RoomNode ctx);
        Task UnloadAsync(string nodeId);
        bool IsLoaded(string nodeId);
    }
}
