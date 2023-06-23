using System.IO;
using UnityEngine;

namespace Blueprints.IO
{
    public static class IOOperations
    {
        // this will not work on mobile devices
        public static string PathForFilename(string filename)
            => Path.Combine(Application.persistentDataPath, filename);
    }
}