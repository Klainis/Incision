using Incision.Events;
using Incision.Interfaces;
using UnityEngine;
using VContainer;

namespace Incision.UI.Components
{
    public class UICamera : MonoBehaviour
    {
        [SerializeField] private Camera uiCamera;
        [Inject] private IEventBus _eventBus;
        
        void OnEnable()
        {
            _eventBus.Subscribe<OnPauseChanged>(OnPauseChanged);
        }

        void OnDisable()
        {
            _eventBus.Unsubscribe<OnPauseChanged>(OnPauseChanged);
        }

        private void OnPauseChanged(OnPauseChanged evt)
        {
            uiCamera.enabled = !evt.IsPaused;
        }
    }
}
