using Incision.Interfaces;
using Incision.Room.Runtime;

namespace Incision.Portal.Structs.Events
{
    public readonly struct OnPortalEntered : IEvent
    {
        public PortalLink PortalLink { get; }
        public string Message => "Entered Portal";

        public OnPortalEntered(PortalLink link)
        {
            PortalLink = link;
        }
    }
}
