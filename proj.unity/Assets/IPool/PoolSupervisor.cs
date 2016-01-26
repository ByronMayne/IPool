using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PoolSystem
{
  public class PoolSupervisor
  {
    private GameObject m_Target;

    private bool m_IsActive;

    private IPool m_IPool;

    public GameObject target
    {
      get
      {
        return m_Target;
      }
    }

    public IPool iPool
    {
      get
      {
        return m_IPool;
      }
      set
      {
        m_IPool = value;
      }
    }
 
    public void AssignTarget(GameObject target)
    {
      target.SetActive(false);
      m_IsActive = false;
    }

    public bool isActive
    {
      get
      {
        return m_IsActive;
      }
    }

    public bool isReadyForAllocation
    {
      get
      {
        if (!isActive && m_Target != null)
        {
          return true;
        }
        return false;
      }
    }

    public string poolID
    {
      get
      {
        return iPool.poolID;
      }
    }

    public void OnAllocated()
    {
      iPool.OnAllocated();
      m_IsActive = true;
    }

    public void OnDeallocated()
    {
      iPool.OnDeallocated();
      m_IsActive = false;
    }
  }
}