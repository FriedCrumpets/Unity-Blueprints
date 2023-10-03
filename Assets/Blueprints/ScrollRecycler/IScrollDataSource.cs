using System;
using System.Collections.Generic;
using Bloktopia.V2.Apps;

namespace Bloktopia.UI.Scroller
{
    /// <summary>
    /// Provides a Data source with the ability to be used inside the <see cref="ScrollRecycler"/> to update the cells
    /// in the recycler with data as it is modified.
    /// </summary>
    /// <remarks> 
    /// This interface has default implementations used in <see cref="ScrollDataSource{TComponent,TData}"/>
    /// and <see cref="DictScrollDataSource{TComponent,TData}"/>.
    ///
    /// This interface must be implemented on any data source to be used with the <see cref="ScrollRecycler"/>.
    /// The Actions
    ///
    ///     DataAdded
    ///     DataUpdated
    ///     DataRemoved
    /// 
    /// are to be invoked with the index within a collection where the data has been modified. This implies the
    /// use of a list or array to achieve this, however this is not defined within the interface to as this is not a
    /// requirement of the user of the implementation.
    ///
    /// It's advised where necessary to use the default implementations, however, it remains as an interface in case
    /// a custom implementation is required to achieve the goals of the project.
    /// </remarks>
    public interface IScrollDataSource
    {
        /// <summary>
        /// Used to add data to the <see cref="ScrollRecycler"/> at the index provided when invoked.
        /// </summary>
        Action<int> DataAdded { get; set; }
        
        /// <summary>
        /// Used to update data to the <see cref="ScrollRecycler"/> at the index provided when invoked.
        /// </summary>
        Action<int> DataUpdated { get; set; }
        
        /// <summary>
        /// Used to remove data to the <see cref="ScrollRecycler"/> at the index provided when invoked.
        /// </summary>
        Action<int> DataRemoved { get; set; }
        
        /// <summary>
        /// Used to remove data to the <see cref="ScrollRecycler"/> at the index provided when invoked.
        /// </summary>
        Action DataCleared { get; set; }
        
        /// <summary>
        /// The Maximum index of the current collection in use
        /// </summary>
        /// <remarks>
        /// This index must be pointed to the maximum index of collection in use and is heavily used within the
        /// <see cref="ScrollRecycler"/> to prevent a provided index from being out of range 
        /// </remarks>
        int MaxIndex { get; }
        
        /// <summary>
        /// Sets the cell provided to the data at the index provided.
        /// </summary>
        /// <remarks>
        /// This method is explicitly called through the <see cref="IScrollDataSource"/> used inside of the <see cref="ScrollRecycler"/>
        /// and is not intended for general use external to this system. As such it is advised to be explicitly implemented.
        /// </remarks>
        /// <param name="cell">The Cell that is to be Set to the data at the index provided</param>
        /// <param name="index">The data index the provided cell is to be set to</param>
        /// <returns>true if cell has been successfully set to the data at the provided index; false if otherwise</returns>
        bool TrySetCell(object cell, int index);
    }
}