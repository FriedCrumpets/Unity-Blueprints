using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Logging
{
    public class EditorLogWindow : EditorWindow
    {
        private const string _WINDOW_NAME = "Logging";
        
        private const string _WINDOW_ASSET_PATH = "Assets/UIToolkit/Windows/GameLoggerV3.uxml";

        private static readonly Vector2 MinimumSize = new(300, 550);
        
        private static bool _windowOpen;

        [MenuItem("Window/Utilities/Logging")]
        public static void ShowWindow()
        {
            if (_windowOpen) { return; }
            
            var window = GetWindow(typeof(EditorLogWindow));
            window.minSize = MinimumSize;
            window.titleContent.text = _WINDOW_NAME;

            _windowOpen = true;
        }

        private void CreateGUI()
        {
            var root = rootVisualElement;

            var mainWindow = EditorUtils.CreateAsset(_WINDOW_ASSET_PATH);
            root.Add(mainWindow);
            
            var debugRoot = root.Q("DebugMode");
            var debug = debugRoot.Q<Button>("CustomToggle");
            EditorLogDebug.Initialise(debug);

            var searchbar = root.Q<ToolbarSearchField>("LoggerSearch");
            EditorLogSearch.Initialise(searchbar);

            var listView = root.Q<ListView>("LoggerList");
            EditorLogList.PopulateListView(listView);
            EditorLogSearch.SearchList(listView);
        }

        private void OnDestroy() => _windowOpen = false;
    }
}