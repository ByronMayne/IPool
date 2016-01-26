using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoolSystem
{
    public static class IPoolExtensions
    {
        /// <summary>
        /// Call this method to have the this object be reclaimed by IPool. This will invoke OnDeallocated on this IPool class.
        /// </summary>
        public static void Reclaim(this IPool iPool)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Tells the pool manager to add X amount of new instances to the pool. By default
        /// one instance will be added per frame until the reserve is hit. 
        /// </summary>
        /// <param name="iPool">The object type you want to reserver.</param>
        /// <param name="reserve">The amount you want to be added to the pool</param>
        public static void AddObjectTooPool(this IPool iPool, int reserveAmount = 0)
        {
            throw new System.NotImplementedException();
        }
    }
}