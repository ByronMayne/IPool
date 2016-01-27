using System.Collections.Generic;
using UnityEngine;

namespace PoolSystem
{
  public class PoolManager : MonoBehaviour
  {
    /// <summary>
    /// 
    /// </summary>
    private static PoolManager m_Instance;
    internal static PoolManager instance
    {
      get
      {
        Initialize();
        return m_Instance;
      }
    }
    private Dictionary<string, Pool> m_PoolMap;

    /// <summary>
    /// Gets or sets entries into our pool map. This map is used
    /// to link resource paths to pools. One pool for each object
    /// in the resources folder.
    /// </summary>
    private Dictionary<string, Pool> poolMap
    {
      get { return  m_PoolMap; }
      set { m_PoolMap = value; }
    }

    /// <summary>
    /// Call this function to Initialize the Pool Manager if it has not already done so.
    /// </summary>
    [RuntimeInitializeOnLoadMethod]
    public static void Initialize()
    {
      if (m_Instance == null)
      {
        GameObject go = new GameObject("PoolManager");
        m_Instance = go.AddComponent<PoolManager>();
        instance.m_PoolMap = new Dictionary<string, Pool>();
        DontDestroyOnLoad(go);
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
      if(string.IsNullOrEmpty(resourcePath))
      {
        throw new System.ArgumentNullException("ResourcePath must have a valid and can't be null");
      }

      if(instance.poolMap.ContainsKey(resourcePath))
      {
        // We already have a pool created for this item. 
        return instance.poolMap[resourcePath];
      }

      GameObject prefab = Resources.Load<GameObject>(resourcePath);

      if(prefab == null)
      {
        throw new System.ArgumentNullException("Unable to load prefab at '" + resourcePath + "'. Please make sure this object exists");
      }

      PooledObject pooledObj = prefab.GetComponent<PooledObject>();

      if (pooledObj == null)
      {
        pooledObj = prefab.AddComponent<PooledObject>(); 
      }

      // Create a new instance of our pool 
      Pool pool = new Pool(prefab);

      // Set the target size
      pool.SetPoolSize(poolSize);

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
      if(instance.poolMap.ContainsKey(resourcePath))
      {
        return instance.poolMap[resourcePath];
      }
      return null;
    }

    /// <summary>
    /// Instantiates a object from the Resource folder. If the object does not have a 
    /// pool create one will be made. Note that any prefab created with this function must
    /// have a PooledObject component at it's root. 
    /// </summary>
    /// <param name="resourcePath">The path to the resources folder.</param>
    /// <returns>The GameObject that was grabed from the pool.</returns>
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
    /// <returns>The GameObject that was grabed from the pool.</returns>
    public static GameObject Instantiate(string resourcePath, Vector3 position, Quaternion ratation)
    {
      if (!instance.poolMap.ContainsKey(resourcePath))
      {
        GameObject prefab = Resources.Load<GameObject>(resourcePath);

        if(prefab != null)
        {
          instance.poolMap[resourcePath] = new Pool(prefab);
        }
        else
        {
          throw new System.ArgumentNullException("Unable to load prefab at '" + 
            resourcePath + "'. Make sure this object exists and is in the resources folder");
        }
      }
      GameObject go = instance.poolMap[resourcePath].GetNextAvaiable();
      go.transform.position = position;
      go.transform.localRotation = ratation;
      return go; 
    }
  }
}