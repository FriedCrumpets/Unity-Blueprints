using UnityEngine;

namespace RequireInterface
{
    /// <summary>
    /// Attribute that require implementation of the provided interface.
    /// </summary>
    public class RequireInterfaceAttribute : PropertyAttribute
    {
        // Interface type.
        public System.Type RequiredType { get; private set; }

        /// <summary>
        /// Requiring implementation of the <see cref="T:RequireInterface.RequireInterfaceAttribute"/> interface.
        /// </summary>
        /// <param name="type">Interface type.</param>
        public RequireInterfaceAttribute(System.Type type)
        {
            RequiredType = type;
        }
    }
}