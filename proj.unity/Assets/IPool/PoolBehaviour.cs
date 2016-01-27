using UnityEngine;

namespace PoolSystem
{
  public class PoolBehaviour : MonoBehaviour
  {
    private static PoolManager m_Instance;

    public static PoolManager instance
    {
      get
      {
        return m_Instance;
      }
    }

    private void Awake()
    {
      m_Instance = new PoolManager();
    }

    /// <summary>
    /// Call this function to Initialize the Pool Manager if it has not already done so.
    /// </summary>
    [RuntimeInitializeOnLoadMethod]
    public static void Initialize()
    {
      if (instance == null)
      {
        GameObject go = new GameObject("PoolManager");
        go.AddComponent<PoolBehaviour>();
        DontDestroyOnLoad(go);
      }
    }
  }
}
