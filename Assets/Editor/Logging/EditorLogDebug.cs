using System.Linq;
using Logging;
using UnityEngine.UIElements;

namespace Editor.Logging
{
    public static class EditorLogDebug
    {
        private const string _ACTIVE_DEBUG_CLASS = "debug-active";
        private const string _INACTIVE_DEBUG_CLASS = "debug-inactive";

        public static void Initialise(Button element)
        {
            SetDebugModeVisualsActive(element, GameLog.DebugModeActive);

            element.clicked += () => SetDebugModeActive(element);

            GameLog.OnDebugModeActiveChanged += change => SetDebugModeVisualsActive(element, change);

            GameLog.OnLoggerDisabled += () => 
            {
                GameLog.DebugModeActive = false;
                SetDebugModeVisualsActive(element, false);
            };
        }

        private static void SetDebugModeActive(VisualElement element)
        {
            GameLog.SetDebugModeActive(!GameLog.DebugModeActive);
            SetDebugModeVisualsActive(element, GameLog.DebugModeActive);
        }

        private static void SetDebugModeVisualsActive(VisualElement debugButton, bool active)
        {
            debugButton.RemoveFromClassList(active ? _INACTIVE_DEBUG_CLASS : _ACTIVE_DEBUG_CLASS);
            
            if (debugButton.GetClasses().Contains(active ? _ACTIVE_DEBUG_CLASS : _INACTIVE_DEBUG_CLASS))
            {
                return;
            }
            
            debugButton.AddToClassList(active ? _ACTIVE_DEBUG_CLASS : _INACTIVE_DEBUG_CLASS);
        }
    }
}