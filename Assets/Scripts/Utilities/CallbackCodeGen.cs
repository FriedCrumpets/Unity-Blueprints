using System;
using System.IO;
using System.Linq;

#if UNITY_EDITOR
namespace Blueprints.Facade
{
    public class CallbackCodeGen
    {
        public void GenerateCode(Type @class, string path)
        {
            var name = @class.Name.Replace("Actions", "");
            path += $"/{name}Ext.cs";
            
            if (File.Exists(path))
            {
                throw new CodeGenException("Callback Code Generator cannot overwrite existing files, please remove the file before generating a new one");
            }
            
            var template = CreateCallbacks(@class, name);
            
            File.WriteAllText(path, template);
        }

        private static string CreateCallbacks(Type @class, string name)
        {
            var events = @class.GetEvents();

            if (events.Length < 1)
            {
                throw new CodeGenException("Attempting to create a callback script for a script without events to subscribe to");
            }
            
            var interfaceName = $"IControl{name}";
            var className = $"{name}Ext";

            var template = $@"using System.Collections.Generic;

namespace Bloktopia.Input
{{
    /// <summary>
    /// 
    /// </summary>
    public static class {className}
    {{
        public interface {interfaceName}
        {{";

            foreach (var @event in events)
            {
                var @params = @event.EventHandlerType.GetMethod("Invoke")?.GetParameters();

                var args
                    = @params.Select((t, i) => $"{ConvertType(t.ParameterType.ToString())} arg{i + 1}").ToList();

                var kwargs = args.Count == 0 ? string.Empty : string.Join(',', args);
                template += $"\r\n\t\t\tvoid {@event.Name}({kwargs});";
            }

            template += $@"
        }}

        private static List<{interfaceName}> Callbacks {{ get; set; }}
        
        public static void AddCallbacks(this {@class.Name} map, {interfaceName} callbacks)
        {{
            Callbacks ??= new List<{interfaceName}>();
            Callbacks.Add(callbacks);
            
            ";

            foreach (var @event in events)
            {
                template += $"map.{@event.Name} += callbacks.{@event.Name};\r\n\t\t\t";
            }
            
            template += $@"
        }}

        public static void RemoveCallbacks(this {@class.Name} map, {interfaceName} callbacks)
        {{
            Callbacks ??= new List<{interfaceName}>();
            var callback 
                = Callbacks.Find(callback => callback == callbacks);
            
            ";
            
            foreach (var @event in events)
            {
                template += $"map.{@event.Name} -= callbacks.{@event.Name};\r\n\t\t\t";
            }

            template += @"
            Callbacks.Remove(callback);
        }
    }
}";

            return template;
        }

        private static string ConvertType(string type)
        {
            return type switch
            {
                "System.Boolean" => "bool",
                "System.String" => "string",
                "System.Object" => "object",
                "System.Byte" => "byte",
                "System.SByte" => "sbyte",
                "System.Char" => "char",
                "System.Decimal" => "decimal",
                "System.Double" => "double",
                "System.Single" => "float",
                "System.Int32" => "int",
                "System.UInt32" => "uint",
                "System.IntPtr" => "nint",
                "System.UIntPtr" => "nuint",
                "System.Int64" => "long",
                "System.UInt64" => "ulong",
                "System.Int16" => "short",
                "System.UInt16" => "ushort",
                _ => type
            };
        }
    }

    public class CodeGenException : Exception
    {
        public CodeGenException(string message) : base(message) { }
    }
}
#endif
