using Blueprints.ScrollRecycler.v2.Core;

namespace Blueprints.ScrollRecycler.v2.Common
{
    /// <summary>
    /// Required identifier for Data used in the <see cref="DictScrollDataSource{TComponent,TData}"/>
    /// </summary>
    /// <remarks>
    /// This interface is presently required for data to be used with the <see cref="DictScrollDataSource{TComponent,TData}"/>
    ///
    /// It could be removed through creating a hash id string or some other form of identification to enable generic types
    /// to function within the <see cref="DictScrollDataSource{TComponent,TData}"/>. Another solution would be to wrap the
    /// Data class internally and provide it with an ID that is used internally inside of the system.
    /// </remarks>
    public interface IIdentifiableScrollData
    {
        /// <summary>
        /// The ID of the Data implementing this interface. Used Explicity to enable the <see cref="DictScrollDataSource{TComponent,TData}"/>
        /// to function with generic data Types
        /// </summary>
        string ScrollID { get; }
    }
}