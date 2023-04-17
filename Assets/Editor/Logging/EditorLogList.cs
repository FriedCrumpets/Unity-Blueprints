using System;
using System.Collections.Generic;   
using Logging;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Logging
{
    public static class EditorLogList
    {
        private const string _LOGGER_ASSET_PATH = "Assets/UIToolkit/Templates/CustomToggle.uxml";
        private const string _ACTIVE_LOGGER_CLASS = "unity-toggle__active";

        public static List<string> Loggers { get; private set; } = new();
        private static List<VisualElement> LoggerElements { get; } = new();
        
        public static void PopulateListView(ListView element)
        {
            Loggers = RetrieveAllLoggers();
            element.itemsSource = Loggers;
            element.selectionType = SelectionType.None;
            
            element.makeItem = () =>
            {
                var item = CreateEmptyGameLogger();
                item.style.width = Length.Percent(100);
                LoggerElements.Add(item);

                item.RegisterCallback<ClickEvent>(_ =>
                {
                    GameLog.ToggleLogger(Loggers[(int)item.userData]);
                });
                
                return item;
            };
            
            element.bindItem = (bind, index) =>
            {
                if (index > Loggers.Count - 1) { return; }

                bind.userData = index;
                
                UpdateLogger(bind, Loggers[index]);
            };

            GameLog.OnLoggerActiveChanged += SetClassActive;
            
            GameLog.OnLoggerAdded += (loggerName) =>
            {
                if (Loggers.Contains(loggerName))
                {
                    return;
                }
                
                Loggers.Add(loggerName);
                element.Rebuild();
            };
        }
        
        private static List<string> RetrieveAllLoggers()
        {
            List<string> loggers = new();
            foreach (var logger in GameLog.AllLoggers)
            {
                if(loggers.Contains(logger.Key)){ continue; }

                loggers.Add(logger.Key);
            }
            
            return loggers;
        }

        private static void SetClassActive(string loggerName, bool active)
        {
            if (!Loggers.Contains(loggerName)) { return; }
            
            var logger = EditorUtils.FindElementByName(LoggerElements, EditorUtils.StringToElementName(loggerName));

            if (logger == null)
            {
                return;
            }
            
            var checkmark = logger.Q("ToggleCheckmark");
            
            Action<string> action = active ? checkmark.AddToClassList : checkmark.RemoveFromClassList;
            action(_ACTIVE_LOGGER_CLASS);
        }
        
        private static void UpdateLogger(VisualElement element, string loggerName)
        {
            element.name = EditorUtils.StringToElementName(loggerName);
            var label = element.Q<Label>("ToggleName");
            label.text = loggerName;
            SetClassActive(loggerName, GameLog.CheckActive(loggerName));
        }
        
        private static VisualElement CreateEmptyGameLogger() => EditorUtils.CreateAsset(_LOGGER_ASSET_PATH);
    }
}