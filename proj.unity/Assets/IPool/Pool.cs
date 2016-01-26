using UnityEngine;

namespace PoolSystem
{
  public class Pool
  {
    /// <summary>
    /// This is the ID the relates back to our prefabs that we
    /// are pooling. 
    /// </summary>
    private string m_PoolID;

    private int m_TargetPoolSize;
    private int m_CurrentPoolSize;

    /// <summary>
    /// This is the prefab this pool is in control of.  
    /// </summary>
    private GameObject m_Prefab;

    private PooledObjectHead m_AllocatedHead = null;
    private PooledObjectHead m_DeallocatedHead = null;

    public Pool(GameObject prefab)
    {
      m_AllocatedHead = new PooledObjectHead(this);
      m_DeallocatedHead = new PooledObjectHead(this);
      m_Prefab = prefab; 
    }


    public void SetPoolSize(int poolSize)
    {
      m_TargetPoolSize = poolSize;
    }

    public GameObject GetNextAvaiable()
    {
      IPooledObject poolObj = m_DeallocatedHead.PopHead();
      m_AllocatedHead.PushHead(poolObj);
      poolObj.gameObject.SetActive(true);
      return poolObj.gameObject;
    }

    public void Deallocate(IPooledObject iPooledObject)
    {
      iPooledObject.OnDeallocated();
      iPooledObject.gameObject.SetActive(false);
      iPooledObject.RemoveLink();
      m_DeallocatedHead.PushHead(iPooledObject);
    }

    public GameObject CreateObject()
    {
      return CreatePooledObject(shouldAllocate: false);
    }

    public GameObject CreateAndAllocateObject()
    {
      return CreatePooledObject(shouldAllocate: true);
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