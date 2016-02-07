using UnityEngine;
using System.Collections;
using PoolSystem;

public class Test_ReusePrefabs : MonoBehaviour
{
  private Pool m_Pool;
  private int m_SpawnCount = 5;

  public void OnEnable()
  {
    m_Pool = PoolManager.CreatePool(PrefabPaths.CAPSULE, poolSize: 0, parent: transform);


    StartCoroutine(TestQueueInit());
  }

  private IEnumerator TestQueueInit()
  {
    for(int i = 0; i < m_SpawnCount; i++)
    {
      m_Pool.Instantiate();
    }

    yield return new WaitForSeconds(2.5f);

    for (int i = 0; i < m_SpawnCount; i++)
    {
      m_Pool.Instantiate();
    }

    if(transform.childCount > m_SpawnCount)
    {
      IntegrationTest.Fail("To many prefabs were created");
    }
    else if (transform.childCount < m_SpawnCount)
    {
      IntegrationTest.Fail("To few prefabs were created");
    }
    else
    {
      IntegrationTest.Pass();
    }
  }
}
