namespace Atlas.Abstractions
{
    /// <summary>
    /// A class for map objects.
    /// </summary>
    public abstract class MapObject
    {
        /// <summary>
        /// Gets the object's network ID.
        /// </summary>
        public abstract uint NetId { get; }
    }
}
