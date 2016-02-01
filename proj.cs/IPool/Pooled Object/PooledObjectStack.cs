using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PoolSystem
{
  public class PooledObjectStack : IEnumerable<IPooledObject>, IEnumerable
  {
    public PooledObjectStack(Pool pool)
    {
      m_Pool = pool;
      m_Head = null;
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

    public IPooledObject Pop()
    {
      if (m_Head == null)
      {
        //We have nothing left in the queue. 
        GameObject newObj = m_Pool.CreateAndAllocateObject();
        return (IPooledObject)newObj.GetComponent(typeof(IPooledObject)); ;
      }
      else
      {
        IPooledObject oldHead = m_Head;
        m_Head = oldHead.next;
        return oldHead;
      }
    }

    public IPooledObject Peek()
    {
      return m_Head;
    }

    public void Push(IPooledObject newHead)
    {
      if (m_Head == null)
      {
        m_Head = newHead;
      }
      else
      {
        IPooledObject oldHead = m_Head;
        newHead.next = m_Head;
        m_Head = newHead;
      }
    }

    public IEnumerator<IPooledObject> GetEnumerator()
    {
      IPooledObject iterator = m_Head;

      if (iterator == null)
      {
        yield break;
      }

      do
      {
        yield return iterator;
      } while (iterator.next != null);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      IPooledObject iterator = m_Head;

      if (iterator == null)
      {
        yield break;
      }

      do
      {
        yield return iterator;
      } while (iterator.next != null);
    }
  }
}
