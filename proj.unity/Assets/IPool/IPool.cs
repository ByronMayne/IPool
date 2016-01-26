using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PoolSystem
{
  public interface IPool
  {
    /// <summary>
    /// The pool ID is used to figure out how objects should be pooled. By default when
    /// you call Couple(this) it will check the pool ID and group them with other objects 
    /// with the same ID.
    /// </summary>
    string poolID { get; }

    /// <summary>
    /// Called when this object is about to be reused. Any reset logic should be contained 
    /// inside of here.
    /// </summary>
    void OnAllocated();

    /// <summary>
    /// Called when the object is about to be put back in the pool. Any logic to turn 
    /// off and unsubscripted functions should be put inside of here. After this function is 
    /// called the object will be disabled the next frame.
    /// </summary>
    void OnDeallocated();

    /// <summary>
    /// Gets the GameObject that belongs to this object. By default this will just be happy with 
    /// the Monobehaviour class.
    /// </summary>
    GameObject gameObject { get; }
  }
}