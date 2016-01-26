using System;
using UnityEngine;

namespace PoolSystem
{
  public interface IPooledObject
  {
    IPooledObject next { get; set; }
    IPooledObject previous { get; set; }

    Pool pool { get; set; }

    void RemoveLink();
    void InsertLinkAfter(IPooledObject list);

    GameObject gameObject { get; }

    void OnAllocated();
    void OnDeallocated();
  }
}
