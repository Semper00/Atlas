using System.Collections.Concurrent;

namespace Atlas.Pools
{
    /// <summary>
    /// Represents an object pool.
    /// </summary>
    public class ObjectPool<T> where T : PoolableObject, new()
    {
        internal ConcurrentQueue<T> pool;

        internal ObjectPool() { pool = new ConcurrentQueue<T>(); }

        /// <summary>
        /// Gets the shared pool instance for this type.
        /// </summary>
        public static readonly ObjectPool<T> Shared = new ObjectPool<T>();

        /// <summary>
        /// Rents an object from the pool. <b>Make sure to return it using the <see cref="Return(T)"/> method!</b>
        /// </summary>
        /// <returns>The pooled object.</returns>
        public T Rent()
        {
            if (!pool.TryDequeue(out T t))
                t = new T();

            t.isPooled = false;

            return t;
        }

        /// <summary>
        /// Returns an object to the pool.
        /// </summary>
        /// <param name="t">The object to return.</param>
        public void Return(T t)
        {
            t.OnReturned();

            t.isPooled = true;

            pool.Enqueue(t);
        }
    }
}
