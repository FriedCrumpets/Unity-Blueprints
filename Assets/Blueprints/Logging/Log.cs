using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Logging
{
    public class Log : Logger, IDisposable
    {
        public Log(string name, ILogHandler logHandler) : base(logHandler)
        {
            Name = name;
            GameLog.AddLogger(this);
        }
        
        public string Name { get; }

        public bool Enabled => logEnabled;

        public void Enable()
            => GameLog.SetLoggerActive(Name, true);
        
        public void Disable()
            => GameLog.SetLoggerActive(Name, false);

        public override string ToString() => Name;

        public void Dispose()
        {
            var keyValuePair = GameLog.AllLoggers.FirstOrDefault(pair => pair.Value == this);

            if (keyValuePair.Equals(default(KeyValuePair<string, Log>))) { return; }

            GameLog.AllLoggers.Remove(keyValuePair);
        }
    }
}