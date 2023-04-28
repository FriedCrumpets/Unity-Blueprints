using System;
using Newtonsoft.Json.Linq;

namespace Blueprints.IO
{
    /// <summary>
    /// Provides saving logic to Monobehaviours
    /// </summary>
    /// <remarks>
    /// When implemented by Monobehaviours this provides them to a saving feature saving whatever has been passed to
    /// <see cref="SavedData"/> Saved Data 
    /// </remarks>
    public interface ISaveable
    {
        /// <summary>
        /// The ID Reference for this saved object
        /// </summary>
        Guid SaveID { get; }
        
        /// <summary>
        /// The data saved for this object
        /// </summary>
        JObject SavedData { get; }
        
        /// <summary>
        /// How the data will be loaded back into this object 
        /// </summary>
        /// <param name="data">the saved data for this object</param>
        void LoadFromData(JObject data);
    }
}