using System;
using System.Collections.Generic;
using System.Linq;

namespace Logging
{
    public static class GameLog
    {
        public static event Action<string> OnLoggerAdded;
        public static event Action<string, bool> OnLoggerActiveChanged;
        public static event Action<bool> OnDebugModeActiveChanged;
        public static event Action OnLoggerDisabled;

        public static bool DebugModeActive { get; set; }

        public static IList<KeyValuePair<string, Log>> AllLoggers { get; private set; } = new List<KeyValuePair<string, Log>>();
        public static IList<KeyValuePair<string, Log>> ActiveLoggers { get; private set; } = new List<KeyValuePair<string, Log>>();
        public static IList<KeyValuePair<string, Log>> InActiveLoggers { get; private set; } = new List<KeyValuePair<string, Log>>();

        public static bool CheckActive(string loggerName)
        {
            var loggers = FindLoggers(loggerName);

            return loggers != null && loggers.First().Value.Enabled;
        }
        
        public static void ToggleLogger(string loggerName)
        {
            var logger = AllLoggers.FirstOrDefault(logger => logger.Key == loggerName).Value;

            if (logger.Enabled)
            {
                logger.Disable();

                return;
            }
            
            logger.Enable();
        }

        public static void SetLoggerActive(string loggerName, bool active)
        {
            InitLists();
            
            foreach (var logger in FindLoggers(loggerName))
            {
                SetLoggerActive(logger, active);
            }
            
            OnLoggerActiveChanged?.Invoke(loggerName, active);

            if (!active) { OnLoggerDisabled?.Invoke(); }
        }

        public static void SetDebugModeActive(bool active)
        {
            InitLists();
            DebugModeActive = active;
            OnDebugModeActiveChanged?.Invoke(DebugModeActive);

            Action action = DebugModeActive ? EnterDebugMode : ExitDebugMode;
            action.Invoke();
        }

        internal static void AddLogger(Log logger)
        {
            AllLoggers ??= new List<KeyValuePair<string, Log>>();
            
            AllLoggers.Add(new KeyValuePair<string, Log>(logger.ToString(), logger));
            OnLoggerAdded?.Invoke(logger.Name);
        }

        private static void InitLists()
        {
            AllLoggers = InitList(AllLoggers);
            ActiveLoggers = InitList(ActiveLoggers);
            InActiveLoggers = InitList(InActiveLoggers);
        }

        private static IList<T> InitList<T>(IList<T> list) => list.Any() ? list : new List<T>();

        private static IEnumerable<KeyValuePair<string, Log>> FindLoggers(string name)
        {
            AllLoggers ??= new List<KeyValuePair<string, Log>>();

            return AllLoggers.Where(item => item.Key == name);
        }

        private static void SetLoggerActive(KeyValuePair<string, Log> logger, bool active)
        {
            logger.Value.logEnabled = active;

            var addTo = active ? ActiveLoggers : InActiveLoggers;
            var removeFrom = active ? InActiveLoggers : ActiveLoggers;

            addTo.Remove(logger);
            removeFrom.Remove(logger);
            addTo.Add(logger);
        }

        private static void EnterDebugMode()
        {
            foreach (var logger in AllLoggers)
            {
                logger.Value.Enable();
            }
        }

        private static void ExitDebugMode()
        {
            foreach (var logger in AllLoggers)
            {
                logger.Value.Disable();
            }
        }
    }
}