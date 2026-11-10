using Incision.Extensions;
using Incision.Interfaces;
using UnityEngine;

namespace Incision.Managers
{
    public class CursorManager : ITaggable
    {
        private bool _visible = true;
        public void ToggleCursor()
        {
            if (_visible)
                HideCursor();
            else
                ShowCursor();
            
        }
        public void HideCursor()
        {
            if (!_visible)
                return;
            
            _visible = false;
            ChangeCursorVisibility(_visible);
        }
        
        public void ShowCursor()
        {
            if (_visible)
                return;
            
            _visible = true;
            ChangeCursorVisibility(_visible);
        }

        private void ChangeCursorVisibility(bool visible)
        {
            Cursor.visible = visible;
            Cursor.lockState = visible
                ? CursorLockMode.None
                : CursorLockMode.Locked;
        }

        public string LOG_TAG { get; } = "[CursorManager]".AsTag(Color.yellow);
    }
}
