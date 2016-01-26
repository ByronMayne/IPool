using NUnit.Framework;
using PoolSystem;
using System;
using UnityEngine;

namespace IPoolUnitTests
{
  [TestFixture]
  public class PoolManagerTests
  {
    public class ClassA : PooledObject
    {
      public bool OnAllocatedCalled { get; private set; }
      public bool OnDeallocatedCalled { get; private set; }

      public string poolID
      {
        get
        {
          return "ClassA";
        }
      }

      public override void OnAllocated()
      {
        OnAllocatedCalled = true;
      }

      public override void OnDeallocated()
      {
        OnDeallocatedCalled = true;
      }
    }

    public class ClassB : PooledObject
    {
      public bool OnAllocatedCalled { get; private set; }
      public bool OnDeallocatedCalled { get; private set; }

      public string poolID
      {
        get
        {
          return "ClassB";
        }
      }

      public override void OnAllocated()
      {
        OnAllocatedCalled = true;
      }

      public override void OnDeallocated()
      {
        OnDeallocatedCalled = true;
      }
    }

    public class ClassC : PooledObject
    {
      public bool OnAllocatedCalled { get; private set; }
      public bool OnDeallocatedCalled { get; private set; }

      public string poolID
      {
        get
        {
          return "ClassC";
        }
      }

      public override void OnAllocated()
      {
        OnAllocatedCalled = true;
      }

      public override void OnDeallocated()
      {
        OnDeallocatedCalled = true;
      }
    }

    [Test]
    public void CreateInstance()
    {
      PoolManager.Initialize();

      Assert.NotNull(PoolManager.instance, "Pool Manager is null after Initialize was called");
    }

    [Test]
    public void CreatePool()
    {
      GameObject prefabA = new GameObject("Prefab A");
      GameObject prefabB = new GameObject("Prefab B");
      GameObject prefabC = new GameObject("Prefab C");

      prefabA.AddComponent<ClassA>();
      prefabB.AddComponent<ClassB>();
      prefabC.AddComponent<ClassC>();

      Pool PoolA = new Pool(prefabA);

      GameObject go = PoolA.GetNextAvaiable();

    }

    [Test]
    public void CreateClass()
    {
    }

  }
}
