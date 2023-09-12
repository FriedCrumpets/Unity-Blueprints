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

        static GameLog()
        {
            AllLoggers = new List<KeyValuePair<string, Debugger>>();
            ActiveLoggers = new List<KeyValuePair<string, Debugger>>();
            InActiveLoggers = new List<KeyValuePair<string, Debugger>>();
        }
        
        public static bool DebugModeActive { get; set; }

        public static IList<KeyValuePair<string, Debugger>> AllLoggers { get; }
        public static IList<KeyValuePair<string, Debugger>> ActiveLoggers { get; }
        public static IList<KeyValuePair<string, Debugger>> InActiveLoggers { get; }

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
                logger.Deactivate();

                return;
            }
            
            logger.Activate();
        }

        public static void SetLoggerActive(string loggerName, bool active)
        {
            foreach (var logger in FindLoggers(loggerName))
            {
                SetLoggerActive(logger, active);
            }
            
            OnLoggerActiveChanged?.Invoke(loggerName, active);

            if (!active) { OnLoggerDisabled?.Invoke(); }
        }

        public static void SetDebugModeActive(bool active)
        {
            DebugModeActive = active;
            OnDebugModeActiveChanged?.Invoke(DebugModeActive);

            Action action = DebugModeActive ? EnterDebugMode : ExitDebugMode;
            action.Invoke();
        }

        internal static void AddLogger(Debugger logger)
        {
            AllLoggers.Add(new KeyValuePair<string, Debugger>(logger.ToString(), logger));
            OnLoggerAdded?.Invoke(logger.Name);
        }
        
        private static IEnumerable<KeyValuePair<string, Debugger>> FindLoggers(string name)
            => AllLoggers.Where(item => item.Key == name);

        private static void SetLoggerActive(KeyValuePair<string, Debugger> pair, bool active)
        {
            pair.Value.Enabled = active;

            var addTo = active ? ActiveLoggers : InActiveLoggers;
            var removeFrom = active ? InActiveLoggers : ActiveLoggers;

            addTo.Remove(pair);
            removeFrom.Remove(pair);
            addTo.Add(pair);
        }

        private static void EnterDebugMode()
        {
            foreach (var logger in AllLoggers)
                logger.Value.Activate();
        }

        private static void ExitDebugMode()
        {
            foreach (var logger in AllLoggers)
                logger.Value.Deactivate();
        }
    }
}