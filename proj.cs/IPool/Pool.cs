using UnityEngine;

namespace PoolSystem
{
  [System.Serializable]
  public class Pool
  {
    /// <summary>
    /// The head object for our Allocated objects 
    /// </summary>
    private PooledObjectHead m_AllocatedHead = null;

    /// <summary>
    /// The head object for are deallocated objects 
    /// </summary>
    private PooledObjectHead m_DeallocatedHead = null;

    /// <summary>
    /// The size of our current pool. 
    /// </summary>
    private int m_CurrentPoolSize;

    /// <summary>
    /// The size that we want to group the pool too. 
    /// </summary>
    private int m_TargetPoolSize;

    /// <summary>
    /// This is the prefab this pool is in control of.  
    /// </summary>
    private GameObject m_Prefab;

    /// <summary>
    /// Create a new instance of a pool and give it a prefab to watch
    /// </summary>
    /// <param name="prefab">The prefab that this pool is in charge of.</param>
    internal Pool(GameObject prefab)
    {
      m_AllocatedHead = new PooledObjectHead(this);
      m_DeallocatedHead = new PooledObjectHead(this);
      m_Prefab = prefab; 
    }

    /// <summary>
    /// Create a new pooled object prefab and put it in the allocated queue.
    /// </summary>
    /// <returns></returns>
    public GameObject CreateAndAllocateObject()
    {
      return CreatePooledObject(shouldAllocate: true);
    }

    /// <summary>
    /// Create a new pooled object and put in the deallocated queue. 
    /// </summary>
    /// <returns></returns>
    public GameObject CreateObject()
    {
      return CreatePooledObject(shouldAllocate: false);
    }

    /// <summary>
    /// When ever a IPooledObject is done being used wit it call this function
    /// which takes it from the allocated queue (if it's there) and adds it to
    /// the deallocated queue. 
    /// </summary>
    /// <param name="iPooledObject">The item you want to deallocate.</param>
    public void Deallocate(IPooledObject iPooledObject)
    {
      iPooledObject.OnDeallocated();
      iPooledObject.gameObject.SetActive(false);
      iPooledObject.RemoveLink();
      m_DeallocatedHead.PushHead(iPooledObject);
    }

    /// <summary>
    /// Gets the next deallocated object. If none is ready it will create a new one
    /// and return that. 
    /// </summary>
    /// <returns>The next available object or creates a new one.</returns>
    public GameObject GetNextAvaiable()
    {
      IPooledObject poolObj = m_DeallocatedHead.PopHead();
      m_AllocatedHead.PushHead(poolObj);
      poolObj.gameObject.SetActive(true);
      return poolObj.gameObject;
    }

    /// <summary>
    /// Sets the pools target pool size to this value. The target size
    /// is used to grow the queue size over a course of time. Rather 
    /// then creating all the instances at once. 
    /// </summary>
    /// <param name="poolSize">The size you would like the queue to be.</param>
    public void SetPoolSize(int poolSize)
    {
      m_TargetPoolSize = poolSize;
    }

    /// <summary>
    /// Creates a new PooledObject.
    /// </summary>
    private GameObject CreatePooledObject(bool shouldAllocate)
    {
      GameObject newObj = Object.Instantiate(m_Prefab);

      IPooledObject linkedObject = newObj.GetComponent<PooledObject>();

      linkedObject.pool = this;

      if ((Object)linkedObject == null)
      {
        throw new System.NullReferenceException(newObj.name + " does not have a PooledObject component at it's root level");
      }

      if (shouldAllocate)
      {
        m_AllocatedHead.PushHead(linkedObject);
      }
      else
      {
        m_DeallocatedHead.PushHead(linkedObject);
      }

      return newObj;
    }
  }
}