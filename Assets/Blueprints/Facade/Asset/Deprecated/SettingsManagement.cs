#nullable enable
using System;
using System.IO;
using System.Reflection;
using Blueprints.DoD.v1;
using Newtonsoft.Json;
using UnityEngine;

namespace Blueprints.Facade
{
    public static class SettingsManagement
    {
        /// <summary>
        /// Save will work for any type and can save all values from any classes it is handed.
        /// They are stored with the naming convention of [fieldname].[fieldname]...and so on
        /// A standard Setting<T> will be stored as [fieldname].value
        /// </summary>
        /// <param name="settings"></param>
        /// <typeparam name="T"></typeparam>
        [Obsolete("use specific settings asset type saving")]
        public static async void Save<T>(this T settings)
        {
            var json = JsonConvert.SerializeObject(settings);
            using var reader = new JsonTextReader(new StringReader(json));
            
            var newData = string.Empty;
            while (await reader.ReadAsync())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.StartObject or JsonToken.EndObject:
                        continue;
                    case JsonToken.PropertyName:
                        newData = reader.Path;
                        continue;
                }
                
                if (reader.Path != newData)
                {
                    continue;
                }
                
                SaveToPrefs(reader)?.Invoke();
                Debug.Log(reader.TokenType + " " + reader.Path);
            }
        }

        private static Action? SaveToPrefs(JsonReader reader)
        {
            return reader.Value == null ? null : reader.TokenType switch
            {
                JsonToken.Integer => () 
                    => PlayerPrefs.SetFloat(reader.Path, int.Parse(reader.Value.ToString())),
                JsonToken.Float => () 
                    => PlayerPrefs.SetFloat(reader.Path, float.Parse(reader.Value.ToString())),
                JsonToken.String => () 
                    => PlayerPrefs.SetString(reader.Path, reader.Value.ToString()),
                JsonToken.Boolean => () 
                    => PlayerPrefs.SetInt(reader.Path, reader.Value.ToString() == "True" ? 1 : 0),
                JsonToken.Date => () 
                    => PlayerPrefs.SetString(reader.Path, reader.Value.ToString()),
                JsonToken.Bytes => () 
                    => PlayerPrefs.SetString(reader.Path, reader.Value.ToString()),
                _ => null
            };
        }

        /// <summary>
        /// Loading of settings requires the target fieldtype to be of type
        /// Setting<T> where T : int, float, bool, DateTime, bytes
        ///
        /// Requires further insight in how to remove this limitation
        /// </summary>
        /// <param name="settings"></param>
        /// <typeparam name="T"></typeparam>
        [Obsolete("use specific settings asset type loading")]
        public static async void Load<T>(this T settings)
        {
            var json = JsonConvert.SerializeObject(settings);
            using var reader = new JsonTextReader(new StringReader(json));
            
            var fields = typeof(T).GetFields();

            var newData = string.Empty;
            while (await reader.ReadAsync())
            {
                // Debug.Log($"{reader.TokenType}\r\n{reader.Path}\r\n");
                
                switch (reader.TokenType)
                {
                    case JsonToken.StartObject or JsonToken.EndObject:
                        continue;
                    case JsonToken.PropertyName:
                        newData = reader.Path;
                        continue;
                }
            
                if (reader.Path != newData)
                {
                    continue;
                }
                
                foreach (var field in fields)
                {
                    if (newData.Contains(field.Name))
                    {
                        LoadFromPrefs(settings, field, reader);
                    }
                }
            }
        }

        private static void LoadFromPrefs<T>(T settings, FieldInfo field, JsonReader reader)
        {
            Action? action = reader.Value == null ? null : reader.TokenType switch
            {
                JsonToken.Integer
                    => () => field.SetValue(settings, 
                        new Data<int>(PlayerPrefs.GetInt(reader.Path, int.Parse(reader.Value.ToString())))),
                JsonToken.Float
                    => () => field.SetValue(settings,
                        new Data<float>(PlayerPrefs.GetFloat(reader.Path, float.Parse(reader.Value.ToString())))),
                JsonToken.String
                    => () => field.SetValue(settings, 
                        new Data<string>(PlayerPrefs.GetString(reader.Path, reader.Value.ToString()))),
                JsonToken.Boolean
                    => () => field.SetValue(settings, 
                        new Data<bool>(PlayerPrefs.GetInt(reader.Path, reader.Value.ToString() == "True" ? 1 : 0) == 1)),
                JsonToken.Date
                    => () => field.SetValue(settings, 
                        new Data<DateTime>(DateTime.Parse(PlayerPrefs.GetString(reader.Path, reader.Value.ToString())))),
                _ => null
            };
            
            Debug.Log(reader.TokenType + " " + reader.Path);

            if (action == null)
            {
                throw new LoadSettingsException("Target type is not recognised." +
                                                " LoadSettings will only work if target type is of basic type "+
                                                "Setting<T> where T : int, float, bool, DateTime, bytes");
            }
            
            action.Invoke();
        }
    }

    internal class LoadSettingsException : Exception
    {
        public LoadSettingsException(string message) : base(message) { }
    }
}