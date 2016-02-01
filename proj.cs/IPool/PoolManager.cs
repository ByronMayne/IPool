using System;
using System.Collections.Generic;
using UnityEngine;

namespace PoolSystem
{
  public class PoolManager
  {
    public PoolManager()
    {
      m_Pools = new List<Pool>();
    }

    private List<Pool> m_Pools;

    /// <summary>
    /// Gets or sets entries into our pool map. This map is used
    /// to link resource paths to pools. One pool for each object
    /// in the resources folder.
    /// </summary>
    private List<Pool> pools
    {
      get { return m_Pools; }
      set { m_Pools = value; }
    }

    private Pool this[string resourcePath]
    {
      get
      {
        for (int i = 0; i < m_Pools.Count; i++)
        {
          if (string.Compare(m_Pools[i].resourcePath, resourcePath, System.StringComparison.Ordinal) == 0)
          {
            return m_Pools[i];
          }
        }
        return null;
      }
    }

    /// <summary>
    /// This is called from <see cref="PoolBehaviour"/> and is used to allow every
    /// pool to have an update function.
    /// </summary>
    internal void UpdatePools()
    {
      for (int i = 0; i < m_Pools.Count; i++)
      {
        m_Pools[i].Update();
      }
    }

    /// <summary>
    /// Creates a new Pool object and returns it back to you.
    /// </summary>
    /// <param name="resourcePath">The path to the prefab in the resource folder that this pool controls.</param>
    /// <returns>The newly created pool or an old one if it is already a thing.</returns>
    public static Pool CreatePool(string resourcePath)
    {
      return CreatePool(resourcePath, 0);
    }

    /// <summary>
    /// Creates a new Pool object and returns it back to you.
    /// </summary>
    /// <param name="resourcePath">The path to the prefab in the resource folder that this pool controls.</param>
    /// <param name="poolSize">The target size that you want the pool to be.</param>
    /// <returns>The newly created pool or an old one if it is already a thing.</returns>
    public static Pool CreatePool(string resourcePath, int poolSize)
    {
      if (string.IsNullOrEmpty(resourcePath))
      {
        throw new System.ArgumentNullException("ResourcePath must have a valid and can't be null");
      }

      Pool pool = PoolBehaviour.instance[resourcePath];


      if (pool != null)
      {
        // We already have a pool created for this item. 
        return pool;
      }

      GameObject prefab = Resources.Load<GameObject>(resourcePath);

      if (prefab == null)
      {
        throw new System.ArgumentNullException("Unable to load prefab at '" + resourcePath + "'. Please make sure this object exists");
      }

      PooledObject pooledObj = prefab.GetComponent<PooledObject>();

      if (pooledObj == null)
      {
        pooledObj = prefab.AddComponent<PooledObject>();
      }

      // Create a new instance of our pool 
      pool = new Pool(resourcePath);

      // Set the target size
      pool.SetPoolSize(poolSize);

      //Add our pool to our list of pools. 
      PoolBehaviour.instance.m_Pools.Add(pool);

      //Return it back to the client. 
      return pool;
    }

    /// <summary>
    /// Given a resourcePath this finds the pool that is in charge of that 
    /// resource. If none exists null is returned. 
    /// </summary>
    /// <param name="resourcePath">The path to the Resource that you want the pool for.</param>
    /// <returns>The pool that is in control of the resource at the path you sent 
    /// in or null if it does not exists.</returns>
    public static Pool GetPool(string resourcePath)
    {
      return PoolBehaviour.instance[resourcePath];
    }

    /// <summary>
    /// Instantiates a object from the Resource folder. If the object does not have a 
    /// pool create one will be made. Note that any prefab created with this function must
    /// have a PooledObject component at it's root. 
    /// </summary>
    /// <param name="resourcePath">The path to the resources folder.</param>
    /// <returns>The GameObject that was grabbed from the pool.</returns>
    public static GameObject Instantiate(string resourcePath)
    {
      return Instantiate(resourcePath, Vector3.zero, Quaternion.identity);
    }

    /// <summary>
    /// Instantiates a object from the Resource folder. If the object does not have a 
    /// pool create one will be made. Note that any prefab created with this function must
    /// have a PooledObject component at it's root. 
    /// </summary>
    /// <param name="resourcePath">The path to the resources folder.</param>
    /// <param name="position">The position where you want it to spawn</param>
    /// <param name="quaternion">The rotation you want it to spawn with.</param>
    /// <returns>The GameObject that was grabbed from the pool.</returns>
    public static GameObject Instantiate(string resourcePath, Vector3 position, Quaternion ratation)
    {
      Pool pool = PoolBehaviour.instance[resourcePath];

      if (pool == null)
      {
        pool = CreatePool(resourcePath);
      }

      GameObject go = pool.GetNextAvaiable();
      go.transform.position = position;
      go.transform.localRotation = ratation;
      return go;
    }
  }
}