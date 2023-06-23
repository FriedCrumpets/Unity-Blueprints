using System;
using UnityEngine;

namespace Logging
{
        public class SessionLog
        {
                private DateTime _sessionStartTime;
                private DateTime _sessionEndTime;
                
                public static Debugger Log { get; } = new("SessionLog", Debug.unityLogger.logHandler);

                public void StartSession()
                {
                        _sessionStartTime= DateTime.Now;
                        Log.Log($"Session Start Time: {_sessionStartTime}", this);
                }

                public void EndSession()
                {
                        _sessionEndTime = DateTime.Now;
                        var timeDifference = _sessionEndTime.Subtract(_sessionStartTime);

                        Log.Log($"Session End Time: {_sessionEndTime}", this);
                        Log.Log($"Session Time Elapsed: {timeDifference}", this);
                }
        }
}