namespace Incision.Interfaces
{
    public interface IEventBus : IService
    {
        void Publish<T>(T evt);
        void Subscribe<T>(System.Action<T> handler);
        void Unsubscribe<T>(System.Action<T> handler);
    }
}