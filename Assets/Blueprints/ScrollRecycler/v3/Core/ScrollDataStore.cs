using System;
using System.Collections.Generic;
using System.Linq;
using Blueprints.ScrollRecycler.v2.Common;

namespace Blueprints.Scroller
{
    /// <summary>
    /// A Data store for information used within the ScrollRecycler.
    /// </summary>
    /// <remarks>
    /// An implementation of the <see cref="IScrollDataSource"/> with a simple list of TData to provide a simple entry point
    /// for data into the <see cref="ScrollRecycler"/>
    /// 
    /// The entrance into the ScrollRecycler is not this class itself but the <see cref="IScrollDataSource"/> interface.
    /// This class provides a generic implementation of the interface to simplify entering into the system, without requiring
    /// a personalised implementation for the solution; hopefully reducing class bloat. The Interface remains so to customise
    /// an implementation into the system should that be necessary.
    /// </remarks>
    /// <typeparam name="TComponent">The UI Component that <see cref="TData"/> is to update</typeparam>
    /// <typeparam name="TData">The Data Type used to update this UI Component</typeparam>
    public class ScrollDataSource<TComponent, TData> : IScrollDataSource
    {
        private readonly Action<TComponent, TData> _setCell;
        protected IList<TData> Data { get; } = new List<TData>();

        /// <inheritdoc cref="IScrollDataSource"/>
        public Action<int> DataAdded { get; set; }
        
        /// <inheritdoc cref="IScrollDataSource"/>
        public Action<int> DataUpdated { get; set; }
        
        /// <inheritdoc cref="IScrollDataSource"/>
        public Action<int> DataRemoved { get; set; }
        
        /// <inheritdoc cref="IScrollDataSource"/>
        public Action DataCleared { get; set; }

        /// <inheritdoc cref="IScrollDataSource"/>
        public int MaxIndex
            => Data.Count - 1;

        /// <summary>
        /// Constructs the object with an Action which determines how the data {TData} will interact/modify the UI {TComponent}. 
        /// </summary>
        /// <remarks>
        /// This has been used to enable full polymorphic affect within the class; rather than require this class to be extended for the
        /// desired functionality it is instead injected here. 
        /// </remarks>
        /// <param name="setCell">The Action used to update TComponent when it is made visible on screen</param>
        public ScrollDataSource(Action<TComponent, TData> setCell)
        {
            _setCell = setCell;
        }

        /// <summary>
        /// Adds data to this Data Source; invoking <see cref="ScrollDataSource{TComponent,TData}.DataAdded"/> so users of the
        /// <see cref="IScrollDataSource"/> are aware that data has been added
        /// </summary>
        /// <param name="data">The Data that is to be added to the Data Source. This can be one or many for simplicity of implementation</param>
        public virtual void AddData(params TData[] data)
        {
            foreach (var item in data)
            {
                Data.Add(item);
                DataAdded?.Invoke(MaxIndex);
            }
        }

        /// <summary>
        /// Attempts to locate and provide data based on the index supplied to the method.
        /// </summary>
        /// <param name="index">The Index of the data attempted to be retrieved</param>
        /// <param name="data"></param>
        /// <returns> If the provided index is in range this will return the data and true; otherwise null and false </returns>
        public virtual bool TryGetData(int index, out TData data)
        {
            var success = IndexWithinRange(index);
            data = success ? Data[index] : default;
            return success;
        }

        /// <summary>
        /// Confirms if the Data provided is contained within this DataSource
        /// </summary>
        /// <param name="data">The Data to be confirmed as present within the store</param>
        /// <returns>ture if data is found; otherwise false</returns>
        public virtual bool ContainsData(TData data)
            => Data.Contains(data);
        
        /// <summary>
        /// Updates the Data in this Data Source at the index provided with the data provided.
        /// This method Invokes <see cref="ScrollDataSource{TComponent,TData}.DataUpdated"/> so users of the
        /// <see cref="IScrollDataSource"/> are aware that data has been updated
        /// </summary>
        /// <param name="index">The Index of the data to be updated; this index will be checked to confirm it is within index range</param>
        /// <param name="data">The Data to update the Index with</param>
        public virtual void UpdateDataAtIndex(int index, TData data)
        {
            if (IndexWithinRange(index))
            {
                Data[index] = data;
                DataUpdated?.Invoke(index);
            }
        }

        /// <summary>
        /// Removes the provided data from this data source 
        /// This method Invokes <see cref="ScrollDataSource{TComponent,TData}.DataRemoved"/> so users of the
        /// <see cref="IScrollDataSource"/> are aware that data has been removed
        /// </summary>
        /// <param name="data">The Data that is to be removed from the Data Source. This can be one or many for simplicity of implementation</param>
        public virtual void RemoveData(params TData[] data)
        {
            foreach(var item in data)
                RemoveDataAtIndex(Data.IndexOf(item));
        }

        /// <summary>
        /// Removes the data from this data source at the provided Index
        /// This method Invokes <see cref="ScrollDataSource{TComponent,TData}.DataRemoved"/> so users of the
        /// <see cref="IScrollDataSource"/> are aware that data has been removed
        /// </summary>
        /// <param name="index">The Index of the data to be removed; this index will be checked to confirm it is within index range</param>
        public virtual void RemoveDataAtIndex(int index)
        {
            if (IndexWithinRange(index))
            {
                Data.RemoveAt(index);
                DataRemoved?.Invoke(index);
            }
        }

        /// <summary>
        /// Clears all data the DataSource. This method links to <see cref="RemoveDataAtIndex"/> To ensure Data removal
        /// is confirmed with the <see cref="IScrollDataSource"/> interface
        /// </summary>
        public virtual void ClearData()
        {
            RemoveData(Data.ToArray());
            Data.Clear();
            DataCleared?.Invoke();
        }
        
        /// <summary>
        /// Implementation of the <see cref="IScrollDataSource"/>. Checks if the cell object is of the
        /// correct type and that the provided index is within index range. Once confirmed the <see cref="_setCell"/>
        /// Action is invoked on the cell with the converted component and the Data at the index.
        /// </summary>
        /// <inheritdoc cref="IScrollDataSource"/>
        bool IScrollDataSource.TrySetCell(object cell, int index)
        {
            if (cell is not TComponent component)
                return false;

            if (index > MaxIndex )  
                return false;

            if( index >= 0 == false)
                return false;
            
            _setCell?.Invoke(component, Data[index]);
            return true;
        }
        
        protected bool IndexWithinRange(int index)
            => index >= 0 && index <= MaxIndex;
    }
}