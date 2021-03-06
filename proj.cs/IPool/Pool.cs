﻿using UnityEngine;

namespace PoolSystem
{
  [System.Serializable]
  public class Pool
  {
    /// <summary>
    /// The head object for are deallocated objects 
    /// </summary>
    private PooledObjectStack m_DeallocatedStack = null;

    /// <summary>
    /// The size of our current pool. 
    /// </summary>
    private int m_CurrentPoolSize = 0;

    /// <summary>
    /// The parent object that all this pooled objects will be stored under. 
    /// </summary>
    private Transform m_Parent; 

    /// <summary>
    /// The size that we want to group the pool too. 
    /// </summary>
    private int m_TargetPoolSize = 0;

    /// <summary>
    /// This is the prefab this pool is in control of.  
    /// </summary>
    private GameObject m_Prefab;

    /// <summary>
    /// The path to the resources folder for this asset. 
    /// </summary>
    private string m_ResourcePath;

    /// <summary>
    /// The file path to this pools prefab in the resources folder.
    /// </summary>
    public string resourcePath
    {
      get { return m_ResourcePath; }
    }

    /// <summary>
    /// Create a new instance of a pool and give it a prefab to watch
    /// </summary>
    /// <param name="prefab">The prefab that this pool is in charge of.</param>
    internal Pool(string resourcePath) : this(resourcePath, null) { }

    internal Pool(string resourcePath, Transform parent)
    {
      m_ResourcePath = resourcePath;
      m_Prefab = Resources.Load<GameObject>(resourcePath);
      m_Parent = parent; 

      PoolManager.AddPool(this);

      if (m_Prefab == null)
      {
        throw new System.ArgumentNullException("No prefab could be loaded at the path '" + resourcePath + "'");
      }
      m_DeallocatedStack = new PooledObjectStack(this);
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
      m_DeallocatedStack.Push(iPooledObject);
    }

    public void PrintQueue()
    {
      IPooledObject iterator = m_DeallocatedStack.Peek();

      string printOut = "";

      do
      {
        if (iterator is PooledObjectStack)
        {
          printOut += "{h}";
        }
        else
        {
          printOut += " * ";
        }
        iterator = iterator.next;
      }
      while (iterator.next != null);

    }

    /// <summary>
    /// Gets the next deallocated object. If none is ready it will create a new one
    /// and return that. 
    /// </summary>
    /// <returns>The next available object or creates a new one.</returns>
    internal GameObject GetNextAvaiable()
    {
      IPooledObject poolObj = m_DeallocatedStack.Pop();
      poolObj.gameObject.SetActive(true);
      poolObj.pool = this;
      return poolObj.gameObject;
    }

    /// <summary>
    /// Instantiates a object from the Resource folder. If the object does not have a 
    /// pool create one will be made. Note that any prefab created with this function must
    /// have a PooledObject component at it's root. 
    /// </summary>
    /// <param name="resourcePath">The path to the resources folder.</param>
    /// <returns>The GameObject that was grabbed from the pool.</returns>
    public GameObject Instantiate()
    {
      return Instantiate(Vector3.zero, Quaternion.identity);
    }

    /// <summary>
    /// Instantiates a new prefab from the pool.
    /// </summary>
    /// <param name="resourcePath">The path to the resources folder.</param>
    /// <param name="position">The position where you want it to spawn</param>
    /// <param name="quaternion">The rotation you want it to spawn with.</param>
    /// <returns>The GameObject that was grabbed from the pool.</returns>
    public GameObject Instantiate(Vector3 position, Quaternion ratation)
    {
      GameObject go = GetNextAvaiable();
      go.transform.position = position;
      go.transform.localRotation = ratation;
      return go;
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

      IPooledObject linkedObject = (IPooledObject)newObj.GetComponentInChildren(typeof(IPooledObject), includeInactive: true);

      linkedObject.pool = this;

      if ((Object)linkedObject == null)
      {
        throw new System.NullReferenceException(newObj.name + " does not have a PooledObject component at it's root level");
      }

      newObj.transform.SetParent(m_Parent);

      if (!shouldAllocate)
      {
        m_DeallocatedStack.Push(linkedObject);
      }

      m_CurrentPoolSize++;

      return newObj;
    }

    /// <summary>
    /// This is used to check if we need to update the size of our pool. We have a target size and
    /// a current size. If our current size is less then our target size we will create one new
    /// item each frame until they match.
    /// </summary>
    internal void Update()
    {
      if (m_CurrentPoolSize < m_TargetPoolSize)
      {
        CreatePooledObject(shouldAllocate: false);
      }
    }
  }
}