using System;
using System.Collections.Generic;
using UnityEngine;

namespace PoolSystem
{
  public class PoolManager : MonoBehaviour
  {
    /// <summary>
    /// Gets or sets the HashSet that contains a list of our
    /// <see cref="PooledObject"/>s. 
    /// </summary>
    public HashSet<Pool> pool
    {
      get
      {
        throw new System.NotImplementedException();
      }

      set
      {
      }
    }

    private static PoolManager m_Instance;

    internal static PoolManager instance
    {
      get
      {
        return m_Instance;
      }
    }


    private Dictionary<string, string> m_ResourcesMap;
    private Dictionary<string, GameObject> m_PrefabMap;
    private Dictionary<string, Pool> m_Pools; 

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
        instance.m_ResourcesMap = new Dictionary<string, string>();
        instance.m_PrefabMap = new Dictionary<string, GameObject>();
        instance.m_Pools = new Dictionary<string, Pool>();
        DontDestroyOnLoad(go);
      }
    }
    
    public static GameObject InstantiateByID(string m_PoolID)
    {
      if (instance.m_PrefabMap.ContainsKey(m_PoolID))
      {
        return GameObject.Instantiate(instance.m_PrefabMap[m_PoolID]);
      }
      else
      {
        if (instance.m_ResourcesMap.ContainsKey(m_PoolID))
        {
          GameObject go = Resources.Load<GameObject>(instance.m_ResourcesMap[m_PoolID]);

          if (go != null)
          {
            instance.m_PrefabMap[m_PoolID] = go;
            return go;
          }
        }
      }
      return null;
    }
  }
}