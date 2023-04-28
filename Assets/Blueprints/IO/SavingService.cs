using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Blueprints.IO
{
    public static class SavingService
    {
        private const string _ACTIVE_SCENE_KEY = "activeScene";
        private const string _SCENES_KEY = "scenes";
        private const string _OBJECTS_KEY = "objects";
        private const string _SAVE_ID_KEY = "$saveID";

        private static UnityAction<Scene, LoadSceneMode> _loadObjectsAfterSceneLoad;

        public static void SaveGame(string fileName)
        {
            // Create empty JObject which will be used to save to disk
            var result = new JObject();

            var saveableObjects = Object
                .FindObjectsOfType<MonoBehaviour>()
                .OfType<ISaveable>();

            var enumerable = saveableObjects as ISaveable[] ?? saveableObjects.ToArray();
            if (enumerable.Any())
            {
                var savedObjects = new JObject();
                foreach (var saveableObject in enumerable)
                {
                    var data = saveableObject.SavedData;

                    if (data.HasValues)
                    {
                        data[_SAVE_ID_KEY] = saveableObject.SaveID;
                        savedObjects.Add(data);
                    }
                    else
                    {
                        var behaviour = saveableObject as MonoBehaviour;
                        
                        Debug.LogWarning($"{behaviour!.name}, save data could not be saved");
                    }
                }
            }
            else
            {
                Debug.LogWarning("The scene did not include any saveable objects");
            }

            // used to store the list of open scenes
            var openScenes = new JObject();
            
            // ask the scene manager how many scenes are open and for each one store its name
            foreach (var scene in Enumerable.Range(1, SceneManager.sceneCount))
                openScenes.Add(SceneManager.GetSceneAt(scene));

            result[_SCENES_KEY] = openScenes;

            // store the active scene name
            result[_ACTIVE_SCENE_KEY] = SceneManager.GetActiveScene().name;
            
            // save data is now generated in full so we'll now write it to disk
            result.WriteTo(new JsonTextWriter(File.CreateText(IOOperations.PathForFilename(fileName))));

            result = null;
            System.GC.Collect();
        }

        public static bool LoadGame(string fileName)
        {
            var filePath = IOOperations.PathForFilename(fileName);
            
            if (!File.Exists(filePath))
            {
                Debug.LogError($"[{nameof(SavingService)}]: No file exists at {filePath}");
                return false;
            }

            // Read data from file back into a JObject
            var data = (JObject)JToken.ReadFrom(new JsonTextReader(File.OpenText(filePath)));

            if (!data.HasValues)
            {
                Debug.LogError($"[{nameof(SavingService)}]: Data at {filePath} is not a JSON Object");
                return false;
            }

            if (!data.ContainsKey(_SCENES_KEY))
            {
                Debug.LogWarning($"Data at {filePath} does not contain any scenes to load!");
                return false;
            }

            // Get a list of scenes
            var scenes = data[_SCENES_KEY];
            var sceneCount = scenes!.Count();
            if (sceneCount == 0)
            {
                Debug.LogWarning($"Data at {filePath} does not specify any scenes to load");
                return false;
            }

            for (var i = 0; i < sceneCount; i++)
                SceneManager.LoadScene((string)scenes[i], LoadSceneMode.Additive);

            if (data.ContainsKey(_ACTIVE_SCENE_KEY))
            {
                var activeScene = SceneManager.GetSceneByName((string)data[_ACTIVE_SCENE_KEY]);

                if (!activeScene.IsValid())
                {
                    Debug.LogError($"Data at {filePath} specifies an active scene that does not exist");
                    return false;
                }

                SceneManager.SetActiveScene(activeScene);
            }
            else
            {
                Debug.LogWarning($"Data at {filePath} does not specify an active scene");
            }

            if (data.ContainsKey(_OBJECTS_KEY))
            {
                var objects = data[_OBJECTS_KEY];

                _loadObjectsAfterSceneLoad = (scene, loadSceneMode) =>
                {
                    //find all saveable objects and build dictionary, using SaveID as the key
                    var allLoadables = Object
                        .FindObjectsOfType<MonoBehaviour>()
                        .OfType<ISaveable>()
                        .ToDictionary(obj => obj.SaveID, o => o);

                    for (var i = 0; i < objects!.Count(); i++)
                    {
                        var objData = (JObject)objects[i];
                        var id = (Guid)objData![_SAVE_ID_KEY];
                        
                        // try to find the object in the scene with the save ID
                        if (allLoadables.ContainsKey(id))
                            allLoadables[id].LoadFromData(objData);
                    }

                    SceneManager.sceneLoaded -= _loadObjectsAfterSceneLoad;
                    _loadObjectsAfterSceneLoad = null;

                    GC.Collect();
                };

                SceneManager.sceneLoaded += _loadObjectsAfterSceneLoad;
            }

            return true;
        }
        
        /*
         * If I want to integrate such a system with addressables I'll need to strip the scene for base objects
         * Create/ store guids for each object and create the object as an addressable; store such information as Json
         * and then fire it up to a server. I don't think that'll be too heavily complex... hmmm....
         */
    }
}