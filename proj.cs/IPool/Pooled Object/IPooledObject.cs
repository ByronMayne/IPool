using System;
using UnityEngine;

namespace PoolSystem
{
  public interface IPooledObject
  {
    IPooledObject next { get; set; }

    Pool pool { get; set; }

    GameObject gameObject { get; }

    void OnAllocated();
    void OnDeallocated();
  }
}
