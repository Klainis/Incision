using Incision.Interfaces;

namespace Incision.UI.Events
{
    public readonly struct OnUIShowMainMenu : IEvent
    {
        public string Message => "OnUIShowMainMenu";
    }
}
