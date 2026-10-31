using Incision.Interfaces;

namespace Incision.Portal.Structs.Events
{
    public readonly struct OnPortalEntered : IEvent
    {
        public string Message => "Entered Portal";
    }
}
