﻿using UnityEngine;
using PoolSystem;
using System.Collections;

[IntegrationTest.DynamicTest("Scenes/IPoolUnitTests.unity")]
public class Test_CreatePrefabsWithPool : MonoBehaviour
{
  private Pool m_Pool;

  public void Awake()
  {
    m_Pool = PoolManager.CreatePool(PrefabPaths.SPHERE_PATH, poolSize:0, parent:transform);

    StartCoroutine(TestRoutine());
  }


  private IEnumerator TestRoutine()
  {
    for (int i = 0; i < 10; i++)
    {
      GameObject go = m_Pool.Instantiate();

      go.transform.SetParent(transform);

      PooledObject pooled = go.GetComponent<PooledObject>();

      yield return new WaitForSeconds(0.05f);

      pooled.Deallocate();

      yield return new WaitForSeconds(0.05f);
    }

    yield return new WaitForSeconds(0.5f);

    if(transform.childCount < 2)
    {
      //We should just reuse the same object
      IntegrationTest.Pass();
    }
    else
    {
      IntegrationTest.Fail(gameObject, "Too many children were spawned. There should only be one.");
    }
  }
}
