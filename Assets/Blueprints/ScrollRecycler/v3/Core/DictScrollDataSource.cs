using System;
using System.Collections.Generic;
using Blueprints.ScrollRecycler.v2.Common;

namespace Blueprints.Scroller
{
    /// <summary>
    /// A Data store for information used within the ScrollRecycler.
    /// </summary>
    /// <remarks>
    /// The Extension of <see cref="ScrollDataSource{TComponent,TData}"/> is to complement the class and functionality with a dictionary
    /// should that be necessary for the data store. Something to consider in the future is creating this as a decorator rather than an extension.
    /// A Dictionary version of the <see cref="ScrollDataSource{TComponent,TData}"/> is essential in some data scenarios
    /// where an Identifier is required to retrieve a specific data class at any time within the scroll. In such situations where data is networked or shared
    /// among others and such data can be updated at seemingly any time in the scroll. 
    /// </remarks>
    /// <inheritdoc cref="ScrollDataSource{TComponent,TData}"/> 
    public class DictScrollDataSource<TComponent, TData> : ScrollDataSource<TComponent, TData> where TData : IIdentifiableScrollData
    {
        private IDictionary<string, TData> Dict { get; } 
            = new Dictionary<string, TData>();

        /// <inheritdoc cref="ScrollDataSource{TComponent,TData}"/>
        public DictScrollDataSource(Action<TComponent, TData> setCell) : base(setCell) { }
        
        /// <inheritdoc cref="ScrollDataSource{TComponent,TData}"/>
        public override void AddData(params TData[] data)
        {
            foreach (var item in data)
            {
                if (!Dict.TryAdd(item.ScrollID, item)) 
                    continue;

                Data.Add(item);
                DataAdded?.Invoke(MaxIndex);
            }
        }

        /// <summary>
        /// Updates the Data in this Data Source if the provided data has a matching id with that presently stored
        /// This method Invokes <see cref="ScrollDataSource{TComponent,TData}.DataUpdated"/> so users of the
        /// <see cref="IScrollDataSource"/> are aware that data has been updated
        /// </summary>
        /// <param name="data">The Data that is to be updated in the Dictionary. This can be one or many for simplicity of implementation</param>
        public void UpdateData(params TData[] data)
        {
            foreach (var item in data)
            {
                if(Dict.TryGetValue(item.ScrollID, out var stored))
                    UpdateDataAtIndex(Data.IndexOf(stored), item);
            }
        }

        public override void ClearData()
        {
            Dict.Clear();
            base.ClearData();
        }
        
        /// <inheritdoc cref="ScrollDataSource{TComponent,TData}"/>
        public override void UpdateDataAtIndex(int index, TData data)
        {
            if (IndexWithinRange(index))
            {
                Data[index] = data;
                Dict[data.ScrollID] = data;
                DataUpdated?.Invoke(index);
            }
        }

        /// <inheritdoc cref="ScrollDataSource{TComponent,TData}"/>
        public override void RemoveDataAtIndex(int index)
        {
            if (IndexWithinRange(index))
            {
                Dict.Remove(Data[index].ScrollID);
                Data.RemoveAt(index);
                DataRemoved?.Invoke(index);
            }
        }
    }
}