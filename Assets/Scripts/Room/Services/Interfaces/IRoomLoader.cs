using System.Threading.Tasks;
using Incision.Room.Runtime;
using UnityEngine.SceneManagement;

namespace Incision.Room.Services.Interfaces
{
    public interface IRoomLoader
    {
        Task LoadAsync(RoomNode ctx, LoadSceneMode mode);
        Task UnloadAsync(string nodeId);
        bool IsLoaded(string nodeId);
    }
}
