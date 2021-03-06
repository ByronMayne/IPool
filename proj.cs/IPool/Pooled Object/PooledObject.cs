﻿
using UnityEngine;

namespace PoolSystem
{
  [System.Serializable]
  public class PooledObject : MonoBehaviour, IPooledObject
  {
    private int m_ID = 0;

    private static int m_NextID;

    private Pool m_Pool;

    /// <summary>
    /// Gets or sets this objects pool.
    /// </summary>
    public Pool pool
    {
      get { return m_Pool; }
      set { m_Pool = value; }
    }

    #region -= IPooledObject [LinkedList] =-
    /// <summary>
    /// The IPooledObject item after this one. 
    /// </summary>
    private IPooledObject m_Next;

    /// <summary>
    /// Get or set our next IPooledObject
    /// </summary>
    IPooledObject IPooledObject.next
    {
      get { return m_Next; }
      set { m_Next = value; }
    }

    protected virtual void Start()
    {
      m_ID = m_NextID;
      m_NextID++;
    }

    #endregion 
    public int poolInstanceID
    {
      get
      {
        if (m_ID == -1)
        {
          m_ID = m_NextID;
          m_NextID++;
        }
        return m_ID;
      }
    }

    /// <summary>
    /// previous
    /// Call this function to "destroy" this object. This will then call OnDeallocated
    /// and then put the object back in queue to be used again. 
    /// </summary>
    public void Deallocate()
    {
      pool.Deallocate(this);
    }

    /// <summary>
    /// This is called when this object is about to be reused
    /// by in it's pool. Any reset logic should be placed inside here. 
    /// </summary>
    public virtual void OnAllocated()
    {

    }

    /// <summary>
    /// This is called when the object has been dismissed. And is waiting
    /// to be used again. Any cleanup logic should be placed inside here.
    /// </summary>
    public virtual void OnDeallocated()
    {

    }

    /// <summary>
    /// This is called when the GameObject is about to be destroyed. If we don't clean this up
    /// we will have a broken link in our linked list. 
    /// </summary>
    protected void OnDestroy()
    {
#if UNITY_EDITOR
      if(!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
      {
        return;
      }
#endif
    }
  }
}