using System;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Editor.Logging
{
    public static class EditorLogSearch
    {
        private static event Action OnSearchStringChanged;

        private static string _searchString = string.Empty;

        private static string SearchString
        {
            get => _searchString;
            set
            {
                _searchString = value;
                OnSearchStringChanged?.Invoke();
            }
        }

        public static void Initialise(ToolbarSearchField searchField)
        {
            var search = searchField.Q<TextField>();
            search.RegisterValueChangedCallback(SearchValueChanged);
        }

        public static void SearchList(ListView listView)
        {
            OnSearchStringChanged += () =>
            {
                var index = EditorLogList.Loggers.IndexOf(EditorLogList.Loggers.FirstOrDefault(logger =>
                    logger.Contains(SearchString)));

                listView.ScrollToItem(index);
            };
        }
        
        private static void SearchValueChanged(ChangeEvent<string> change)
        {
            SearchString = change.newValue;
        }
    }
}