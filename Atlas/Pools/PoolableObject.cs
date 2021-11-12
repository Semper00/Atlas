namespace Atlas.Pools
{
    /// <summary>
    /// A base class for all poolable objects.
    /// </summary>
    public class PoolableObject
    {
        internal bool isPooled;

        /// <summary>
        /// Gets called when the object gets returned to it's pool.
        /// </summary>
        public virtual void OnReturned() { }

        /// <summary>
        /// Gets a value indicating whether this object is currently pooled or not.
        /// </summary>
        public bool IsInPool { get => isPooled; }
    }
}
