namespace Blueprints.Utility
{
    /// <summary>
    /// T would ordinarily want to be an interface to enable services to be maluable to further requirements
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class SimpleLocator<T>
    {
        public static T Service { get; set; }
    }
}