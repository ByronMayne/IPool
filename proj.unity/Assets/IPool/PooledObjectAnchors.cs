using System;
using UnityEngine;

namespace PoolSystem
{
  public class PooledObjectHead : IPooledObject
  {
    public PooledObjectHead(Pool pool)
    {
      PooledObjectTail tail = new PooledObjectTail();
      IPooledObject @this = this;

      @this.pool = pool;
      this.m_Head = tail;
    }

    //No GameObjects
    public GameObject gameObject
    {
      get
      {
        return null;
      }
    }

    private Pool m_Pool;

    private IPooledObject m_Head;

    //Not used
    IPooledObject IPooledObject.next
    {
      get { return m_Head; }
      set { }
    }

    public Pool pool
    {
      get
      {
        return m_Pool;
      }
      set
      {
        m_Pool = value;
      }
    }

    public IPooledObject previous
    {
      get
      {
        return null;
      }

      set
      {
        //Don't do anything
      }
    }


    public IPooledObject PopHead()
    {
      if (m_Head is PooledObjectTail || m_Head is PooledObjectHead)
      {
        Debug.Log("Making new instance since head is tail");
        //We have nothing left in the queue. 
        GameObject newObj = m_Pool.CreateAndAllocateObject();
        return (IPooledObject)newObj.GetComponent(typeof(IPooledObject)); ;
      }
      else
      {
        Debug.Log("<color=green><b> REUSING </b></color>");
        IPooledObject oldHead = m_Head;
        m_Head.RemoveLink();
        m_Head = oldHead.previous;
        return oldHead;
      }
    }

    public void PushHead(IPooledObject obj)
    {
      if(m_Head is PooledObjectTail)
      {
        obj.next = m_Head;
        m_Head = obj;
        obj.previous = this; 
      }
      else
      {
        obj.InsertLinkAfter(m_Head);
        m_Head = obj;
      }
    }

    public void InsertLinkAfter(IPooledObject list)
    {
      //Don't do anything
    }

    public void OnAllocated()
    {
      //Don't do anything
    }

    public void OnDeallocated()
    {
      //Don't do anything
    }

    public void RemoveLink()
    {
      //Don't do anything
    }
  }

  public class PooledObjectTail : IPooledObject
  {
    #region  -= IPooled Object (Not Used) =-
    GameObject IPooledObject.gameObject
    {
      get
      {
        throw new NotImplementedException();
      }
    }
    private Pool m_Pool;

    public Pool pool
    {
      get
      {
        return m_Pool;
      }
      set
      {
        m_Pool = value;
      }
    }

    void IPooledObject.OnAllocated()
    {
      throw new NotImplementedException();
    }

    void IPooledObject.OnDeallocated()
    {
      throw new NotImplementedException();
    }

    void IPooledObject.RemoveLink()
    {
      throw new NotImplementedException();
    }
    #endregion

    public void InsertLinkAfter(IPooledObject head)
    {
      //This should only be inserted after Head
      if (head is PooledObjectHead)
      {
        head.next = this;
        previous = head;
      }
      else
      {
        throw new System.Exception("Tail can only be inserted after Head");
      }
    }

    private IPooledObject m_Previous;

    public IPooledObject next
    {
      get
      {
        return this;
      }

      set
      {
        //Don't Do Anything. 
      }
    }

    public IPooledObject previous
    {
      get
      {
        return m_Previous;
      }

      set
      {
        m_Previous = value;
      }
    }
  }
}
