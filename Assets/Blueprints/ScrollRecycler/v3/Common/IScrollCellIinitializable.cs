namespace Blueprints.Scroller
{
    /// <summary>
    /// Provides the ability for a UI Component to be used within the <see cref="ScrollRecycler"/>
    /// </summary>
    /// <remarks>
    /// This interface is required for any UI Component to be used with the <see cref="ScrollRecycler"/>
    ///
    /// params object[] has been used to allow for the <see cref="ScrollRecycler"/> to remain open for any type of UI
    /// Component. Any class that implements this must be aware of the order of which their parameters are provided and cast
    /// them to their appropriate type.
    ///
    /// If this interface is implemented it is implying that the Implementer is to be used within the <see cref="ScrollRecycler"/>
    /// as such the methods should not be called externally to that system. It is advised that the class is Explicity implemented.
    /// </remarks>
    public interface IScrollCell
    {
        /// <summary>
        /// Initialises this Scroll Cell with the parameters provided
        /// </summary>
        /// <remarks>
        /// This method is called within the <see cref="ScrollRecycler"/> and should not be used externally to that
        /// system. The Initialisation method will be called by the <see cref="ScrollRecycler"/> this implementation is
        /// used within is Initialised.
        /// </remarks>
        /// <param name="args">The parameters required to initialise this object</param>
        void Init(params object[] args);
        
        /// <summary>
        /// Deinitialises this Scroll Cell
        /// </summary>
        /// <remarks>
        /// This method is called within the <see cref="ScrollRecycler"/> and should not be used externally to that
        /// system. The Deinit method will be called by the <see cref="ScrollRecycler"/> this implementation is
        /// used within as it is Deinitialised
        /// </remarks>
        void DeInit();
    }
}