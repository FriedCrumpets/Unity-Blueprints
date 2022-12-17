#nullable enable
using System;
using UnityEngine;

namespace Blueprints.Command
{
    public class Command<T> : ICommand
    {
        public Command(T receiver, Action<T> execute, Action<T>? undo = null)
        {
            _receiver = receiver;
            _execute = execute;
            _undo = undo;
        }

        private readonly T _receiver;
        private readonly Action<T> _execute;
        private readonly Action<T>? _undo;

        /// <summary>
        /// Immediately Execute the command into the Command Stream
        /// </summary>
        public void Execute() => _execute.Invoke(_receiver);

        /// <summary>
        /// Immediately undo the command and set its state into the command stream
        /// </summary>
        public void Undo() => _undo?.Invoke(_receiver);

        public void Buffer() => CommandBuffer.Buffer(this);
    }
    
    [Serializable]
    public class Option<T> : Command<T>
    {
        public Option(T receiver, Action<T> execute, Action<T>? undo = null) : base(receiver, execute, undo)
        {
            DisplayName = PrimaryName = execute.Method.Name;

            if (undo != null)
            {
                SecondaryName = undo.Method.Name;
            }
        }
        
        public Option(T receiver, Action<T> execute, string displayName, Action<T>? undo = null) 
            : base(receiver, execute, undo)
        {
            DisplayName = displayName;
            PrimaryName = execute.Method.Name;

            if (undo != null)
            {
                SecondaryName = undo.Method.Name;
            }
        }

        [field: SerializeField] public string DisplayName { get; private set; }
        public string PrimaryName { get; private set; } = string.Empty;
        public string SecondaryName { get; private set; } = string.Empty;
        
        public bool Enabled { get; private set; } = true;

        public void Enable() => Enabled = true;

        public void Disable() => Enabled = false;
        
        public void ChangeDisplayName(string newName)
        {
            DisplayName = newName;
        }
    }
}