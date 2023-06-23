using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Logging
{
    public class Debugger : IDisposable
    {
        public enum LogType
        {
            Message,
            Warning,
            Error
        }
        
        private readonly Logger _logger;
        
        public Debugger(string name, ILogHandler logHandler)
        {
            Name = name;
            _logger = new Logger(logHandler);
            GameLog.AddLogger(this);
        }
        
        public string Name { get; }

        public bool Enabled
        {
            get => _logger.logEnabled;
            set => _logger.logEnabled = value;
        }

        public void Log(string message, object obj, LogType type = LogType.Message)
        {
            Action<string, object> log = type switch
            {
                LogType.Message => _logger.Log,
                LogType.Warning => _logger.LogWarning,
                LogType.Error => _logger.LogError,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
            
            log.Invoke($"{Name}: {message}", obj);
        }

        public void Enable()
            => GameLog.SetLoggerActive(Name, true);
        
        public void Disable()
            => GameLog.SetLoggerActive(Name, false);

        public override string ToString() 
            => Name;

        public void Dispose()
        {
            var keyValuePair = GameLog.AllLoggers.FirstOrDefault(pair => pair.Value == this);

            if (keyValuePair.Equals(default(KeyValuePair<string, Debugger>))) { return; }

            GameLog.AllLoggers.Remove(keyValuePair);
        }
    }
}